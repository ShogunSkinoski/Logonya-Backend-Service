using Domain.Account.Port;
using Domain.Common;
using Infrastructure.Persistence.Repository;
using Infrastructure.Persistence.Repository.Account;

namespace Presentation.Extensions;

public static class RepositoryInjectionExtension
{
    public static IServiceCollection AddRepositoriesAndUnitOfWork(this IServiceCollection services)
    {
        services.AddScoped<UserRepositoryPort, EfUserRepository>();
        services.AddScoped<ApiKeyRepositoryPort, EfApiKeyRepository>();

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        return services;
    }
}
