namespace NameProject.Server.Configs;

public static class CorsConfig
{
    public const string MyPolicy = "MyPolicy";

    /// <summary>
    /// Configures Cross-Origin Resource Sharing (CORS) for the application by adding a custom policy.
    /// </summary>
    /// <param name="services">The IServiceCollection instance to which the CORS configuration and policy are added.</param>
    /// <returns>The modified IServiceCollection with the CORS configuration applied.</returns>
    public static IServiceCollection SetupCors(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy(MyPolicy, builder =>
            {
                builder.AllowAnyMethod()
                    .AllowAnyOrigin()
                    //.WithOrigins("https://example.com", "https://another.com")
                    .AllowAnyHeader();
            });
        });
        
        services.AddRouting(r => r.SuppressCheckForUnhandledSecurityMetadata = true);
        
        return services;
    }
}