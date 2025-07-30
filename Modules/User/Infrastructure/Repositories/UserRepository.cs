using Common.Infrastructure.Repositories;
using User.Domain.Entities;
using User.Domain.Interfaces;

namespace User.Infrastructure.Repositories;

public class UserRepository(UserDbContext dbContext) : GenericRepository<UserEntity,UserDbContext>(dbContext),IUserRepository
{
    private readonly UserDbContext _dbContext = dbContext;

    public async Task<UserEntity?> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Users.FindAsync([id], cancellationToken: cancellationToken);
    }

    public async Task InsertAsync(UserEntity user, CancellationToken cancellationToken = default)
    {
        await _dbContext.Users.AddAsync(user, cancellationToken);
    }
}