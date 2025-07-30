using System.Linq.Expressions;
using Common.Domain.Interfaces;
using Common.Domain.Pagination;
using Microsoft.EntityFrameworkCore;

namespace Common.Infrastructure.Repositories;

public class GenericRepository<TEntity, TContext>(TContext context) : IGenericRepository<TEntity>
    where TEntity  : class
    where TContext : DbContext
{
    private readonly DbSet<TEntity> _set = context.Set<TEntity>();

    public async Task<TEntity?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
        await _set.FindAsync([id], ct);

    public async Task<IReadOnlyList<TEntity>> ListAsync(
        Expression<Func<TEntity, bool>>? predicate = null,
        CancellationToken cancellationToken = default,
        params Expression<Func<TEntity, object>>[] includes)
    {
        var query = _set.AsQueryable();

        if (predicate is not null) query = query.Where(predicate);
        if (includes.Length > 0)   query = includes.Aggregate(query, (currentQuery, includeExpression) => currentQuery.Include(includeExpression));

        return await query.ToListAsync(cancellationToken);
    }

    public async Task<PagedList<TEntity>> ListPagedAsync(
        PaginationParams paginationParams,
        Expression<Func<TEntity, bool>>? predicate = null,
        CancellationToken cancellationToken = default,
        params Expression<Func<TEntity, object>>[] includes)
    {
        var query = _set.AsQueryable();

        if (predicate is not null) query = query.Where(predicate);
        if (includes.Length > 0)   query = includes.Aggregate(query, (currentQuery, includeExpression) => currentQuery.Include(includeExpression));

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize)
            .Take(paginationParams.PageSize)
            .ToListAsync(cancellationToken);

        return new PagedList<TEntity>(items, totalCount, paginationParams.PageNumber, paginationParams.PageSize);
    }

    public Task AddAsync(TEntity entity, CancellationToken cancellationToken = default) =>
        _set.AddAsync(entity, cancellationToken).AsTask();

    public void Update(TEntity entity) => _set.Update(entity);

    public void Remove(TEntity entity) => _set.Remove(entity);
}
