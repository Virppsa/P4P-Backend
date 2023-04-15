using System.ComponentModel.DataAnnotations;
using P4P.Constants;

namespace P4P.Models.DTOs.Request;

public class LoginRequest
{
    [Required(ErrorMessage = "Šis laukas negali būti tuščias")]
    [EmailAddress]
    [MaxLength(CommonConstants.MaxStringLength)]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "Šis laukas negali būti tuščias")]
    [MinLength(UserConstants.MinPasswordLength)]
    [MaxLength(CommonConstants.MaxStringLength)]
    public string Password { get; set; } = null!;
}
