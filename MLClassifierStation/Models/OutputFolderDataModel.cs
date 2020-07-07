using MLClassifierStation.Common;
using MLClassifierStation.Models.ExamplesActionStrategies;
using MLCS.LearningAlgorithmPlugins;
using MLCS.Services;
using System.Collections.Generic;

namespace MLClassifierStation.Models
{
    public class OutputFolderDataModel : DataModelBase
    {
        private readonly IModelService modelService;
        private readonly IEvaluation evaluation;
        private readonly ExampleType exampleType;

        private IDictionary<ExampleType, IProcessExamplesStrategy> examplesStrategies;

        private string outputFolderPath;

        // [Required]
        public string OutputFolderPath
        {
            get => outputFolderPath;
            set
            {
                SetProperty(ref outputFolderPath, value);
                modelService.LearningResultFolderPath = outputFolderPath;
            }
        }

        public OutputFolderDataModel(IModelService modelService, IEvaluation evaluation,
            ValidatableBindableBase parentViewModel, ExampleType exampleType)
        {
            this.modelService = modelService;
            this.evaluation = evaluation;
            this.exampleType = exampleType;
            this.parentViewModel = parentViewModel;

            InitializeProcessExamplesStrategies();

            ErrorsChanged += NotifyParentChildErrorsChanged;
        }

        private void InitializeProcessExamplesStrategies()
        {
            examplesStrategies = new Dictionary<ExampleType, IProcessExamplesStrategy>();
            examplesStrategies.Add(ExampleType.Learning, new LearnExamplesStrategy());
            examplesStrategies.Add(ExampleType.Classification, new ClassifyExamplesStrategy());
        }

        public void ProcessExamples()
        {
            examplesStrategies[exampleType].ProcessExamples(modelService, evaluation, OutputFolderPath);
        }
    }
}