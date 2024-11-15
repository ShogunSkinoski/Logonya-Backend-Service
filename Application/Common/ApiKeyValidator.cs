
using Domain.Account.Model;
using Domain.Account.Port;
using Domain.Common;
using Microsoft.Extensions.Caching.Memory;

namespace Application.Common;

public class ApiKeyValidator(
        IUnitOfWork uow,
        IMemoryCache memoryCache
    ) : IApiKeyValidator
{
    private readonly IUnitOfWork _uow = uow;
    private readonly IMemoryCache _memoryCache = memoryCache;
    private readonly TimeSpan CACHE_DURATION = TimeSpan.FromMinutes(5);
    public async Task<bool> IsValidApiKey(string apiKey, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(apiKey))
            return false;
        if (_memoryCache.TryGetValue<bool>(apiKey, out var _))
            return true;

        try
        {
            

            var apiKeyEntity = await _uow.GetRepository<ApiKeyRepositoryPort>().FindApiKey(apiKey, cancellationToken);
            if (apiKeyEntity == null)
                return false;

            //TODO: Add is active or not check for api key entity
            _memoryCache.Set(apiKey, true, CACHE_DURATION);

            return true;
        }
        catch (Exception ex)
        {
            return false;
        }
    }
}
