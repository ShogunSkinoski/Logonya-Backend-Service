namespace Application.Usecases.Chat.SendMessageCommand;

public sealed record SendMessageResponse(
    Guid MessageId,
    string Content,
    string Role,
    DateTime CreatedAt
); 