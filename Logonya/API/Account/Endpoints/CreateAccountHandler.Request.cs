namespace Presentation.API.Account;

public sealed record CreateAccountRequest(
    string Username ,
    string Password ,
    string Email
);
