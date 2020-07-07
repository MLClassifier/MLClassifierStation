using MLClassifierStation.Common;
using MLCS.Common;
using MLCS.Entities.Features;
using MLCS.Entities.Features.Default;
using MLCS.Entities.Vectors;
using MLCS.FileParser;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace MLClassifierStation.Features
{
    public class FeaturesFormatCorrespondToExamplesFormatAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
                return ValidationResult.Success;

            if (!validationContext.Items.ContainsKey(Constants.ExamplesValidationContextItemKey))
                return ValidationResult.Success;
            IEnumerable<IVector> examples = (IEnumerable<IVector>)validationContext.Items[Constants.ExamplesValidationContextItemKey];
            if (examples == null || !examples.Any())
                return ValidationResult.Success;

            ValidationContext objectValidationContext = ((ValidatableBindableBase)validationContext.ObjectInstance).ValidationContext;

            var featuresProvider = (IFeaturesProvider)objectValidationContext.GetService(typeof(IFeaturesProvider));
            var dataProviderFactory = (IDataProviderFactory)objectValidationContext.GetService(typeof(IDataProviderFactory));

            var containerType = validationContext.ObjectInstance.GetType();
            var property = containerType.GetProperty(validationContext.MemberName);
            if (property == null)
                return new ValidationResult("Error while reading features file path property.");

            var featuresFilePathPropertyValue = property.GetValue(validationContext.ObjectInstance, null);
            string featuresFilePath = featuresFilePathPropertyValue.ToString();
            IDataProvider featuresDataProvider = dataProviderFactory.Create(featuresFilePath);

            int featureLineIndexResult = featuresProvider.IsCorrectFormat(featuresDataProvider);
            if (featureLineIndexResult != Constants.CorrectFormatLineIndexResult)
                return ValidationResult.Success;    // already handled in ValidFeaturesFileFormatFeature

            IEnumerable<IFeature> features = featuresProvider.GetFeatures(featuresDataProvider);

            IVector example = examples.First();
            bool isMatch = true;
            foreach (IFeature feature in features)
            {
                if (!example.Keys.Contains(feature, new FeatureComparer()))
                {
                    isMatch = false;
                    break;
                }
            }

            return isMatch
                ? ValidationResult.Success
                : new ValidationResult(
                    "The features file does not correspond to the specified examples file. Please change either the features file or the examples file.",
                    new string[] { validationContext.MemberName });
        }
    }
}