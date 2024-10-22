namespace Application.Usecases.Account.LoginAccountCommand;

public sealed record LoginAccountResponse(
        string UserId,
        string Token,
        string RefreshToken
    );
