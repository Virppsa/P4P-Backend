using P4P.Wrappers;

namespace P4P.Models.DTOs.Response;

public class LocationByIdResponse
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public float X { get; set; }

    public float Y { get; set; }

    public double Stars { get; set; }

    public string ImageName { get; set; } = null!;

    public PaginatedList<List<LocationByIdPostResponse>> Posts { get; set; } = null!;

}

public class LocationByIdPostResponse
{
    public int Id { get; set; }

    public string Text { get; set; } = null!;

    public LocationByIdUserResponse User { get; set; } = null!;
}

public class LocationByIdUserResponse
{
    public string Name { get; set; } = null!;
}
