using Application.Usecases.Logging.ReplayLogCommand;
using MediatR;
using Microsoft.AspNetCore.Mvc;
namespace Presentation.API.Logging;

public static partial class LoggingEndpoints
{
    public static async Task<IResult> ReplayLogHandler(
        [FromQuery] string requestId,
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

        var command = new ReplayLogCommand
        {
            RequestId = requestId,
            UserId = Guid.Parse(userId) 
        };

        var result = await mediator.Send(command, cancellationToken);

        if (result.IsFailed)
        {
            return Results.BadRequest(ApiResponse<ReplayLogResponse>.FromResult(result));
        }

        return Results.Ok(ApiResponse<ReplayLogResponse>.FromResult(result));
    }
}
