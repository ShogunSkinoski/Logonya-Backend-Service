using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Messaging;
public class KafkaTopicInitializer : IHostedService
{
    private readonly KafkaTopicConfig _topicConfig;
    private readonly ILogger<KafkaTopicInitializer> _logger;

    public KafkaTopicInitializer(
        KafkaTopicConfig topicConfig,
        ILogger<KafkaTopicInitializer> logger)
    {
        _topicConfig = topicConfig;
        _logger = logger;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Starting Kafka topic initialization");
            await _topicConfig.CreateTopicsAsync();
            _logger.LogInformation("Kafka topic initialization completed");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to initialize Kafka topics");
            throw;
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}