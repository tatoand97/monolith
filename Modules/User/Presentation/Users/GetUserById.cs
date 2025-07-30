using Common.Presentation.Endpoint;
using User.Application.Commands.CreateUser;
using Wolverine;

namespace User.Presentation.Users;

public class GetUserById(IMessageBus messageBus)
{
    public void MapEndpoint(IEndpointRouteBuilder e)
    {
        e.MapGet("/users/{id}", Handle)
            .AllowAnonymous()
            .WithName("Users.GetById");
    }

    private async Task<IResult> Handle(CreateUser request, CancellationToken ct)
    {
        await messageBus.InvokeAsync(request, ct);

        return Results.Ok();
    }
}