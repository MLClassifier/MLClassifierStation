using MLClassifierStation.Common;
using MLCS.FileParser;
using MLCS.Services;
using MvvmDialogs;

namespace MLClassifierStation.ViewModels
{
    public class ExamplesViewModel : WizardElementViewModelBase
    {
        public OpenExamplesViewModel OpenExamplesViewModel { get; set; }

        public ExamplesViewModel(IDialogService dialogService, IModelService modelService, IDataProviderFactory dataProviderFactory,
            IExamplesProvider examplesProvider, IExamplesPreviewer examplesPreviewer, ValidatableBindableBase parentViewModel, ExampleType exampleType)
        {
            OpenExamplesViewModel = new OpenExamplesViewModel(dialogService, modelService, dataProviderFactory,
                examplesProvider, examplesPreviewer, this, exampleType);
            this.parentViewModel = parentViewModel;

            ChildErrorsChanged += ExamplesViewModelChildErrorsChanged;
        }

        private void ExamplesViewModelChildErrorsChanged(object sender, ChildErrorsChangedEventArgs e)
        {
            parentViewModel.OnChildErrorsChanged("ExamplesViewModel", e.Errors);
        }

        public override void Initialize()
        {
            OpenExamplesViewModel.Initialize();
            childViewModels.Clear();
            childViewModels.Add(OpenExamplesViewModel);
        }
    }
}