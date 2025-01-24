using Application.Common;
using Application.Usecases.Logging.CreateAlertCommand;
using Domain.Logging.Port;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Infrastructure.Services.BackgroundServices
{
    public class AlertMonitoringService : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ILogger<AlertMonitoringService> _logger;
        private readonly TimeSpan _checkInterval = TimeSpan.FromMinutes(1);
        private readonly Dictionary<Guid, bool> _alertStates = new();
        private readonly IHostApplicationLifetime _applicationLifetime;

        public AlertMonitoringService(
            IServiceScopeFactory serviceScopeFactory,
            ILogger<AlertMonitoringService> logger,
            IHostApplicationLifetime applicationLifetime)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
            _applicationLifetime = applicationLifetime;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Run(async () =>
            {
                var startedSource = new TaskCompletionSource<bool>();
                using var reg = _applicationLifetime.ApplicationStarted.Register(() => startedSource.SetResult(true));
                await startedSource.Task;
            }, stoppingToken);
            _logger.LogInformation("Starting alert monitoring service");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = _serviceScopeFactory.CreateScope();
                    var alertRepository = scope.ServiceProvider.GetRequiredService<AlertRepositoryPort>();
                    var logRepository = scope.ServiceProvider.GetRequiredService<LogRepositoryPort>();
                    var webhookService = scope.ServiceProvider.GetRequiredService<IWebhookService>();

                    var activeAlerts = await alertRepository.GetAllAsync(
                        filter: x => x.IsActive,
                        cancellationToken: stoppingToken
                    );

                    foreach (var alert in activeAlerts)
                    {
                        var condition = JsonSerializer.Deserialize<AlertCondition>(alert.Condition);
                        if (condition == null) continue;
                        
                        var errorCount = await EvaluateMetric(
                            condition.MetricType,
                            condition.CustomParameters,
                            alert.UserId,
                            DateTime.UtcNow.AddMinutes(-condition.TimeWindowMinutes),
                            stoppingToken
                        );

                        var shouldTrigger = CompareValue(errorCount, condition.Operator, condition.Threshold);
                        _alertStates.TryGetValue(alert.Id, out var wasTriggered);

                        if (shouldTrigger && !wasTriggered)
                        {
                            var payload = new AlertTriggeredPayload
                            {
                                AlertId = alert.Id,
                                AlertName = alert.Name,
                                Description = alert.Description,
                                Condition = condition,
                                ErrorCount = errorCount,
                                TriggeredAt = DateTime.UtcNow
                            };

                            await webhookService.SendWebhookAsync(
                                WebhookEvents.ALERT_TRIGGERED,
                                payload,
                                alert.UserId,
                                stoppingToken);

                            alert.Trigger();
                            alertRepository.Update(alert);
                            await alertRepository.SaveChangesAsync(stoppingToken);
                            _alertStates[alert.Id] = true;
                        }
                        else if (!shouldTrigger && wasTriggered)
                        {
                            var payload = new AlertResolvedPayload
                            {
                                AlertId = alert.Id,
                                AlertName = alert.Name,
                                Description = alert.Description,
                                ErrorCount = errorCount,
                                ResolvedAt = DateTime.UtcNow,
                                TriggeredAt = alert.LastTriggeredAt!.Value
                            };

                            await webhookService.SendWebhookAsync(
                                WebhookEvents.ALERT_RESOLVED,
                                payload,
                                alert.UserId,
                                stoppingToken);

                            _alertStates[alert.Id] = false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error in alert monitoring service");
                }

                await Task.Delay(_checkInterval, stoppingToken);
            }
        }

        private async Task<double> EvaluateMetric(
            MetricType metricType,
            Dictionary<string, string>? customParameters,
            Guid userId,
            DateTime since,
            CancellationToken cancellationToken)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var logRepository = scope.ServiceProvider.GetRequiredService<LogRepositoryPort>();

            return metricType switch
            {
                MetricType.ERROR_COUNT => await logRepository.GetErrorCountForUserSince(userId, since, cancellationToken),
                MetricType.ERROR_RATE => await logRepository.GetErrorRateForUserSince(userId, since, cancellationToken),
                MetricType.RESPONSE_TIME => await logRepository.GetAverageResponseTimeForUserSince(userId, since, cancellationToken),
                MetricType.API_ERRORS => await logRepository.GetApiErrorCountForUserSince(
                    userId, 
                    since,
                    customParameters?.GetValueOrDefault("apiPath"),
                    customParameters?.GetValueOrDefault("errorCode"),
                    cancellationToken),
                MetricType.CUSTOM_EVENT => await logRepository.GetCustomEventCountForUserSince(
                    userId,
                    since,
                    customParameters?.GetValueOrDefault("eventName"),
                    customParameters?.GetValueOrDefault("eventValue"),
                    cancellationToken),
                _ => throw new NotSupportedException($"Metric type {metricType} is not supported")
            };
        }

        private bool CompareValue(double value, OperatorType op, double threshold)
        {
            return op switch
            {
                OperatorType.GREATER_THAN => value > threshold,
                OperatorType.LESS_THAN => value < threshold,
                OperatorType.EQUALS => Math.Abs(value - threshold) < 0.0001,
                OperatorType.GREATER_THAN_OR_EQUALS => value >= threshold,
                OperatorType.LESS_THAN_OR_EQUALS => value <= threshold,
                _ => false
            };
        }
    }

    public class AlertTriggeredPayload
    {
        public Guid AlertId { get; init; }
        public string AlertName { get; init; }
        public string Description { get; init; }
        public AlertCondition Condition { get; init; }
        public double ErrorCount { get; init; }
        public DateTime TriggeredAt { get; init; }
    }

    public class AlertResolvedPayload
    {
        public Guid AlertId { get; init; }
        public string AlertName { get; init; }
        public string Description { get; init; }
        public double ErrorCount { get; init; }
        public DateTime TriggeredAt { get; init; }
        public DateTime ResolvedAt { get; init; }
    }
} 