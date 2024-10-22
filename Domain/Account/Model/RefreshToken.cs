namespace Domain.Account.Model;

public class RefreshToken
{
    public Guid Id { get; private set; }
    public string Token { get; private set; }
    public DateTime ExpiresAt { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? RevokedAt { get; private set; }
    public bool IsExpired => DateTime.UtcNow >= ExpiresAt;
    public bool IsRevoked => RevokedAt != null;
    public bool IsActive => !IsRevoked && !IsExpired;

    public RefreshToken(string token, DateTime expiresAt, DateTime createdAt, DateTime? revokedAt)
    {
        Token = token;
        ExpiresAt = expiresAt;
        CreatedAt = createdAt;
        RevokedAt = revokedAt;
    }

    public void InvalidateToken()
    {
        if (IsRevoked)
            return;

        if (IsExpired)
            return; 

        RevokedAt = DateTime.UtcNow;
    }
}