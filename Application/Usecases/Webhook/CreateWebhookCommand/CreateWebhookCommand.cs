using FluentResults;
using MediatR;

namespace Application.Usecases.Webhook.CreateWebhookCommand;

public sealed record CreateWebhookCommand : IRequest<Result<CreateWebhookResponse>>
{
    public string Name { get; init; }
    public string Url { get; init; }
    public string UserId { get; init; }
    public List<string> Events { get; init; }
} 