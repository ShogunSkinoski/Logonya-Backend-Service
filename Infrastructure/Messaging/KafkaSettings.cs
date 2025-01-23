using Domain.Common;

namespace Infrastructure.Messaging;

public class KafkaSettings: IKafkaSettings
{
    public string BootstrapServers { get; set; }
    public Dictionary<string, TopicConfig> Topics { get; set; } = new();
    IReadOnlyDictionary<string, TopicConfig> IKafkaSettings.Topics => Topics;
}
