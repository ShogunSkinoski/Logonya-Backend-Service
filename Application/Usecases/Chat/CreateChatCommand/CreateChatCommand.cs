using FluentResults;
using MediatR;

namespace Application.Usecases.Chat.CreateChatCommand;

public sealed record CreateChatCommand : IRequest<Result<CreateChatResponse>>
{
    public string UserId { get; init; }
    public string Title { get; init; }
} 