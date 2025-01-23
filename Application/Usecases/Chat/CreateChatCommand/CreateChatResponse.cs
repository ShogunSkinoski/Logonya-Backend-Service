namespace Application.Usecases.Chat.CreateChatCommand;

public sealed record CreateChatResponse(
    Guid ChatId,
    string Title,
    DateTime CreatedAt
); 