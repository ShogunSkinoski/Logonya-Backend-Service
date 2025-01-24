using Domain.Account.Model;

namespace Domain.Logging.Model
{
    public class Alert
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public string Condition { get; private set; } 
        public string Channel { get; private set; }   
        public string Target { get; private set; }     
        public bool IsActive { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? LastTriggeredAt { get; private set; }
        
        // Navigation properties
        public Guid UserId { get; private set; }
        public User User { get; private set; }

        public Alert(
            string name,
            string description,
            string condition,
            string channel,
            string target,
            Guid userId)
        {
            Id = Guid.NewGuid();
            Name = name;
            Description = description;
            Condition = condition;
            Channel = channel;
            Target = target;
            UserId = userId;
            IsActive = true;
            CreatedAt = DateTime.UtcNow;
        }

        public void Trigger()
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
} 