using MLClassifierStation.Common;
using MLCS.Services;

namespace MLClassifierStation.ViewModels
{
    public class LearningResultViewModel : WizardElementViewModelBase
    {
        private readonly IModelService modelService;

        private string modelInformation;

        public string ModelInformation
        {
            get => modelInformation;
            set => SetProperty(ref modelInformation, value); 
        }

        private string statisticsInformation;

        public string StatisticsInformation
        {
            get => statisticsInformation;
            set => SetProperty(ref statisticsInformation, value);
        }

        public LearningResultViewModel(IModelService modelService)
        {
            this.modelService = modelService;
        }

        public void SetInformation()
        {
            ModelInformation = modelService.ModelInformation;
            StatisticsInformation = modelService.StatisticsInformation;
        }

        public override void Initialize()
        {
            ModelInformation = string.Empty;
            StatisticsInformation = string.Empty;
        }
    }
}