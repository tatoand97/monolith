using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Common.Presentation.Endpoint;

/// Provides extension methods for mapping endpoints to a <see cref="WebApplication"/>.
/// This class is designed to enable the dynamic registration of endpoints from assemblies.
public static class EndpointExtensions
{
    /// Maps the endpoints defined in the specified assembly to the application.
    /// This method scans the provided assembly for types implementing the IEndpoint interface
    /// and invokes their MapEndpoint method to configure routing or other application-specific
    /// logic. If no custom route builder is provided, the application's default route builder is used.
    /// <param name="app">
    /// The <see cref="WebApplication"/> instance to configure.
    /// </param>
    /// <param name="assembly">
    /// The assembly to scan for types implementing the <see cref="IEndpoint"/> interface.
    /// </param>
    /// <param name="routeBuilder">
    /// Optional. An instance of <see cref="IEndpointRouteBuilder"/> to be used for mapping endpoints.
    /// If not provided, the default route builder of the <see cref="WebApplication"/> is used.
    /// </param>
    public static void MapEndpoints(this WebApplication app,
        Assembly assembly,
        IEndpointRouteBuilder? routeBuilder = null)
    {
        using var scope = app.Services.CreateScope();
        var provider = scope.ServiceProvider;
        var builder = routeBuilder ?? app;

        var types = SafeGetTypes(assembly)
            .Where(t => t is { IsAbstract: false, IsInterface: false, ContainsGenericParameters: false }
                        && typeof(IEndpoint).IsAssignableFrom(t));

        var count = 0;
        foreach (var t in types)
        {
            var ep = (IEndpoint)ActivatorUtilities.CreateInstance(provider, t);
            ep.MapEndpoint(builder);
            count++;
        }

        app.Logger.LogInformation("MapEndpoints({Asm}): {Count} endpoints.",
            assembly.GetName().Name, count);
    }

    private static IEnumerable<Type> SafeGetTypes(Assembly assembly)
    {
        try { return assembly.DefinedTypes.Select(ti => ti.AsType()); }
        catch (ReflectionTypeLoadException ex) { return ex.Types.Where(t => t is not null)!; }
    }
}