using FluentResults;
using MediatR;

namespace Application.Usecases.Account.CreateAccountCommand;

public sealed record CreateAccountCommand : IRequest<Result<CreateAccountResponse>>
{
    public string Username { get; set; }
    public string Password { get; set; }
    public string Email { get; set; }
}
