using Application.Usecases.Account.CreateAccountCommand;
using MediatR;

namespace Presentation.API.Account;


public static partial class AccountEndpoints
{
    private static async Task<IResult> CreateAccountHandler(
        CreateAccountRequest request,
        IMediator mediator)
    {
        var command = new CreateAccountCommand
        {
            Username = request.Username,
            Password = request.Password,
            Email = request.Email
        };
        var result = await mediator.Send(command);
        var response = ApiResponse<CreateAccountResponse>.FromResult(result);
        return Results.Ok(response);
    }
}
