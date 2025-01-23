using Domain.Chat.Model;
using Domain.Chat.Port;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repository.Chat;

public class EfChatHistoryRepository(ApplicationDbContext context) : GenericRepository<ChatHistory>(context), ChatHistoryRepositoryPort
{
        public async Task<IEnumerable<ChatHistory>> GetUserChatHistoriesAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            return await _context.Set<ChatHistory>()
                .Where(ch => ch.UserId == userId)
                .OrderByDescending(ch => ch.LastMessageAt ?? ch.CreatedAt)
                .ToListAsync(cancellationToken);
        }

        public async Task<ChatHistory?> GetChatHistoryWithMessagesAsync(Guid chatHistoryId, CancellationToken cancellationToken = default)
        {
            return await _context.Set<ChatHistory>()
                .Include(ch => ch.Messages)
                .FirstOrDefaultAsync(ch => ch.Id == chatHistoryId, cancellationToken);
       }
    
}
