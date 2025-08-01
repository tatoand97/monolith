﻿using User.Domain.Interfaces;

namespace User.Infrastructure.Repositories;

public sealed class UnitOfWork(UserDbContext dbContext) : IUnitOfWork
{
    private UserRepository? _userRepository;
    private bool _disposed;

    public IUserRepository Users => _userRepository ??= new UserRepository(dbContext);

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await dbContext.SaveChangesAsync(cancellationToken);
    }

    private void Dispose(bool disposing)
    {
        if (!_disposed && disposing)
        {
            dbContext.Dispose();
        }
        _disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
    }
}