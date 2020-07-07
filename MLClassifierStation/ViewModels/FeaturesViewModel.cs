using MLClassifierStation.Common;
using MLCS.FileParser;
using MLCS.Services;
using MvvmDialogs;

namespace MLClassifierStation.ViewModels
{
    public class FeaturesViewModel : WizardElementViewModelBase
    {
        public OpenFeaturesViewModel OpenFeaturesViewModel { get; set; }

        public FeaturesViewModel(IDialogService dialogService, IModelService modelService, IDataProviderFactory dataProviderFactory,
            IFeaturesProvider featuresProvider, IFeaturesPreviewer featuresPreviewer, ValidatableBindableBase parentViewModel)
        {
            OpenFeaturesViewModel = new OpenFeaturesViewModel(dialogService, modelService, dataProviderFactory,
                featuresProvider, featuresPreviewer, this);
            this.parentViewModel = parentViewModel;

            ChildErrorsChanged += FeaturesViewModelChildErrorsChanged;
        }

        private void FeaturesViewModelChildErrorsChanged(object sender, ChildErrorsChangedEventArgs e)
        {
            parentViewModel.OnChildErrorsChanged("FeaturesViewModel", e.Errors);
        }

        public override void Initialize()
        {
            OpenFeaturesViewModel.Initialize();
            childViewModels.Clear();
            childViewModels.Add(OpenFeaturesViewModel);
        }
    }
}