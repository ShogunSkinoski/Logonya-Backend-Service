namespace Presentation.API.Account;

public sealed record CreateApiHandlerRequest(
        string ApiName,
        string? Description
    );
