using Microsoft.EntityFrameworkCore;

namespace P4P.Models;

[Keyless]
public class Rating
{
    public int PostId { get; set; }

    public int UserId { get; set; }

    public int Value { get; set; }
}
