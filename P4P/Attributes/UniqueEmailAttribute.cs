using System.ComponentModel.DataAnnotations;
using P4P.Services.Interfaces;

namespace P4P.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class UniqueEmailAttribute : ValidationAttribute
{
    private new const string ErrorMessage = "Email is already in use.";

    protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
    {
        var userService = (IUserService)validationContext.GetService(typeof(IUserService))!;
        
        if (value is not string s || userService.ExistsByEmail(s).Result == false)
        {
            return ValidationResult.Success;
        }

        return new ValidationResult(ErrorMessage);
    }
}
