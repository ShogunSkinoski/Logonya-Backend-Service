using System.IdentityModel.Tokens.Jwt;
using Application.Usecases.Chat.CreateChatCommand;
using MediatR;

namespace Presentation.API.Chat;

public static partial class ChatEndpoints
{
    private static async Task<IResult> CreateChatHandler(
        CreateChatRequest request,
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

        var command = new CreateChatCommand
        {
            UserId = userId,
            Title = request.Title
        };
        var result = await mediator.Send(command);
        var response = ApiResponse<CreateChatResponse>.FromResult(result);
        return Results.Ok(response);
    }
}