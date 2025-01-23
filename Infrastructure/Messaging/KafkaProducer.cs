using Confluent.Kafka;
using Domain.Common;
using Newtonsoft.Json;

namespace Infrastructure.Messaging;

public class KafkaProducer : IMessagingProducer, IDisposable
{
    private readonly IProducer<string, string> _kafkaProducer;
    private readonly IKafkaSettings _settings;
    private readonly ConsumerConfig _consumerConfig;

    public KafkaProducer(IKafkaSettings settings)
    {
        _settings = settings;
        var producerConfig = new ProducerConfig
        {
            BootstrapServers = settings.BootstrapServers,
            EnableDeliveryReports = true,
            EnableIdempotence = true,
            Acks = Acks.All
        };
        _kafkaProducer = new ProducerBuilder<string, string>(producerConfig).Build();

        _consumerConfig = new ConsumerConfig
        {
            BootstrapServers = settings.BootstrapServers,
            GroupId = "webhook-consumer-group",
            AutoOffsetReset = AutoOffsetReset.Earliest
        };
    }

    public void SendMessage<T>(string topic, string key, T message)
    {
        // Validate topic exists in settings
        if (!_settings.Topics.Any(t => t.Value.Name == topic))
        {
            throw new ArgumentException($"Topic {topic} is not configured in settings");
        }

        var serializedMessage = JsonConvert.SerializeObject(message);
        _kafkaProducer.Produce(topic, new Message<string, string>
        {
            Key = key,
            Value = serializedMessage,
            Timestamp = new Timestamp(DateTime.UtcNow)
        });
    }

    public async Task<T?> ConsumeMessage<T>(string topic, CancellationToken cancellationToken = default)
    {
        using var consumer = new ConsumerBuilder<string, string>(_consumerConfig)
            .SetValueDeserializer(Deserializers.Utf8)
            .Build();

        consumer.Subscribe(topic);

        try
        {
            var consumeResult = consumer.Consume(cancellationToken);
            if (consumeResult == null) return default;

            return JsonConvert.DeserializeObject<T>(consumeResult.Message.Value);
        }
        catch (Exception ex)
        {
            return default;
        }
        finally
        {
            consumer.Close();
        }
    }

    public void Dispose()
    {
        _kafkaProducer?.Dispose();
    }
}