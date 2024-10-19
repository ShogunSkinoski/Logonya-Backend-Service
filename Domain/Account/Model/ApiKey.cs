namespace Domain.Account.Model { 
    public class ApiKey
    {
        public Guid Id { get; private set; }
        public string Key { get; private set; }
        public string Name { get; private set; }
        public string? Description { get; private set; }
        public DateTime CreatedAt { get; private set; }

        public ApiKey(string name,string key, string? description)
        {
            Name = name;
            Key = key;
            Description = description;
            CreatedAt = DateTime.Now.ToUniversalTime();
        }
    }
}