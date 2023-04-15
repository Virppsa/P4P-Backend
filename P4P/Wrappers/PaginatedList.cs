using P4P.Filters;

namespace P4P.Wrappers;

public class PaginatedList<TEntity>
{
    public TEntity? Items { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalItems { get; set; }
    public int TotalPages { get; set; }

    public PaginatedList()
    {
    }

    private PaginatedList(TEntity items, PaginationFilter paginationFilter, int totalItems)
    {
        Items = items;
        Page = paginationFilter.Page;
        PageSize = paginationFilter.PageSize;
        TotalItems = totalItems;
        TotalPages = (int)Math.Ceiling((double)totalItems / PageSize);
    }

    public static PaginatedList<List<TEntity>> Create(
        List<TEntity> items,
        int totalItems,
        PaginationFilter paginationFilter
    )
    {
        return new PaginatedList<List<TEntity>>(items, paginationFilter, totalItems);
    }
}
