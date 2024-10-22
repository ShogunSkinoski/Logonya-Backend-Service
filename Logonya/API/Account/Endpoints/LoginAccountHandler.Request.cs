namespace Presentation.API.Account;

public sealed record LoginAccountHandlerRequest(
        string Email,
        string Password
    );
