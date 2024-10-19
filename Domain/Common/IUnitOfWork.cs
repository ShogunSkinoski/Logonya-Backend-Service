namespace Domain.Common;

public interface IUnitOfWork : IDisposable
{
    public TRepository GetRepository<TRepository>() where TRepository : class;
    Task<int> CompleteAsync(CancellationToken cancellationToken = default);
}
