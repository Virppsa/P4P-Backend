namespace P4P.Models.DTOs.Response;

public class LocationResponse
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public float X { get; set; }

    public float Y { get; set; }

    public double Stars { get; set; }

    public string ImageName { get; set; } = null!;
}
