using Confluent.Kafka.Admin;
using Confluent.Kafka;
using Domain.Common;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Messaging;

public class KafkaTopicConfig
{
    private readonly IKafkaSettings _settings;
    private readonly ILogger<KafkaTopicConfig> _logger;

    public KafkaTopicConfig(
        IKafkaSettings settings,
        ILogger<KafkaTopicConfig> logger)
    {
        _settings = settings;
        _logger = logger;
    }

    public async Task CreateTopicsAsync()
    {
        var adminConfig = new AdminClientConfig
        {
            BootstrapServers = _settings.BootstrapServers
        };

        using var adminClient = new AdminClientBuilder(adminConfig).Build();
        try
        {
            var metadata = adminClient.GetMetadata(TimeSpan.FromSeconds(10));
            var existingTopics = metadata.Topics.Select(t => t.Topic).ToList();

            var topicsToCreate = new List<TopicSpecification>();

            foreach (var topic in _settings.Topics)
            {
                if (!existingTopics.Contains(topic.Value.Name))
                {
                    _logger.LogInformation("Preparing to create topic: {TopicName}", topic.Value.Name);

                    topicsToCreate.Add(new TopicSpecification
                    {
                        Name = topic.Value.Name,
                        NumPartitions = topic.Value.PartitionCount,
                        ReplicationFactor = topic.Value.ReplicationFactor
                    });
                }
                else
                {
                    _logger.LogInformation("Topic already exists: {TopicName}", topic.Value.Name);
                }
            }

            if (topicsToCreate.Any())
            {
                _logger.LogInformation("Creating {Count} topics", topicsToCreate.Count);
                await adminClient.CreateTopicsAsync(topicsToCreate);
                _logger.LogInformation("Topics created successfully");
            }
        }
        catch (CreateTopicsException ex)
        {
            _logger.LogError(ex, "Error creating topics: {Message}", ex.Message);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error during topic creation: {Message}", ex.Message);
            throw;
        }
    }

    public async Task DeleteTopicsAsync(params string[] topics)
    {
        var adminConfig = new AdminClientConfig
        {
            BootstrapServers = _settings.BootstrapServers
        };

        using var adminClient = new AdminClientBuilder(adminConfig).Build();
        try
        {
            _logger.LogInformation("Deleting topics: {Topics}", string.Join(", ", topics));
            await adminClient.DeleteTopicsAsync(topics);
            _logger.LogInformation("Topics deleted successfully");
        }
        catch (DeleteTopicsException ex)
        {
            _logger.LogError(ex, "Error deleting topics: {Message}", ex.Message);
            throw;
        }
    }
}

