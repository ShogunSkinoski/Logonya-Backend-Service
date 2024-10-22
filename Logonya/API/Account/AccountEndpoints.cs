namespace Presentation.API.Account;

public static partial class AccountEndpoints
{
    public static RouteGroupBuilder MapAccountEndpoints(this RouteGroupBuilder builder)
    {
        builder.MapPost("account/register", AccountEndpoints.CreateAccountHandler);
        builder.MapPost("account/login", AccountEndpoints.LoginAccountHandler);
        builder.MapPost("account/api-keys", AccountEndpoints.CreateApiHandler);
        return builder;
    }
}
