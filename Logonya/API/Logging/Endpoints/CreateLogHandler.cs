using Application.Usecases.Logging.CreateLogCommand;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.API.Logging;

public static partial class LoggingEndpoints
{
    private static async Task<IResult> CreateLogHandler(
        CreateLogRequest request,
        HttpContext context,
        [FromHeader(Name = "X-API-KEY")] string apiKey,
        IMediator mediator,
        CancellationToken cancellationToken)
    {
        var command = new CreateLogCommand
        {
            Action = request.Action,
            Description = request.Description,
            Level = request.Level,
            Source = request.Source,
            Environment = request.Environment,
            IpAddress = context.Connection.RemoteIpAddress?.ToString() ?? "unknown",
            ApiKeyId = apiKey,
            Metadata = request.Metadata,
            UserName = request.UserName,
            Exception = request.Exception,
            StackTrace = request.StackTrace
        };

        var result = await mediator.Send(command, cancellationToken);

        if (result.IsFailed)
        {
            return Results.BadRequest(ApiResponse<CreateLogResponse>.FromResult(result));
        }

        return Results.Ok(ApiResponse<CreateLogResponse>.FromResult(result));
    }
}
