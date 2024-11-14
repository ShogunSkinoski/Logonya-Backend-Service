using Domain.Account.Model;
using Domain.Account.Port;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repository.Account;

public class EfApiKeyRepository(ApplicationDbContext dbContext) : GenericRepository<ApiKey>(dbContext), ApiKeyRepositoryPort
{
    public async Task<ApiKey?> FindApiKey(string apiKey, CancellationToken cancellationToken)
    {
        var ApiKey = await _context.Set<ApiKey>().Where(x=> x.Key == apiKey).FirstOrDefaultAsync(cancellationToken);
        return ApiKey;
    }
}
