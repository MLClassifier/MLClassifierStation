using MLClassifierStation.Common;
using MLClassifierStation.Models;
using MLCS.FileParser;
using MLCS.Services;
using MvvmDialogs;
using MvvmDialogs.FrameworkDialogs.OpenFile;
using System;
using System.Windows.Input;

namespace MLClassifierStation.ViewModels
{
    public class OpenExamplesViewModel : WizardElementViewModelBase
    {
        private readonly IDialogService dialogService;
        private readonly IModelService modelService;
        private readonly IExamplesProvider examplesProvider;
        private readonly IExamplesPreviewer examplesPreviewer;
        private readonly IDataProviderFactory dataProviderFactory;
        private readonly ExampleType exampleType;

        public ExamplesDataModel ExamplesDataModel { get; set; }

        private OpenFileDialogSettings ExamplesFileSettings
        {
            get
            {
                return new OpenFileDialogSettings
                {
                    Title = "Open examples file",
                    InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                    Filter = "Text Documents (*.txt)|*.txt|All Files (*.*)|*.*"
                };
            }
        }

        public ICommand OpenExamplesFileCommand { get; set; }

        public OpenExamplesViewModel(IDialogService dialogService, IModelService modelService, IDataProviderFactory dataProviderFactory,
            IExamplesProvider examplesProvider, IExamplesPreviewer examplesPreviewer, ValidatableBindableBase parentViewModel, ExampleType exampleType)
        {
            this.dialogService = dialogService;
            this.modelService = modelService;
            this.dataProviderFactory = dataProviderFactory;
            this.examplesProvider = examplesProvider;
            this.examplesPreviewer = examplesPreviewer;
            this.parentViewModel = parentViewModel;
            this.exampleType = exampleType;

            Initialize();

            ChildErrorsChanged += FilesDataModelErrorsChanged;

            OpenExamplesFileCommand = new FLCommand(p =>
            {
                ExamplesDataModel.IsExamplesInfoVisible = false;
                ExamplesDataModel.ExamplesInformation = string.Empty;
                ExamplesDataModel.ExamplesFilePath = OpenFile(ExamplesFileSettings);
            });
        }

        private void FilesDataModelErrorsChanged(object sender, ChildErrorsChangedEventArgs e)
        {
            parentViewModel.OnChildErrorsChanged("OpenModelViewModel", e.Errors);
        }

        public override void Initialize()
        {
            ExamplesDataModel = new ExamplesDataModel(modelService, dataProviderFactory, examplesProvider, examplesPreviewer, this, exampleType);
            childViewModels.Clear();
            childViewModels.Add(ExamplesDataModel);
        }

        private string OpenFile(OpenFileDialogSettings settings)
        {
            bool? success = dialogService.ShowOpenFileDialog(this, settings);
            return success == true ? settings.FileName : string.Empty;
        }
    }
}