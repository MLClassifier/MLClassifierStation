using MLClassifierStation.Features;
using MLClassifierStation.Common;
using MLClassifierStation.Providers;
using MLCS.Common;
using MLCS.Common.Utils;
using MLCS.Entities.Features;
using MLCS.FileParser;
using MLCS.Services;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MLClassifierStation.Models
{
    public class FeaturesDataModel : DataModelBase
    {
        private readonly IModelService modelService;
        private readonly IFeaturesProvider featuresProvider;
        private readonly IFeaturesPreviewer featuresPreviewer;
        private readonly IDataProviderFactory dataProviderFactory;

        private string featuresFilePath;

        [Required(ErrorMessage = "The features file path is required")]
        [FileExists]
        [ValidFeaturesFileFormat]
        public string FeaturesFilePath
        {
            get => featuresFilePath;
            set
            {
                SetProperty(ref featuresFilePath, value);

                bool hasValidationErrors = GetErrors("FeaturesFilePath").IsNotEmpty();
                if (hasValidationErrors)
                {
                    modelService.Clear();
                    IsFeaturesInfoVisible = false;
                    return;
                }

                IDataProvider featuresDataProvider = dataProviderFactory.Create(featuresFilePath);
                Features = featuresProvider.GetFeatures(featuresDataProvider);
                modelService.Features = Features;

                ValidationContext.Items.Remove(Constants.FeaturesValidationContextItemKey);
                ValidationContext.Items.Add(new KeyValuePair<object, object>(Constants.FeaturesValidationContextItemKey, Features));

                FeaturesInformation = featuresPreviewer.Preview(features);
                IsFeaturesInfoVisible = true;
            }
        }

        private IEnumerable<IFeature> features;

        public IEnumerable<IFeature> Features
        {
            get => features;
            set => SetProperty(ref features, value);
        }

        private string featuresInformation;

        public string FeaturesInformation
        {
            get => featuresInformation; 
            set => SetProperty(ref featuresInformation, value);
        }

        private bool isFeaturesInfoVisible;

        public bool IsFeaturesInfoVisible
        {
            get => isFeaturesInfoVisible;
            set => SetProperty(ref isFeaturesInfoVisible, value);
        }

        public FeaturesDataModel(IModelService modelService, IDataProviderFactory dataProviderFactory,
            IFeaturesProvider featuresProvider, IFeaturesPreviewer featuresPreviewer, ValidatableBindableBase parentViewModel)
        {
            this.modelService = modelService;
            this.dataProviderFactory = dataProviderFactory;
            this.featuresProvider = featuresProvider;
            this.featuresPreviewer = featuresPreviewer;
            this.parentViewModel = parentViewModel;

            SetValidationContextServices();

            ErrorsChanged += NotifyParentChildErrorsChanged;
        }

        private void SetValidationContextServices()
        {
            ValidationServiceProvider serviceProvider = new ValidationServiceProvider();
            serviceProvider.AddService(typeof(IFeaturesProvider), featuresProvider);
            serviceProvider.AddService(typeof(IDataProviderFactory), dataProviderFactory);
            ValidationContext.InitializeServiceProvider(serviceProvider.GetService);
        }

        public void Clear()
        {
            featuresFilePath = string.Empty;
            FeaturesInformation = string.Empty;
            IsFeaturesInfoVisible = false;
        }
    }
}