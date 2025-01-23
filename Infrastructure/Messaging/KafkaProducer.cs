using Confluent.Kafka;
using Domain.Common;
using Newtonsoft.Json;

namespace Infrastructure.Messaging;

public class KafkaProducer : IMessagingProducer, IDisposable
{
    private readonly IProducer<string, string> _kafkaProducer;
    private readonly IKafkaSettings _settings;

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

    public void Dispose()
    {
        _kafkaProducer?.Dispose();
    }
}