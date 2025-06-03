using Common.Infrastructure.ServiceExtensions;
using Domain.Interfaces;
using Infrastructure;
using Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Presentation;

public static class UserModule
{
    public static void SetupUserModule(this IServiceCollection services, IConfiguration configuration)
    {
        
    }
    
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        
        var databaseName = configuration["DatabaseName"];
        services.AddSingleton<IUnitOfWork, UnitOfWork>();

        if (databaseName != null) services.AddMongoService<UserDbContext>(databaseName);
    }
}