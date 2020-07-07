using MLClassifierStation.Features;
using MLClassifierStation.Common;
using System.ComponentModel.DataAnnotations;

namespace MLClassifierStation.ViewModels
{
    public class IntParameterViewModel : ValidatableBindableBase, IParameterViewModel
    {
        private string parameter;

        [Required(ErrorMessageResourceName = "RequiredParameter", ErrorMessageResourceType = typeof(Properties.Resources))]
        [IntValue]
        public string Parameter
        {
            get => parameter;
            set
            {
                UntypedParameter = value;
                SetProperty(ref parameter, value);
            }
        }

        private string name;

        public string Name
        {
            get => name;
            private set => SetProperty(ref name, value);
        }

        public object UntypedParameter { get; set; }

        public IntParameterViewModel(string name, ValidatableBindableBase parentViewModel)
        {
            Name = name;
            this.parentViewModel = parentViewModel;

            ErrorsChanged += NotifyParentChildErrorsChanged;
        }

        public void Dispose()
        {
        }

        public override string ToString()
        {
            return Name;
        }
    }
}