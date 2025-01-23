using Domain.Account.Model;
using Domain.Common;

namespace Domain.Account.Port;

public interface ApiKeyRepositoryPort : IGenericRepository<ApiKey>
{
    public Task<ApiKey?> FindApiKey(string apiKey, CancellationToken cancellationToken);
}
