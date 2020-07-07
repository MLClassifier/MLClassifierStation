using MLClassifierStation.Common;
using MLCS.Services;

namespace MLClassifierStation.ViewModels
{
    public class ClassificationResultViewModel : WizardElementViewModelBase
    {
        private readonly IModelService modelService;

        private string resultInformation;

        public string ResultInformation
        {
            get => resultInformation;
            set => SetProperty(ref resultInformation, value);
        }

        public ClassificationResultViewModel(IModelService modelService)
        {
            this.modelService = modelService;
        }

        public void SetInformation()
        {
            resultInformation = modelService.ClassificationResultInformation;
        }
    }
}