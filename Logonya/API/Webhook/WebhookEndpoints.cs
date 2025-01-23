namespace Presentation.API.Webhook;

public static partial class WebhookEndpoints
{
    public static RouteGroupBuilder MapWebhookEndpoints(this RouteGroupBuilder builder)
    {
        builder.MapPost("webhooks", WebhookEndpoints.CreateWebhookHandler)
            .RequireAuthorization();
        return builder;
    }
} 