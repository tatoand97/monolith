using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using NameProject.Server.Utils.HealthChecks;

namespace NameProject.Server.Configs;

[ExcludeFromCodeCoverage]
public static class HealthcheckConfig
{
    public static IServiceCollection ConfigureHealthCheckServices(this IServiceCollection services)
    {
        services.AddHealthChecks()
            .AddCheck<CosmosMongoHealthCheck>("cosmos-mongo", tags: ["ready"]);

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