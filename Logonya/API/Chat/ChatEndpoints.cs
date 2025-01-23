 namespace Presentation.API.Chat;

public static partial class ChatEndpoints
{
    public static RouteGroupBuilder MapChatEndpoints(this RouteGroupBuilder builder)
    {
        builder.MapPost("chat", ChatEndpoints.CreateChatHandler).RequireAuthorization();
        builder.MapPost("chat/{chatId}/messages", ChatEndpoints.SendMessageHandler).RequireAuthorization();
        return builder;
    }
}