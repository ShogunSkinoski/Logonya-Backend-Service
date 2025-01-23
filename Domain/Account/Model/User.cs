using Domain.Chat.Model;
using Domain.Logging.Model;

namespace Domain.Account.Model;
public class User
{
    public Guid Id { get; private set; }
    public string Username { get; private set; }
    public string Email { get; private set; }
    public string PasswordHash { get; private set; }
    public List<ApiKey> ApiKeys { get; private set; } = new List<ApiKey>();
    public List<RefreshToken> RefreshTokens { get; private set; } = new List<RefreshToken>();
    public List<Log> Logs { get; private set; } = new List<Log>();
    public DateTime CreatedAt { get; private set; }
    public List<ChatHistory> ChatHistories { get; private set; } = new List<ChatHistory>();

    public User(string username, string email, string passwordHash)
    {
        Id = Guid.NewGuid();
        Username = username;
        Email = email;
        PasswordHash = passwordHash;
        CreatedAt = DateTime.UtcNow;
        ApiKeys = new List<ApiKey>();
        RefreshTokens = new List<RefreshToken>();
        Logs = new List<Log>();
        ChatHistories = new List<ChatHistory>();
    }
    public void CreateNewApiKey(string name, string? description)
    {
        var key = Guid.NewGuid().ToString();
        var apiKey = new ApiKey(name, key, description, Id);
        ApiKeys.Add(apiKey);
    }
    public void AddRefreshToken(RefreshToken refreshToken)
    {
        RefreshTokens.Add(refreshToken);
    }

    public void AddLog(Log log)
    {
        Logs.Add(log);
    }

    public void RemoveOldRefreshTokens(int daysToKeep)
    {
        RefreshTokens.RemoveAll(x => !x.IsActive && x.CreatedAt.AddDays(daysToKeep) <= DateTime.UtcNow);
    }

    public void AddChatHistory(ChatHistory chatHistory)
    {
        ChatHistories.Add(chatHistory);
    }
}