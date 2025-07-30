using Common.Infrastructure.ServiceExtensions;
using FluentValidation;
using User.Domain.Interfaces;
using User.Infrastructure;
using User.Infrastructure.Repositories;

namespace User.Presentation;

/// <summary>
/// Provides an extension method to configure the UserEntity module within the service collection of an application.
/// </summary>
public static class UserModule
{
    /// <summary>
    /// Configures the UserEntity module by registering application and infrastructure services into the service collection.
    /// </summary>
    /// <param name="services">The service collection to which the module's services will be added.</param>
    /// <param name="configuration">The application configuration used to configure module dependencies.</param>
    public static void SetupUserModule(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddApplicationModule(configuration);
        //services.AddInfrastructure(configuration);
    }

    private static void AddApplicationModule(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddValidatorsFromAssembly(Application.AssemblyReference.Assembly);
    }
    
    private static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {

        var databaseName = configuration["DatabaseName"];
        if (databaseName != null) services.AddMongoService<UserDbContext>(databaseName);
        services.AddSingleton<IUnitOfWork, UnitOfWork>();
    }
}