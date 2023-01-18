using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class ProbationPeriodValidation : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (Convert.ToString(value) == "" )
                return new ValidationResult(ErrorMessage);
            else
                return ValidationResult.Success;
        }
    }
}