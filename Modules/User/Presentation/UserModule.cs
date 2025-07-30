using Common.Infrastructure.ServiceExtensions;
using Domain.Interfaces;
using FluentValidation;
using Infrastructure;
using Infrastructure.Repositories;

namespace User.Presentation;

/// <summary>
/// Provides an extension method to configure the User module within the service collection of an application.
/// </summary>
public static class UserModule
{
    /// <summary>
    /// Configures the User module by registering application and infrastructure services into the service collection.
    /// </summary>
    /// <param name="services">The service collection to which the module's services will be added.</param>
    /// <param name="configuration">The application configuration used to configure module dependencies.</param>
    public static void SetupUserModule(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddApplicationModule(configuration);
        services.AddInfrastructure(configuration);
    }

    private static void AddApplicationModule(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddValidatorsFromAssembly(AssemblyReference.Assembly);
    }
    
    private static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {

        var databaseName = configuration["DatabaseName"];
        if (databaseName != null) services.AddMongoService<UserDbContext>(databaseName);
        services.AddSingleton<IUnitOfWork, UnitOfWork>();
    }
}