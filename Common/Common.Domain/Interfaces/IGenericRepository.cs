using System.Linq.Expressions;
using Common.Domain.Pagination;

namespace Common.Domain.Interfaces;

public interface IGenericRepository<TEntity> where TEntity : class
{
    Task<TEntity?>  GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<IReadOnlyList<TEntity>> ListAsync(
        Expression<Func<TEntity, bool>>? predicate = null,
        CancellationToken ct = default,
        params Expression<Func<TEntity, object>>[] includes);

    Task<PagedList<TEntity>> ListPagedAsync(
        PaginationParams paginationParams,
        Expression<Func<TEntity, bool>>? predicate = null,
        CancellationToken ct = default,
        params Expression<Func<TEntity, object>>[] includes);

    Task AddAsync   (TEntity entity, CancellationToken ct = default);
    void Update     (TEntity entity);
    void Remove     (TEntity entity);
}