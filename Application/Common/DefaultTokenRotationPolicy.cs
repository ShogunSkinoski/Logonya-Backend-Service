using Domain.Account.Model;
using Domain.Common;


namespace Application.Common;

public class DefaultTokenRotationPolicy : ITokenRotationPolicy
{
    private readonly TimeSpan _tokenLifetime;

    public DefaultTokenRotationPolicy(TimeSpan tokenLifetime)
    {
        _tokenLifetime = tokenLifetime;
    }

    public bool ShouldRotateToken(RefreshToken currentToken)
    {
        return (DateTime.UtcNow - currentToken.CreatedAt) > (_tokenLifetime / 2);
    }
}
