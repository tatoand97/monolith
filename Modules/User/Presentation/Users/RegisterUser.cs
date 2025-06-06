using Application.Commands.CreateUser;
using Wolverine;

namespace Presentation.Users;

public class RegisterUser
{
    public void MapEndpoint(IEndpointRouteBuilder app, IMessageBus messageBus)
    {
        app.MapPost("/users", async (CreateUser user) =>
        {
            await messageBus.InvokeAsync(user);
        });
    }
}