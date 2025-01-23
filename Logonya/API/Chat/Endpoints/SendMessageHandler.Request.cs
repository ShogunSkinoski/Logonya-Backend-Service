namespace Presentation.API.Chat;

public sealed record SendMessageRequest(
    string Content,
    string Role = "user"
); 