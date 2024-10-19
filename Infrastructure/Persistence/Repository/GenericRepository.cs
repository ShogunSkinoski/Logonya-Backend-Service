using Domain.Common;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Persistence.Repository;

public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
{
    protected readonly DbContext _context;
    protected readonly DbSet<TEntity> _dbSet;
    public GenericRepository(DbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _dbSet = context.Set<TEntity>();
    }

    public async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        if (entity == null) throw new ArgumentNullException(nameof(entity));
        await _dbSet.AddAsync(entity, cancellationToken);
    }

    public async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _dbSet.Where(predicate).ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<TEntity>> GetAllByIdAsyn(CancellationToken cancellationToken = default)
    {
        return await _dbSet.ToListAsync(cancellationToken);
    }

    public async Task<TEntity> GetByIdAsyn(object id, CancellationToken cancellation = default)
    {
        return await _dbSet.FindAsync(new object[] {id}, cancellation);
    }

    public IQueryable<TEntity> Query()
    {
        return _dbSet.AsQueryable();
    }

    public void Remove(TEntity entity)
    {
        _dbSet.Remove(entity);
    }

    public void RemoveRange(IEnumerable<TEntity> entities)
    {
        _dbSet.RemoveRange(entities);
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public void Update(TEntity entity)
    {
        if (entity == null) throw new ArgumentNullException(nameof(entity));
        _dbSet.Update(entity);
    }
}
