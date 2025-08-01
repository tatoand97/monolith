﻿using System.Diagnostics.CodeAnalysis;
using Azure.Identity;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using MongoDB.Driver.Core.Extensions.DiagnosticSources;

namespace NameProject.Server.ServiceCollections;

[ExcludeFromCodeCoverage]
public static class ConfigureServiceExtensions
{
    /// <summary>
    /// Adds and configures Swagger services for the API.
    /// </summary>
    /// <param name="services">
    /// The <see cref="IServiceCollection"/> to which the Swagger services will be added.
    /// </param>
    public static void AddSwaggerConfiguration(this IServiceCollection services)
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
    }

    /// Configures the Swagger middleware for the application.
    /// This method enables Swagger and Swagger UI to document and test APIs.
    /// <param name="app">
    /// The <see cref="IApplicationBuilder"/> used to configure the application's middleware pipeline.
    /// </param>
    public static void UseSwaggerConfiguration(this IApplicationBuilder app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "NameProject API v1"));
    }

    /// Configures the application's configuration using Azure App Configuration.
    /// This method connects to an Azure App Configuration store and sets up
    /// application settings with optional Key Vault integration and refresh functionality.
    /// <param name="builder">
    /// The <see cref="WebApplicationBuilder"/> used to configure the application.
    /// </param>
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

    public static void AddMongoClient(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<MongoClient>(_ =>
        {
            var dbConnection = configuration.GetConnectionString("MongoDb:StringConnection");
            var clientSettings = MongoClientSettings.FromConnectionString(dbConnection);
            clientSettings.ClusterConfigurator = builder => builder.Subscribe(new DiagnosticsActivityEventSubscriber());
            return new MongoClient(clientSettings);
        });
}
}