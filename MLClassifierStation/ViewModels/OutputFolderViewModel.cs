using MLClassifierStation.Common;
using MLClassifierStation.Models;
using MLCS.LearningAlgorithmPlugins;
using MLCS.Services;
using MvvmDialogs;
using MvvmDialogs.FrameworkDialogs.FolderBrowser;

namespace MLClassifierStation.ViewModels
{
    public class OutputFolderViewModel : WizardElementViewModelBase
    {
        private readonly IDialogService dialogService;
        private readonly IModelService modelService;
        private readonly IEvaluation evaluation;
        private readonly ExampleType exampleType;

        private FolderBrowserDialogSettings OutputFolderSettings
        {
            get
            {
                return new FolderBrowserDialogSettings
                {
                    Description = "Pick output folder"
                };
            }
        }

        public OutputFolderDataModel OutputFolderDataModel { get; set; }

        public FLCommand PickOutputFolderCommand { get; set; }

        public OutputFolderViewModel(IDialogService dialogService, IModelService modelService,
            IEvaluation evaluation, ValidatableBindableBase parentViewModel, ExampleType exampleType)
        {
            this.dialogService = dialogService;
            this.modelService = modelService;
            this.evaluation = evaluation;
            this.exampleType = exampleType;
            this.parentViewModel = parentViewModel;

            PickOutputFolderCommand = new FLCommand(p =>
            {
                OutputFolderDataModel.OutputFolderPath = PickOutputFolder(OutputFolderSettings);
            });
        }

        private string PickOutputFolder(FolderBrowserDialogSettings settings)
        {
            bool? success = dialogService.ShowFolderBrowserDialog(this, settings);
            return success == true ? settings.SelectedPath : string.Empty;
        }

        public override void Initialize()
        {
            OutputFolderDataModel = new OutputFolderDataModel(modelService, evaluation, parentViewModel, exampleType);
        }

        public void ProcessExamples()
        {
            OutputFolderDataModel.ProcessExamples();
        }
    }
}