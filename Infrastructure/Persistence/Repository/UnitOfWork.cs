using Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;


namespace Infrastructure.Persistence.Repository;

public class UnitOfWork : IUnitOfWork
{
    private readonly DbContext _dbContext;
    private readonly IServiceProvider _serviceProvider;
    public UnitOfWork(DbContext dbContext, IServiceProvider serviceProvider)
    {
        _dbContext = dbContext;
        _serviceProvider = serviceProvider;
    }
    public async Task<int> CompleteAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public void Dispose()
    {
        _dbContext.Dispose();
    }

    public TRepository GetRepository<TRepository>() where TRepository : class
    {
       return _serviceProvider.GetRequiredService<TRepository>();
    }
}
