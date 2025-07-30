namespace Common.Domain.Pagination;

public class PagedList<T>(IReadOnlyList<T> items, int totalCount, int pageNumber, int pageSize)
{
    public IReadOnlyList<T> Items { get; } = items;
    public int PageNumber { get; } = pageNumber;
    private int TotalPages { get; } = (int)Math.Ceiling(totalCount / (double)pageSize);
    public int TotalCount { get; } = totalCount;
    public int PageSize { get; } = pageSize;
    public bool HasPreviousPage => PageNumber > 1;
    public bool HasNextPage => PageNumber < TotalPages;
}
