namespace Domain.Common;

public class TopicConfig
{
    public string Name { get; set; }
    public string KeyPrefix { get; set; }
    public int PartitionCount { get; set; }
    public short ReplicationFactor { get; set; }
}
