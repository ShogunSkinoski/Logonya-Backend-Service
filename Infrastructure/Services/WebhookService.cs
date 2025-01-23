using System.Security.Cryptography;
using System.Text;
using Application.Common;
using Domain.Common;
using Domain.Logging.Port;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Infrastructure.Services;

public class WebhookService : IWebhookService
{
    private readonly ILogger<WebhookService> _logger;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IMessagingProducer _messagingProducer;
    private readonly WebhookRepositoryPort _webhookRepository;
    private readonly IKafkaSettings _kafkaSettings;

    public WebhookService(
        ILogger<WebhookService> logger,
        IHttpClientFactory httpClientFactory,
        IMessagingProducer messagingProducer,
        WebhookRepositoryPort webhookRepository,
        IKafkaSettings kafkaSettings)
    {
        _logger = logger;
        _httpClientFactory = httpClientFactory;
        _messagingProducer = messagingProducer;
        _webhookRepository = webhookRepository;
        _kafkaSettings = kafkaSettings;
    }

    public async Task SendWebhookAsync(string eventType, object payload, Guid userId, CancellationToken cancellationToken = default)
    {
        var webhooks = await _webhookRepository.GetActiveWebhooksByEventAsync(eventType, cancellationToken);
        
        foreach (var webhook in webhooks)
        {
            try
            {
                if (webhook.UserId != userId)
                    return;
                var message = new WebhookMessage
                {
                    WebhookId = webhook.Id,
                    EventType = eventType,
                    Payload = payload,
                    Timestamp = DateTime.UtcNow
                };

                _messagingProducer.SendMessage(
                    _kafkaSettings.Topics[InfraConsts.KafkaTopicKeys.WEBHOOK_NOTIFICATIONS].Name,
                    webhook.Id.ToString(),
                    message
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error queueing webhook {WebhookId} for event {EventType}", 
                    webhook.Id, eventType);
            }
        }
    }

    public async Task ProcessWebhookQueueAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var message = await _messagingProducer.ConsumeMessage<WebhookMessage>(
                _kafkaSettings.Topics[InfraConsts.KafkaTopicKeys.WEBHOOK_NOTIFICATIONS].Name,
                cancellationToken);

            if (message == null) return;

            var webhook = await _webhookRepository.GetByIdAsync(message.WebhookId, cancellationToken);
            if (webhook == null)
            {
                _logger.LogWarning("Webhook {WebhookId} not found", message.WebhookId);
                return;
            }

            var httpClient = _httpClientFactory.CreateClient("webhook");
            var payload = JsonConvert.SerializeObject(message.Payload);
            var signature = GenerateSignature(payload, webhook.Secret);

            var request = new HttpRequestMessage(HttpMethod.Post, webhook.Url)
            {
                Content = new StringContent(payload, Encoding.UTF8, "application/json")
            };
            
            request.Headers.Add("X-Webhook-Signature", signature);
            request.Headers.Add("X-Webhook-Event", message.EventType);

            var response = await httpClient.SendAsync(request, cancellationToken);
            response.EnsureSuccessStatusCode();

            webhook.UpdateLastTriggered();
            await _webhookRepository.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing webhook queue");
        }
    }

    private string GenerateSignature(string payload, string secret)
    {
        using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secret));
        var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(payload));
        return Convert.ToBase64String(hash);
    }
}

public class WebhookMessage
{
    public Guid WebhookId { get; set; }
    public string EventType { get; set; }
    public object Payload { get; set; }
    public DateTime Timestamp { get; set; }
} 