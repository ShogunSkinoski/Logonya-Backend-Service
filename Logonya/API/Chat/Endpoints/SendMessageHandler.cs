using Application.Usecases.Chat.SendMessageCommand;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.API.Chat;

public static partial class ChatEndpoints
{
    private static async Task<IResult> SendMessageHandler(
        SendMessageRequest request,
        [FromRoute] string chatId,
        IMediator mediator)
    {
        var command = new SendMessageCommand
        {
            ChatHistoryId = chatId,
            Content = request.Content,
            Role = request.Role
        };
        var result = await mediator.Send(command);
        var response = ApiResponse<SendMessageResponse>.FromResult(result);
        return Results.Ok(response);
    }
} 