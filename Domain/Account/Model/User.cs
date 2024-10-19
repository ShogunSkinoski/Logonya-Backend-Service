namespace Domain.Account.Model { 
    public class User
    {
        public Guid Id { get; private set; }
        public string UserName { get; private set; }
        public string Email { get; private set; }
        public string PasswordHash { get; private set; }
        public IEnumerable<ApiKey>? ApiKeys { get; private set; }
        public DateTime CreatedAt { get; private set; }

        public User(string userName, string email, string passwordHash)
        {
            UserName = userName;
            Email = email;
            PasswordHash = passwordHash;
            CreatedAt = DateTime.Now.ToUniversalTime();
        }
    }
}