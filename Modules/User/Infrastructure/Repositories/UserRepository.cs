using Common.Infrastructure.Repositories;
using Domain.Entities;
using Domain.Interfaces;

namespace Infrastructure.Repositories;

public class UserRepository(UserDbContext dbContext) : GenericRepository<User,UserDbContext>(dbContext),IUserRepository
{
    private readonly UserDbContext _dbContext = dbContext;

    public async Task<User?> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Users.FindAsync([id, cancellationToken], cancellationToken: cancellationToken);
    }

    public async Task InsertAsync(User user, CancellationToken cancellationToken = default)
    {
        await _dbContext.Users.AddAsync(user, cancellationToken);
    }
}