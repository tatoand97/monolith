using Common.Domain.Pagination;

namespace User.Application.Queries.GetUsers;

public abstract class GetUsers(int pageNumber = 1, int pageSize = 10)
{
    public PaginationParams ToPaginationParams() => new()
    {
        PageNumber = pageNumber,
        PageSize = pageSize
    };
}
