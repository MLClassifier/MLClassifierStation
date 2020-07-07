using MLClassifierStation.Common;
using MLCS.FileParser;
using MLCS.Services;
using MvvmDialogs;

namespace MLClassifierStation.ViewModels
{
    public class LoadModelViewModel : ValidatableBindableBase
    {
        // public OpenModelViewModel OpenModelViewModel { get; set; }

        public LoadModelViewModel(IDialogService dialogService, IModelService modelService, IDataProviderFactory dataProviderFactory,
            IFeaturesProvider featuresProvider, IFeaturesPreviewer featuresPreviewer, IExamplesProvider examplesProvider, IExamplesPreviewer examplesPreviewer)
        {
            //OpenModelViewModel = new OpenModelViewModel(dialogService, modelService, dataProviderFactory,
            //    featuresProvider, featuresPreviewer, examplesProvider, examplesPreviewer, this);
        }
    }
}