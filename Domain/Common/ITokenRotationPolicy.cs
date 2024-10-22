using Domain.Account.Model;

namespace Domain.Common;

public interface ITokenRotationPolicy
{
    bool ShouldRotateToken(RefreshToken currentToken);
}
