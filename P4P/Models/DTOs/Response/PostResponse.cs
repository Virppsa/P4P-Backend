namespace P4P.Models.DTOs.Response;

public class PostResponse
{
    public int Id { get; set; }

    public string Text { get; set; } = null!;

    public int LocationId { get; set; }

    public PostUserResponse User { get; set; } = null!;

    public Location Location { get; set; } = null!;

    public PostLikeResponse Like { get; set; } = new();
}

public class PostUserResponse
{
    public string Name { get; set; } = null!;
    
    public string ImageName { get; set; } = null!;
}

public class PostLocationResponse
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;
    
    public string ImageName { get; set; } = null!;
}

public class PostLikeResponse
{
    public bool HasLiked { get; set; }

    public int LikeCount { get; set; }
}
