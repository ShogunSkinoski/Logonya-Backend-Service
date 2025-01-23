using Domain.Logging.Model;
using Domain.Logging.Port;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repository.Logging;

public class EfWebhookRepository : GenericRepository<Webhook>, WebhookRepositoryPort
{
    private readonly ApplicationDbContext _context;

    public EfWebhookRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Webhook>> GetActiveWebhooksByEventAsync(
        string eventType, 
        CancellationToken cancellationToken = default)
    {
        // First get all active webhooks
        var activeWebhooks = await _context.Set<Webhook>()
            .Where(w => w.IsActive)
            .ToListAsync(cancellationToken);

        // Then filter by event type in memory
        return activeWebhooks.Where(w => 
            w.Events.Contains(eventType));
    }
} 