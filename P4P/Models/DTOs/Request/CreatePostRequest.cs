using System.ComponentModel.DataAnnotations;
using P4P.Constants;

namespace P4P.Models.DTOs.Request;

public class CreatePostRequest
{
    [Required(ErrorMessage = "Rekomendacijos laukas negali būti tuščias")]
    public string Text { get; set; } = null!;

    [Required(ErrorMessage = "Šis laukas negali būti tuščias")]
    public int LocationId { get; set; }
}
