using System.Linq.Expressions;

namespace Application.Common;

public interface IGenericRepository<TEntity> where TEntity : class
{
    Task<TEntity> GetByIdAsyn(object id, CancellationToken cancellation = default);
    Task<IEnumerable<TEntity>> GetAllByIdAsyn(CancellationToken cancellationToken = default);
    Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);
    Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);
    void Update(TEntity entity);
    void Remove(TEntity entity);
    void RemoveRange(IEnumerable<TEntity> entities);
    IQueryable<TEntity> Query();
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}