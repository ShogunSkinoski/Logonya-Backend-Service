using Application.Usecases.Account.LoginAccountCommand;
using MediatR;

namespace Presentation.API.Account;

public static partial class AccountEndpoints { 
    public static async Task<IResult> LoginAccountHandler(LoginAccountHandlerRequest request, IMediator mediator) { 
        var command = new LoginAccountCommand(request.Email, request.Password);
        var response = await mediator.Send(command);
        return Results.Ok(response.Value);
    }
}
