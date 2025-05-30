﻿using Domain.Account.Port;
using Domain.Chat.Port;
using Domain.Common;
using Domain.Logging.Port;
using Infrastructure.Persistence.Repository;
using Infrastructure.Persistence.Repository.Account;
using Infrastructure.Persistence.Repository.Chat;
using Infrastructure.Persistence.Repository.Logging;

namespace Presentation.Extensions;

public static class RepositoryInjectionExtension
{
    public static IServiceCollection AddRepositoriesAndUnitOfWork(this IServiceCollection services)
    {
        services.AddScoped<UserRepositoryPort, EfUserRepository>();
        services.AddScoped<ApiKeyRepositoryPort, EfApiKeyRepository>();
        services.AddScoped<LogRepositoryPort, EfLogRepository>();
        services.AddScoped<ChatHistoryRepositoryPort, EfChatHistoryRepository>();
        services.AddScoped<ChatMessageRepositoryPort, EfChatMessageRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        return services;
    }
}
