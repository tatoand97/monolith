using User.Presentation;

namespace NameProject.Server.Configs;

/// <summary>
/// Provides a configuration class for setting up modules in the service collection of an application.
/// </summary>
public static class ModulesConfig
{
    /// <summary>
    /// Adds and configures application modules by extending the service collection.
    /// </summary>
    /// <param name="services">The service collection to add the modules to.</param>
    /// <param name="configuration">The configuration object used for module setup.</param>
    public static void AddModules(this IServiceCollection services, IConfiguration configuration)
    {
        services.SetupUserModule(configuration);
    }
    
}