using Common.Domain.Responses;
using Common.Presentation.Endpoint;
using User.Application.Queries.GetUserById;
using User.Application.Responses;
using Wolverine;

namespace User.Presentation.Users;

internal sealed class GetUserByIdEndpoint(IMessageBus messageBus) : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder e)
    {
        e.MapGet("/users/{id:guid}", Handle)
            .WithName("Users.GetById");
    }

    private async Task<IResult> Handle(Guid id, CancellationToken ct)
    {
        var request = new GetUserById(id);
        var response = await messageBus.InvokeAsync<Response<UserResponse>>(request, ct);

        return !response.Success ? Results.NotFound(response) : Results.Ok(response);
    }
}
