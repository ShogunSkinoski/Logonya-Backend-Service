using Domain.Account.Model;

namespace Application.Common;

public interface IJWTGenerator
{
    public string GenerateToken(User entity);
    public string GenerateRefreshToken();
    bool ValidateToken(string token);
}
