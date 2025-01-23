using Domain.Chat.Model;
using Domain.Common;

namespace Domain.Chat.Port;

public interface ChatMessageRepositoryPort : IGenericRepository<ChatMessage>
{
    Task<IEnumerable<ChatMessage>> GetChatMessagesAsync(Guid chatHistoryId, CancellationToken cancellationToken = default);
} 