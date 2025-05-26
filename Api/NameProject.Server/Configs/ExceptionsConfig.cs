using NameProject.Server.Handlers;

namespace NameProject.Server.Configs;

public static class ExceptionsConfig
{
    /// Configures exception handling services for dependency injection.
    /// <param name="services">
    /// The service collection to which the exception handling services are added.
    /// </param>
    public static void SetupExceptions(this IServiceCollection services)
    {
        services.AddProblemDetails();
        services.AddExceptionHandler<GlobalExceptionHandler>();
    }
}