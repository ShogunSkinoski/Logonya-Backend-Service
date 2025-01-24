using Application.Usecases.Logging.CreateAlertCommand;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.API.Logging;

public static partial class LoggingEndpoints
{
    public static async Task<IResult> CreateAlertHandler(
        [FromBody] CreateAlertRequest request,
        HttpContext context,
        IMediator mediator,
        CancellationToken cancellationToken)
    {
        var identity = context.User.Identity as Microsoft.IdentityModel.Tokens.CaseSensitiveClaimsIdentity;
        var jwt = identity?.SecurityToken as Microsoft.IdentityModel.JsonWebTokens.JsonWebToken;
        var userId = jwt?.Subject;

        if (string.IsNullOrEmpty(userId))
        {
            return Results.Unauthorized();
        }

        var command = new CreateAlertCommand
        {
            Name = request.Name,
            Description = request.Description,
            Condition = request.Condition,
            Channel = request.Channel,
            Target = request.Target,
            UserId = userId
        };

        var result = await mediator.Send(command, cancellationToken);

        if (result.IsFailed)
        {
            return Results.BadRequest(ApiResponse<CreateAlertResponse>.FromResult(result));
        }

        return Results.Ok(ApiResponse<CreateAlertResponse>.FromResult(result));
    }
} 