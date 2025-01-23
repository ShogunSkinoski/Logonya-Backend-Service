namespace Domain.Common;

public interface IKafkaSettings
{
    string BootstrapServers { get; }
    IReadOnlyDictionary<string, TopicConfig> Topics { get; }
}

