using MLClassifierStation.Features;
using MLClassifierStation.Common;
using MLClassifierStation.Models.GetExamplesStrategies;
using MLClassifierStation.Providers;
using MLCS.Common;
using MLCS.Common.Utils;
using MLCS.Entities.Vectors;
using MLCS.FileParser;
using MLCS.Services;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace MLClassifierStation.Models
{
    public class ExamplesDataModel : DataModelBase
    {
        private readonly IModelService modelService;
        private readonly IExamplesProvider examplesProvider;
        private readonly IExamplesPreviewer examplesPreviewer;
        private readonly IDataProviderFactory dataProviderFactory;
        private readonly ExampleType exampleType;

        private IDictionary<ExampleType, IGetExamplesStrategy> examplesStrategies;

        private string examplesFilePath;

        [Required(ErrorMessage = "The examples file path is required")]
        [FileExists]
        [ValidExamplesFileFormat]
        public string ExamplesFilePath
        {
            get => examplesFilePath;
            set
            {
                if (modelService.Features == null || !modelService.Features.Any()) return;

                ValidationContext.Items.Remove(Constants.FeaturesValidationContextItemKey);
                ValidationContext.Items.Add(new KeyValuePair<object, object>(Constants.FeaturesValidationContextItemKey, modelService.Features));

                SetProperty(ref examplesFilePath, value);

                bool hasValidationErrors = GetErrors("ExamplesFilePath").IsNotEmpty();
                if (hasValidationErrors) return;

                if (string.IsNullOrWhiteSpace(ExamplesFilePath)) return;

                LoadExamples();
            }
        }

        private IEnumerable<IVector> examples;

        public IEnumerable<IVector> Examples
        {
            get => examples;
            set => SetProperty(ref examples, value);
        }

        private string examplesInformation;

        public string ExamplesInformation
        {
            get => examplesInformation;
            set => SetProperty(ref examplesInformation, value);
        }

        private bool isExamplesInfoVisible;

        public bool IsExamplesInfoVisible
        {
            get => isExamplesInfoVisible;
            set => SetProperty(ref isExamplesInfoVisible, value);
        }

        public ExamplesDataModel(IModelService modelService, IDataProviderFactory dataProviderFactory,
           IExamplesProvider examplesProvider, IExamplesPreviewer examplesPreviewer,
           ValidatableBindableBase parentViewModel, ExampleType exampleType)
        {
            this.modelService = modelService;
            this.dataProviderFactory = dataProviderFactory;
            this.examplesProvider = examplesProvider;
            this.examplesPreviewer = examplesPreviewer;
            this.parentViewModel = parentViewModel;
            this.exampleType = exampleType;

            SetValidationContextServices();

            InitializeGetExamplesStrategies();

            ErrorsChanged += NotifyParentChildErrorsChanged;
        }

        private void SetValidationContextServices()
        {
            ValidationServiceProvider serviceProvider = new ValidationServiceProvider();
            serviceProvider.AddService(typeof(IExamplesProvider), examplesProvider);
            serviceProvider.AddService(typeof(IDataProviderFactory), dataProviderFactory);
            ValidationContext.InitializeServiceProvider(serviceProvider.GetService);
        }

        private void InitializeGetExamplesStrategies()
        {
            examplesStrategies = new Dictionary<ExampleType, IGetExamplesStrategy>();
            examplesStrategies.Add(ExampleType.Learning, new GetLearningExamplesStrategy());
            examplesStrategies.Add(ExampleType.Classification, new GetClassificationExamplesStrategy());
        }

        public void Clear()
        {
            examplesFilePath = string.Empty;
            ExamplesInformation = string.Empty;
            IsExamplesInfoVisible = false;
        }

        private void LoadExamples()
        {
            IDataProvider examplesDataProvider = dataProviderFactory.Create(examplesFilePath);

            Examples = examplesStrategies[exampleType].GetExamples(modelService, examplesDataProvider, examplesProvider);

            ValidationContext.Items.Remove(Constants.ExamplesValidationContextItemKey);
            ValidationContext.Items.Add(new KeyValuePair<object, object>(Constants.ExamplesValidationContextItemKey, Examples));

            ExamplesInformation = examplesPreviewer.Preview(examples);
            IsExamplesInfoVisible = true;
        }
    }
}