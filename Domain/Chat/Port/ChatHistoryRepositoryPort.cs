using Domain.Chat.Model;
using Domain.Common;

namespace Domain.Chat.Port
{
    public interface ChatHistoryRepositoryPort : IGenericRepository<ChatHistory>
    {
        Task<IEnumerable<ChatHistory>> GetUserChatHistoriesAsync(Guid userId, CancellationToken cancellationToken = default);
        Task<ChatHistory?> GetChatHistoryWithMessagesAsync(Guid chatHistoryId, CancellationToken cancellationToken = default);
    }
} 