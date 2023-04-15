using System.ComponentModel.DataAnnotations;
using P4P.Constants;

namespace P4P.Models.DTOs.Request;

public class CreateLocationRequest
{
    [Required(ErrorMessage = "Šis laukas negali būti tuščias")]
    [MaxLength(CommonConstants.MaxStringLength)]
    public string Name { get; set; } = null!;

    [Required(ErrorMessage = "Šis laukas negali būti tuščias")]
    [MaxLength(CommonConstants.MaxStringLength)]
    public string Description { get; set; } = null!;

    [Required(ErrorMessage = "Šis laukas negali būti tuščias")]
    public float X { get; set; }

    [Required(ErrorMessage = "Šis laukas negali būti tuščias")]
    public float Y { get; set; }

    [Required(ErrorMessage = "Šis laukas negali būti tuščias")] 
    public IFormFile Image { get; set; } = null!;
}
