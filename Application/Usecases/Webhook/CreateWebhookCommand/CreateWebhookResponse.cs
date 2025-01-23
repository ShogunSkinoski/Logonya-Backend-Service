namespace Application.Usecases.Webhook.CreateWebhookCommand;

public sealed record CreateWebhookResponse(
    Guid Id,
    string Name,
    string Url,
    string Secret,
    List<string> Events,
    DateTime CreatedAt
); 