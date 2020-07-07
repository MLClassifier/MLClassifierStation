using MLClassifierStation.Common;
using MLCS.Common;
using MLCS.FileParser;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace MLClassifierStation.Features
{
    public class ValidFeaturesFileFormatAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
                return ValidationResult.Success;

            ValidationContext objectValidationContext = ((ValidatableBindableBase)validationContext.ObjectInstance).ValidationContext;

            var featuresProvider = (IFeaturesProvider)objectValidationContext.GetService(typeof(IFeaturesProvider));
            var dataProviderFactory = (IDataProviderFactory)objectValidationContext.GetService(typeof(IDataProviderFactory));

            var containerType = validationContext.ObjectInstance.GetType();
            var property = containerType.GetProperty(validationContext.MemberName);
            if (property == null)
                return new ValidationResult("Error while reading feature file path property.");

            var propertyValue = property.GetValue(validationContext.ObjectInstance, null);
            string featuresFilePath = propertyValue.ToString();

            if (!File.Exists(featuresFilePath))
                return ValidationResult.Success;    // already handled in FileExistsFeature

            IDataProvider featuresDataProvider = dataProviderFactory.Create(featuresFilePath);

            int featureLineIndexResult = featuresProvider.IsCorrectFormat(featuresDataProvider);

            return featureLineIndexResult == Constants.CorrectFormatLineIndexResult
                ? ValidationResult.Success
                : new ValidationResult(
                    string.Format("Error while parsing features file - line number {0}.", featureLineIndexResult),
                    new string[] { validationContext.MemberName });
        }
    }
}