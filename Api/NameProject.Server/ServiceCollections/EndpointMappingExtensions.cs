using System.Reflection;
using Common.Presentation.Endpoint;

namespace NameProject.Server.ServiceCollections;

public static class EndpointMappingExtensions
{
    /// <summary>
    /// Maps module-specific endpoints to the provided <see cref="WebApplication"/> instance.
    /// </summary>
    /// <param name="app">
    /// The <see cref="WebApplication"/> instance to which the module endpoints will be mapped.
    /// </param>
    public static void MapModuleEndpoints(this WebApplication app)
    {
        Assembly[] modules =
        [
            User.Presentation.AssemblyReference.Assembly
        ];

        foreach (var asm in modules)
            app.MapEndpoints(asm);
    }
}