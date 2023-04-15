using System.ComponentModel.DataAnnotations;
using P4P.Attributes;
using P4P.Constants;

namespace P4P.Models.DTOs.Request;

public class LocationRatingRequest
{
    [Required(ErrorMessage = "Šis laukas negali būti tuščias")]
    [Range(LocationConstants.MinRating, LocationConstants.MaxRating)]
    public int Rating { get; set; }
}
