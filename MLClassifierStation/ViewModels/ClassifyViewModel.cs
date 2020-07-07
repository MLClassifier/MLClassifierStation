using MLClassifierStation.Common;
using MLCS.Services;
using MvvmDialogs;

namespace MLClassifierStation.ViewModels
{
    public class ClassifyViewModel : BindableBase
    {
        public ClassifyViewModel(IDialogService dialogService, IClassifyService classifyService)
        {
            //LearningInputFilesViewModel = new LearningInputFilesViewModel(dialogService, classifyService);
        }
    }
}