using System.Diagnostics.CodeAnalysis;
using Azure.Identity;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using Microsoft.OpenApi.Models;

namespace NameProject.Server.ServiceCollections;

[ExcludeFromCodeCoverage]
public static class ConfigureServiceExtensions
{
    public static IServiceCollection AddSwaggerConfiguration(this IServiceCollection services)
    {
        
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "NameProject API",
                Description = "API REST de NameProject"
            });
        });

        return services;
    }

    public static IApplicationBuilder UseSwaggerConfiguration(this IApplicationBuilder app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "NameProject API v1"));
        
        return app;
    }

    public static void ConfigureAppConfiguration(this WebApplicationBuilder builder)
    {
        var connectionString = builder.Configuration["AppConfiguration:Endpoint"];
        var durationCacheSeconds = builder.Configuration.GetValue<int?>("AppConfiguration:DurationCacheMinutes") ?? 60;

        builder.Configuration.AddAzureAppConfiguration(options =>
        {
            if (connectionString == null) return;
            var connect = connectionString.StartsWith("Endpoint", StringComparison.InvariantCulture)
                ? options.Connect(connectionString)
                : options.Connect(new Uri(connectionString), new DefaultAzureCredential());

            connect.Select(KeyFilter.Any, AppDomain.CurrentDomain.FriendlyName).ConfigureRefresh(refresh =>
            {
                refresh.Register("Settings.Sentinel", true)
                    .SetRefreshInterval(TimeSpan.FromMinutes(durationCacheSeconds));
            }).ConfigureKeyVault(vaultOptions => vaultOptions.SetCredential(new DefaultAzureCredential()));
        });
    }
}