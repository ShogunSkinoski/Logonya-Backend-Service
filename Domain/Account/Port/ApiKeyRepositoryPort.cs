using Domain.Account.Model;

namespace Domain.Account.Port;

public interface ApiKeyRepositoryPort
{
    public Task<ApiKey?> FindApiKey(string apiKey, CancellationToken cancellationToken);
}
