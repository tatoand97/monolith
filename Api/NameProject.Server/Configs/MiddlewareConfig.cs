using NameProject.Server.Middlewares;

namespace NameProject.Server.Configs;

/// <summary>
/// Provides configuration for custom middleware components used in the application's request pipeline.
/// </summary>
internal static class MiddlewareConfig
{
    /// <summary>
    /// Configures the application to use a set of custom middleware components in the request pipeline.
    /// </summary>
    /// <param name="app">The IApplicationBuilder instance used to configure the application's request pipeline.</param>
    internal static void UseCustomMiddlewares(this IApplicationBuilder app)
    {
        app.UseMiddleware<LogContextTraceMiddleware>();
        app.UseMiddleware<PathSanitizationMiddleware>();
        app.UseMiddleware<AddHeadersMiddleware>();
    }
}