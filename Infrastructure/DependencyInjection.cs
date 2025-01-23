using Application.Common;
using Domain.Common;
using Infrastructure.Messaging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Anthropic.SDK;
using Infrastructure.Services;

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

        return services;
    }

   
}
