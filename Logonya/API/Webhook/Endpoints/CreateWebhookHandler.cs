using Application.Usecases.Webhook.CreateWebhookCommand;
using MediatR;
using System.Security.Claims;

namespace Presentation.API.Webhook;

public static partial class WebhookEndpoints
{
    private static async Task<IResult> CreateWebhookHandler(
        CreateWebhookRequest request,
        HttpContext context,
        IMediator mediator)
    {
        var identity = context.User.Identity as Microsoft.IdentityModel.Tokens.CaseSensitiveClaimsIdentity;
        var jwt = identity?.SecurityToken as Microsoft.IdentityModel.JsonWebTokens.JsonWebToken;
        var userId = jwt?.Subject;

        if (string.IsNullOrEmpty(userId))
        {
            return Results.Unauthorized();
        }
        var command = new CreateWebhookCommand
        {
            Name = request.Name,
            Url = request.Url,
            UserId = userId,
            Events = request.Events
        };

        var result = await mediator.Send(command);
        var response = ApiResponse<CreateWebhookResponse>.FromResult(result);
        return Results.Ok(response);
    }
} 