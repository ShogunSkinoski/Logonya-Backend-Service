using Domain.Chat.Model;
using Domain.Chat.Port;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repository.Chat;

public class EfChatMessageRepository(ApplicationDbContext context) : GenericRepository<ChatMessage>(context), ChatMessageRepositoryPort
{
    public async Task<IEnumerable<ChatMessage>> GetChatMessagesAsync(Guid chatHistoryId, CancellationToken cancellationToken = default)
    {
        return await _context.Set<ChatMessage>()
            .Where(m => m.ChatHistoryId == chatHistoryId)
            .OrderBy(m => m.CreatedAt)
            .ToListAsync(cancellationToken);
    }
} 