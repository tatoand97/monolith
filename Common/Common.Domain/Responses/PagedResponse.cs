using Common.Domain.Pagination;

namespace Common.Domain.Responses;

public class PagedResponse<T> : Response<IReadOnlyList<T>>
{
    private int PageNumber { get; }
    public int PageSize { get; }
    private int TotalPages { get; }
    public int TotalCount { get; }
    public bool HasPreviousPage => PageNumber > 1;
    public bool HasNextPage => PageNumber < TotalPages;

    private PagedResponse(IReadOnlyList<T> data, int pageNumber, int pageSize, int totalCount, string? message = null)
        : base(data, true, message)
    {
        PageNumber = pageNumber;
        PageSize = pageSize;
        TotalCount = totalCount;
        TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
    }

    public static PagedResponse<T> FromPagedList(PagedList<T> pagedList, string? message = null)
    {
        return new PagedResponse<T>(
            pagedList.Items,
            pagedList.PageNumber,
            pagedList.PageSize,
            pagedList.TotalCount,
            message);
    }

    private PagedResponse(string? message = null, string[]? errors = null)
        : base(null, false, message, errors)
    {
        PageNumber = 0;
        PageSize = 0;
        TotalCount = 0;
        TotalPages = 0;
    }

    public new static PagedResponse<T> Fail(string? message = null, string[]? errors = null)
    {
        return new PagedResponse<T>(message, errors);
    }
}