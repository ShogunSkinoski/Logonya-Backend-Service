using FluentResults;
using MediatR;

namespace Application.Usecases.Chat.SendMessageCommand;

public sealed record SendMessageCommand : IRequest<Result<SendMessageResponse>>
{
    public string ChatHistoryId { get; init; }
    public string Content { get; init; }
    public string Role { get; init; }
    public double? TokenCount { get; init; }
} 