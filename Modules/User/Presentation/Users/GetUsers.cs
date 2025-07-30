using Common.Domain.Pagination;
using Common.Domain.Responses;
using Common.Presentation.Endpoint;
using User.Application.DTOs;
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

    private async Task<IResult> Handle([AsParameters] GetUsers query, CancellationToken ct)
    {
        var pagedUsers = await messageBus.InvokeAsync<PagedList<UserDto>>(query, ct);
        var response = PagedResponse<UserDto>.FromPagedList(pagedUsers, "Usuarios obtenidos exitosamente");

        return Results.Ok(response);
    }
}
