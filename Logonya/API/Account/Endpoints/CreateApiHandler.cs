using Application.Usecases.Account.CreateApiCommand;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.API.Account;

public static partial class AccountEndpoints
{

    private static async Task<IResult> CreateApiHandler(CreateApiHandlerRequest request,[FromRoute] string userId, IMediator mediator)
    {
        var command = new CreateApiCommand(userId, request.ApiName, request.Description);
        var response = await mediator.Send(command);
        return Results.Ok(response);
    }
}
