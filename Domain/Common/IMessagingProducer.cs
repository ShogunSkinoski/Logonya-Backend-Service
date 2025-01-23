namespace Domain.Common;

public interface IMessagingProducer: IDisposable
{
    public void SendMessage<T>(string topic, string key, T message);
    public Task<T?> ConsumeMessage<T>(string topic, CancellationToken cancellationToken = default);
}
