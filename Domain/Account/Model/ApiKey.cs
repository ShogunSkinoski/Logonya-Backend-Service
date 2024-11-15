using Domain.Logging.Model;

namespace Domain.Account.Model { 
    public class ApiKey
    {
        public Guid Id { get; private set; }
        public string Key { get; private set; }
        public string Name { get; private set; }
        public string? Description { get; private set; }
        public DateTime CreatedAt { get; private set; }

        // Navigation properties
        public Guid UserId { get; private set; }
        public User User { get; private set; }
        public List<Log> Logs { get; private set; } = new List<Log>();

        public ApiKey(string name, string key, string? description, Guid userId)
        {
            Id = Guid.NewGuid();
            Name = name;
            Key = key;
            Description = description;
            UserId = userId;
            CreatedAt = DateTime.UtcNow;
            Logs = new List<Log>();
        }
    }
}