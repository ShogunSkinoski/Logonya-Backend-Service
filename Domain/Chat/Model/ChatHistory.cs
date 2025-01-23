
using Domain.Account.Model;

namespace Domain.Chat.Model
{
    public class ChatHistory
    {
        public Guid Id { get; private set; }
        public string Title { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? LastMessageAt { get; private set; }
        
        // Navigation properties
        public Guid UserId { get; private set; }
        public User User { get; private set; }
        public List<ChatMessage> Messages { get; private set; } = new List<ChatMessage>();

        public ChatHistory(string title, Guid userId)
        {
            Id = Guid.NewGuid();
            Title = title;
            UserId = userId;
            CreatedAt = DateTime.UtcNow;
            Messages = new List<ChatMessage>();
        }

        public void AddMessage(ChatMessage message)
        {
            Messages.Add(message);
            LastMessageAt = DateTime.UtcNow;
        }
    }
}
