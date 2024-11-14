namespace Presentation.API.Account;

public static partial class AccountEndpoints
{
    public static RouteGroupBuilder MapAccountEndpoints(this RouteGroupBuilder builder)
    {
        builder.MapPost("account/register", AccountEndpoints.CreateAccountHandler).AllowAnonymous();
        builder.MapPost("account/login", AccountEndpoints.LoginAccountHandler).AllowAnonymous();
        builder.MapPost("account/{userId}/api-keys", AccountEndpoints.CreateApiHandler).RequireAuthorization();
        return builder;
    }
}
