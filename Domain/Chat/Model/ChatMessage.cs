using System;
using Domain.Account.Model;

namespace Domain.Chat.Model
{
    public class ChatMessage
    {
        public Guid Id { get; private set; }
        public string Content { get; private set; }
        public string Role { get; private set; }  // system, user, assistant
        public DateTime CreatedAt { get; private set; }
        public double? TokenCount { get; private set; }
        
        // Navigation properties
        public Guid ChatHistoryId { get; private set; }
        public ChatHistory ChatHistory { get; private set; }

        public ChatMessage(string content, string role, Guid chatHistoryId, double? tokenCount = null)
        {
            Id = Guid.NewGuid();
            Content = content;
            Role = role;
            ChatHistoryId = chatHistoryId;
            TokenCount = tokenCount;
            CreatedAt = DateTime.UtcNow;
        }
    }
} 