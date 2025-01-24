using Application.Common;
using Domain.Common;
using Domain.Logging.Port;
using Infrastructure.Messaging;
using Infrastructure.Persistence.Repository.Logging;
using Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Anthropic.SDK;
using Infrastructure.Services.BackgroundServices;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddKafkaServices(
        this IServiceCollection services,
        IConfiguration configuration,
        string sectionName = "KafkaProducerConfig")
    {
        // Bind and validate configuration
        services.AddOptions<KafkaSettings>()
            .Bind(configuration.GetSection(sectionName))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        // Register settings
        services.AddSingleton<IKafkaSettings>(sp =>
            sp.GetRequiredService<IOptions<KafkaSettings>>().Value);

        // Register Kafka producer
        services.AddSingleton<IMessagingProducer, KafkaProducer>();

        // Register Kafka admin client for topic management
        services.AddSingleton<KafkaTopicConfig>();

        // Register hosted service for topic initialization
        services.AddHostedService<KafkaTopicInitializer>();

        

        services.AddSingleton<AnthropicClient>(_ => new AnthropicClient(configuration["Anthropic:ApiKey"]));
        services.AddHttpClient<IRAGService, RAGService>();
        // Register webhook related services
        services.AddScoped<WebhookRepositoryPort, EfWebhookRepository>();
        
        services.AddScoped<IWebhookService, WebhookService>();
        services.AddHostedService<WebhookProcessingService>();

        services.AddScoped<AlertRepositoryPort, EfAlertRepository>();
        services.AddHostedService<AlertMonitoringService>();
        return services;
    }
}
