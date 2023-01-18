using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class UserTypeValidation : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (Convert.ToString(value) == "0")
                return new ValidationResult(ErrorMessage);
            else
                return ValidationResult.Success;
        }
    }
}