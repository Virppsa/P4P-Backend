using Swashbuckle.AspNetCore.Annotations;
using System.Text.Json.Serialization;

namespace P4P.Models;

public class Like
{
    [SwaggerSchema(ReadOnly = true)]
    public int Id { get; set; }

    public int UserId { get; set; }

    public int? PostId { get; set; }

    [JsonIgnore]
    public int? CommentId { get; set; }

    public User User { get; set; } = null!;

    [JsonIgnore]
    public Post? Post { get; set; }
}
