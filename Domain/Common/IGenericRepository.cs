using System.Linq.Expressions;

namespace Domain.Common;

public interface IGenericRepository<TEntity> where TEntity : class
{
    Task<TEntity> GetByIdAsync(object id, CancellationToken cancellation = default);
    Task<IEnumerable<TEntity>> GetAllByIdAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<TEntity>> GetAllAsync(
        Expression<Func<TEntity, bool>>? filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        string? includeProperties = null,
        CancellationToken cancellationToken = default);
    Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);
    Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);
    void Remove(TEntity entity);
    void Update(TEntity entity);
    void RemoveRange(IEnumerable<TEntity> entities);
    IQueryable<TEntity> Query();
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}