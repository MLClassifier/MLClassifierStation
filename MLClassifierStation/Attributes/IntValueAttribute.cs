using System.ComponentModel.DataAnnotations;

namespace MLClassifierStation.Features
{
    public class IntValueAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (int.TryParse(value.ToString().Trim(), out _))
                return ValidationResult.Success;

            return new ValidationResult("Please enter a valid integer value.",
                new[] { validationContext.MemberName });
        }
    }
}
