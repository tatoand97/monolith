using Common.Domain.Responses;
using Common.Presentation.Endpoint;
using User.Application.Commands.CreateUser;
using User.Application.Responses;
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
        var response = await messageBus.InvokeAsync<Response<RegisterResponse>>(request, ct);

        if (response.Data != null)
            return !response.Success
                ? Results.BadRequest((object?)response)
                : Results.Created($"/users/{response.Data.Id}", (object?)response);
        return Results.BadRequest((object?)response);
    }
}