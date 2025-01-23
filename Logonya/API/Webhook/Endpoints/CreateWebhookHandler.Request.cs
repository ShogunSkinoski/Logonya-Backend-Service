namespace Presentation.API.Webhook;

public sealed record CreateWebhookRequest(
    string Name,
    string Url,
    List<string> Events
); 