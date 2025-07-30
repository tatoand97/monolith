using Application.Commands.CreateUser;
using Common.Presentation.Endpoint;
using Wolverine;

namespace User.Presentation.Users;

internal sealed class RegisterUser(IMessageBus messageBus) : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder e)
    {
        e.MapPost("/users", Handle)
            .AllowAnonymous()
            .WithName("Users.Register");
    }

    private async Task<IResult> Handle(CreateUser request, CancellationToken ct)
    {
        await messageBus.InvokeAsync(request, ct);

        return Results.Ok();
    }
}