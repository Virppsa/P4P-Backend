namespace P4P.Filters;

public class LocationFilter : PaginationFilter
{
    public float X1 { get; set; } = -180;

    public float X2 { get; set; } = 180;

    public float Y1 { get; set; } = -90;

    public float Y2 { get; set; } = 90;
}
