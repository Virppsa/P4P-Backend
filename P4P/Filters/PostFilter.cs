namespace P4P.Filters;

public class PostFilter : PaginationFilter
{
    public int MinStars { get; set; }

    public int? LocationId { get; set; } = null;
}
