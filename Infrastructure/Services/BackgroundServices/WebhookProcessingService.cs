using Application.Common;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services;

public class WebhookProcessingService : BackgroundService
{
    private readonly ILogger<WebhookProcessingService> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly IHostApplicationLifetime _applicationLifetime;

    public WebhookProcessingService(
        ILogger<WebhookProcessingService> logger,
        IServiceProvider serviceProvider,
        IHostApplicationLifetime applicationLifetime)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
        _applicationLifetime = applicationLifetime;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            await Task.Run(async () =>
            {
                var startedSource = new TaskCompletionSource<bool>();
                using var reg = _applicationLifetime.ApplicationStarted.Register(() => startedSource.SetResult(true));
                await startedSource.Task;
            }, stoppingToken);

            _logger.LogInformation("Starting webhook processing service");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var webhookService = scope.ServiceProvider.GetRequiredService<IWebhookService>();
                        await webhookService.ProcessWebhookQueueAsync(stoppingToken);
                    }
                    await Task.Delay(TimeSpan.FromSeconds(1), stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error processing webhook queue");
                    await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
                }
            }
        }
        catch (OperationCanceledException)
        {
            _logger.LogInformation("Webhook processing service stopped");
        }
    }
}