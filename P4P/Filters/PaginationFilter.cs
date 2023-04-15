using P4P.Constants;

namespace P4P.Filters;

public class PaginationFilter
{
    public int Page { get; set; }
    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value is < PaginationConstants.MinPageSize or > PaginationConstants.MaxPageSize
            ? PaginationConstants.PageSize
            : value;
    }
    private int _pageSize;

    public PaginationFilter()
    {
        Page = PaginationConstants.DefaultPage;
        _pageSize = PaginationConstants.PageSize;
    }
}
