using Application.Usecases.Account.CreateApiCommand;
using MediatR;

namespace Presentation.API.Account;

public static partial class AccountEndpoints
{

    private static async Task<IResult> CreateApiHandler(CreateApiHandlerRequest request, IMediator mediator)
    {
        var command = new CreateApiCommand(request.UserId, request.ApiName, request.Description);
        var response = await mediator.Send(command);
        return Results.Ok(response);
    }
}
