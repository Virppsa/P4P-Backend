namespace P4P.Models;
public class UserRefreshToken
{
    public int Id { get; set; }

    public string Token { get; set; } = null!;

    public string IpAddress { get; set; } = null!;

    public DateTime CreatedDate { get; set; }

    public DateTime ExpirationDate { get; set; }

    public int UserId { get; set; }

    public User User { get; set; } = null!;
}

