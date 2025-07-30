using Common.Domain.Pagination;
using User.Application.DTOs;
using User.Application.Mappers;
using User.Domain.Interfaces;

namespace User.Application.Queries.GetUsers;

public class GetUsersHandler(IUnitOfWork unitOfWork)
{
    public async Task<PagedList<UserDto>> Handle(GetUsers query, CancellationToken ct)
    {
        var paginationParams = query.ToPaginationParams();
        var pagedEntities = await unitOfWork.Users.ListPagedAsync(paginationParams, ct: ct);

        var dtoItems = pagedEntities.Items.ToDtoList();
        return new PagedList<UserDto>(dtoItems, pagedEntities.TotalCount, pagedEntities.PageNumber, pagedEntities.PageSize);
    }
}
