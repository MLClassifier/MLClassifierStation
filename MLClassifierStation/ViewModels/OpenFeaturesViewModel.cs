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
    public class OpenFeaturesViewModel : WizardElementViewModelBase
    {
        private readonly IDialogService dialogService;
        private readonly IModelService modelService;
        private readonly IFeaturesProvider featuresProvider;
        private readonly IFeaturesPreviewer featuresPreviewer;
        private readonly IDataProviderFactory dataProviderFactory;

        public FeaturesDataModel FeaturesDataModel { get; set; }

        private OpenFileDialogSettings FeaturesFileSettings
        {
            get
            {
                return new OpenFileDialogSettings
                {
                    Title = "Open features file",
                    InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                    Filter = "Text Documents (*.txt)|*.txt|All Files (*.*)|*.*"
                };
            }
        }

        public ICommand OpenFeaturesFileCommand { get; set; }

        public OpenFeaturesViewModel(IDialogService dialogService, IModelService modelService, IDataProviderFactory dataProviderFactory,
            IFeaturesProvider featuresProvider, IFeaturesPreviewer featuresPreviewer, ValidatableBindableBase parentViewModel)
        {
            this.dialogService = dialogService;
            this.modelService = modelService;
            this.dataProviderFactory = dataProviderFactory;
            this.featuresProvider = featuresProvider;
            this.featuresPreviewer = featuresPreviewer;
            this.parentViewModel = parentViewModel;

            //FeaturesDataModel = new FeaturesDataModel(modelService, dataProviderFactory, featuresProvider, featuresPreviewer, this);

            ChildErrorsChanged += FilesDataModelErrorsChanged;

            OpenFeaturesFileCommand = new FLCommand(p => { FeaturesDataModel.FeaturesFilePath = OpenFile(FeaturesFileSettings); });
        }

        private void FilesDataModelErrorsChanged(object sender, ChildErrorsChangedEventArgs e)
        {
            parentViewModel.OnChildErrorsChanged("OpenFeaturesViewModel", e.Errors);
        }

        public override void Initialize()
        {
            FeaturesDataModel = new FeaturesDataModel(modelService, dataProviderFactory, featuresProvider, featuresPreviewer, this);
            childViewModels.Clear();
            childViewModels.Add(FeaturesDataModel);
        }

        private string OpenFile(OpenFileDialogSettings settings)
        {
            bool? success = dialogService.ShowOpenFileDialog(this, settings);
            return success == true ? settings.FileName : string.Empty;
        }
    }
}