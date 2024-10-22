using FluentResults;
using MediatR;

namespace Application.Usecases.Account.LoginAccountCommand;

public class LoginAccountCommand(
        string email,
        string password
    ) : IRequest<Result<LoginAccountResponse>>
{
    public string Email { get; set; } = email;
    public string Password { get; set; } = password;
}

