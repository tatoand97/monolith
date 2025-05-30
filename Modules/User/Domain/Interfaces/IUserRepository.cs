using Common.Domain.Interfaces;
using Domain.Entities;

namespace Domain.Interfaces;

public interface IUserRepository : IGenericRepository<User>
{
    Task<User?> GetAsync(Guid id, CancellationToken cancellationToken = default);
    Task InsertAsync(User user, CancellationToken cancellationToken = default);
}