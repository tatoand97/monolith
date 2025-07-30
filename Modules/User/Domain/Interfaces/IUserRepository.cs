using Common.Domain.Interfaces;
using User.Domain.Entities;

namespace User.Domain.Interfaces;

public interface IUserRepository : IGenericRepository<UserEntity>
{
    Task<UserEntity?> GetAsync(Guid id, CancellationToken cancellationToken = default);
    Task InsertAsync(UserEntity userEntity, CancellationToken cancellationToken = default);
}