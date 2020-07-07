using System.ComponentModel.DataAnnotations;
using System.IO;

namespace MLClassifierStation.Features
{
    public class FileExistsAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
                return ValidationResult.Success;

            string filePath = value.ToString();
            if (File.Exists(filePath))
                return ValidationResult.Success;

            return new ValidationResult(string.Format("File {0} does not exist.", filePath),
                new[] { validationContext.MemberName });
        }
    }
}