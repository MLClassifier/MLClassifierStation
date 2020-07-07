using System.ComponentModel.DataAnnotations;

namespace MLClassifierStation.Features
{
    public class ItemSelectedAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
                return ValidationResult.Success;

            return new ValidationResult("Please select an item",
                new[] { validationContext.MemberName });
        }
    }
}
