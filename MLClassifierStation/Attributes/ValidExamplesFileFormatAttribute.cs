using MLCS.Common;
using MLCS.Entities.Features;
using MLCS.FileParser;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace MLClassifierStation.Features
{
    internal class ValidExamplesFileFormatAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
                return ValidationResult.Success;

            var examplesProvider = (IExamplesProvider)validationContext.GetService(typeof(IExamplesProvider));
            var dataProviderFactory = (IDataProviderFactory)validationContext.GetService(typeof(IDataProviderFactory));

            var containerType = validationContext.ObjectInstance.GetType();
            var property = containerType.GetProperty(validationContext.MemberName);
            if (property == null)
                return new ValidationResult("Error while reading examples file path property.");

            var examplesFilePathPropertyValue = property.GetValue(validationContext.ObjectInstance, null);
            string examplesFilePath = examplesFilePathPropertyValue.ToString();

            if (!File.Exists(examplesFilePath))
                return ValidationResult.Success;    // already handled in FileExistsFeature

            IDataProvider examplesDataProvider = dataProviderFactory.Create(examplesFilePath);

            IEnumerable<IFeature> features = (IEnumerable<IFeature>)validationContext.Items[Constants.FeaturesValidationContextItemKey];

            int exampleLineIndexResult = examplesProvider.IsCorrectFormat(examplesDataProvider, features);

            return exampleLineIndexResult == Constants.CorrectFormatLineIndexResult
                ? ValidationResult.Success
                : new ValidationResult(
                    string.Format("Error while parsing examples file - line number {0}.", exampleLineIndexResult),
                    new string[] { validationContext.MemberName });
        }
    }
}