using Domain.Account.Model;

namespace Domain.Logging.Model;

public class Webhook
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string Url { get; private set; }
    public string Secret { get; private set; }
    public bool IsActive { get; private set; }
    public List<string> Events { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? LastTriggeredAt { get; private set; }
    
    // Navigation properties
    public Guid UserId { get; private set; }
    public User User { get; private set; }

    public Webhook(string name, string url, string secret, Guid userId, List<string> events)
    {
        Id = Guid.NewGuid();
        Name = name;
        Url = url;
        Secret = secret;
        UserId = userId;
        Events = events;
        IsActive = true;
        CreatedAt = DateTime.UtcNow;
    }

    public void UpdateLastTriggered()
    {
        LastTriggeredAt = DateTime.UtcNow;
    }

    public void Deactivate()
    {
        IsActive = false;
    }

    public void Activate()
    {
        IsActive = true;
    }
} 