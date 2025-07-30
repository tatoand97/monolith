namespace Common.Domain.Pagination;

public record PaginationParams
{
    private const int MaxPageSize = 50;

    public int PageNumber { get; init; } = 1;

    private readonly int _pageSize = 10;
    public int PageSize
    {
        get => _pageSize;
        init => _pageSize = Math.Min(value, MaxPageSize);
    }
}
