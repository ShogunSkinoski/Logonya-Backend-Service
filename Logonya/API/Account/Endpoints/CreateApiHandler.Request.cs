namespace Presentation.API.Account;

public sealed record CreateApiHandlerRequest(
        string UserId,
        string ApiName,
        string? Description
    );
