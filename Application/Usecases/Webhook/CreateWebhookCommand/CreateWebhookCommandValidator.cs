using Application.Common;
using FluentValidation;

namespace Application.Usecases.Webhook.CreateWebhookCommand;

public class CreateWebhookCommandValidator : BaseValidator<CreateWebhookCommand>
{
    public CreateWebhookCommandValidator()
    {
        ValidateRequired(nameof(CreateWebhookCommand.Name));
        ValidateRequired(nameof(CreateWebhookCommand.Url));
        ValidateRequired(nameof(CreateWebhookCommand.UserId));
        ValidateRequired(nameof(CreateWebhookCommand.Events));

        RuleFor(x => x.Name)
            .MaximumLength(100)
            .WithMessage("Name must not exceed 100 characters")
            .WithErrorCode("NAME_TOO_LONG");

        RuleFor(x => x.Url)
            .MaximumLength(500)
            .WithMessage("URL must not exceed 500 characters")
            .WithErrorCode("URL_TOO_LONG")
            .Must(BeValidUrl)
            .WithMessage("Invalid URL format")
            .WithErrorCode("INVALID_URL");

        RuleFor(x => x.Events)
            .Must(events => events.All(e => IsValidEventType(e)))
            .WithMessage("Contains invalid event types")
            .WithErrorCode("INVALID_EVENT_TYPES");
    }

    private bool BeValidUrl(string url)
    {
        return Uri.TryCreate(url, UriKind.Absolute, out var uriResult)
            && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
    }

    private bool IsValidEventType(string eventType)
    {

        var validEvents = new[]
        {
            WebhookEvents.LOG_CREATED,
            WebhookEvents.ERROR_DETECTED,
            WebhookEvents.RATE_LIMIT_EXCEEDED,
            WebhookEvents.API_KEY_EXPIRED,
            WebhookEvents.ALERT_TRIGGERED,
            WebhookEvents.ALERT_RESOLVED

        };

        return validEvents.Contains(eventType);
    }
    public static class WebhookEvents
    {
        public const string LOG_CREATED = "log.created";
        public const string ERROR_DETECTED = "error.detected";
        public const string RATE_LIMIT_EXCEEDED = "rate_limit.exceeded";
        public const string API_KEY_EXPIRED = "api_key.expired";
        public const string ALERT_TRIGGERED = "alert.triggered";
        public const string ALERT_RESOLVED = "alert.resolved";
    }
} 