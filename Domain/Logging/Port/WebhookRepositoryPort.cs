using Domain.Common;
using Domain.Logging.Model;

namespace Domain.Logging.Port;

public interface WebhookRepositoryPort : IGenericRepository<Webhook>
{
    Task<IEnumerable<Webhook>> GetActiveWebhooksByEventAsync(string eventType, CancellationToken cancellationToken = default);
} 