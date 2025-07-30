using Common.Presentation.Endpoint;
using Wolverine;

namespace User.Presentation.Users;

public class GetUsers(IMessageBus messageBus) : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder e)
    {
        e.MapGet("/users", Handle)
            .AllowAnonymous()
            .WithName("Users.Get");
    }

    private static Task<IResult> Handle(CancellationToken ct)
    {
        //await messageBus.InvokeAsync(request, ct);

        return Task.FromResult(Results.Ok());
    }
}