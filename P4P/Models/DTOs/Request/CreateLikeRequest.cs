namespace P4P.Models.DTOs.Request;

public class CreateLikeRequest
{
    public int? PostId { get; set; }

    public int? CommentId { get; set; }
}
