namespace Application.Common;

public interface IWebhookService
{
    Task SendWebhookAsync(string eventType, object payload, Guid userId, CancellationToken cancellationToken = default);
    Task ProcessWebhookQueueAsync(CancellationToken cancellationToken = default);
} 