using System.ComponentModel.DataAnnotations.Schema;

namespace P4P.Models;

public class Location
{
    public Location()
    {
        _stars = new Lazy<double>(() => Ratings.Count > 0
            ? Ratings.Select(x => x[0] - '0').Average()
            : 0.0
        );
    }

    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public float X { get; set; }

    public float Y { get; set; }

    // Format: "{rating}-{userId}"
    
    public List<string> Ratings { get; set; } = new();

    [NotMapped]
    public double Stars => _stars.Value;

    // 2. Lazy initialization usage
    private readonly Lazy<double> _stars;

    public string ImageName { get; set; } = null!;
}
