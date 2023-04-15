namespace P4P.Models;

public class Post
{
    public int Id { get; set; }

    public string Text { get; set; } = null!;

    public int LocationId { get; set; }

    public int UserId { get; set; }

    public Location? Location { get; set; }

    public User User { get; set; } = null!;

    public List<Like> Likes { get; set; } = new();
}
