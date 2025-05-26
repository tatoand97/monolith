using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

namespace NameProject.Server.Configs;

[ExcludeFromCodeCoverage]
public static class HealthcheckConfig
{

    public static IServiceCollection AddHealthChecks(this IServiceCollection services)
    {
        services.AddHealthChecks()
            .AddAzureAppConfiguration();
            
            return services;
    }
    
    public static IEndpointRouteBuilder ConfigureHealthChecks(this IEndpointRouteBuilder builder)
    {
        builder.MapHealthChecks("/health/live", new HealthCheckOptions
        {
            Predicate = _ => false
        });

        builder.MapHealthChecks("/health/ready", new HealthCheckOptions
        {
            Predicate = check => check.Tags.Contains("ready")
        });

        return builder;
    }
}