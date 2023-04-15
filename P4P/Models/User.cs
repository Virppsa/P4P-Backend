using System.ComponentModel.DataAnnotations;
using P4P.Constants;

namespace P4P.Models;

public class User
{
    public int Id { get; set; }

    [MaxLength(UserConstants.MaxPasswordLength)]
    public string Name { get; set; } = null!;

    [MaxLength(UserConstants.MaxPasswordLength)]
    public string Email { get; set; } = null!;

    [MaxLength(UserConstants.MaxPasswordLength)]
    public string Password { get; set; } = null!;

    public bool Verified { get; set; }

    public string ImageName { get; set; } = null!;
}
