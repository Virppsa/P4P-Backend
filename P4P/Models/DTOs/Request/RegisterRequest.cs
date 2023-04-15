using System.ComponentModel.DataAnnotations;
using P4P.Attributes;
using P4P.Constants;

namespace P4P.Models.DTOs.Request;

public class RegisterRequest
{
    [Required(ErrorMessage = "Šis laukas negali būti tuščias")]
    [MaxLength(CommonConstants.MaxStringLength)]
    public string Name { get; set; } = null!;

    [Required(ErrorMessage = "Šis laukas negali būti tuščias")]
    [EmailAddress]
    [MaxLength(CommonConstants.MaxStringLength)]
    [UniqueEmail]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "Šis laukas negali būti tuščias")]
    [MinLength(UserConstants.MinPasswordLength)]
    [MaxLength(UserConstants.MaxPasswordLength)]
    public string Password { get; set; } = null!;

    public IFormFile? Image { get; set; }
}
