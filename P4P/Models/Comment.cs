using Swashbuckle.AspNetCore.Annotations;

namespace P4P.Models;

public class Comment
{
    [SwaggerSchema(ReadOnly = true)]
    public int Id { get; set; }

    public string Text { get; set; } = null!;

    public int UserId { get; set; }

    public int PostId { get; set; }

    public User User { get; set; } = null!;

    public Post Post { get; set; } = null!;

    public List<Like> Likes { get; set; } = null!;
}
