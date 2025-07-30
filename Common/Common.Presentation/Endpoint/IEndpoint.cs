using Microsoft.AspNetCore.Routing;

namespace Common.Presentation.Endpoint;
public interface IEndpoint
{
    void MapEndpoint(IEndpointRouteBuilder endpoints);
}
