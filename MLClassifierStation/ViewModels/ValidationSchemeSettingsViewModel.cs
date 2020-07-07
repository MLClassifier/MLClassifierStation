using MLClassifierStation.Features;
using MLClassifierStation.Common;
using MLClassifierStation.Properties;
using MLCS.Entities.Model.Features;
using MLCS.LearningAlgorithmPlugins;
using MLCS.Services;
using Nito.Mvvm;
using PluginLoader;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;

namespace MLClassifierStation.ViewModels
{
    public class ValidationSchemeSettingsViewModel : SettingsViewModelBase
    {
        private IModelService modelService;

        public ObservableCollection<IParameterViewModel> ValidationSchemeParameters { get; set; }

        private NotifyTask<IEnumerable<IValidationScheme>> validationSchemePlugins;

        public NotifyTask<IEnumerable<IValidationScheme>> ValidationSchemePlugins
        {
            get => validationSchemePlugins;
            set => SetProperty(ref validationSchemePlugins, value);
        }

        private IValidationScheme selectedValidationScheme;

        [ItemSelected]
        public IValidationScheme SelectedValidationScheme
        {
            get => selectedValidationScheme;
            set
            {
                SetProperty(ref selectedValidationScheme, value);

                if (SelectedValidationScheme == null) return;

                IEnumerable<PropertyInfo> parameters = selectedValidationScheme.GetType().GetProperties().Where(
                        prop => Attribute.IsDefined(prop, typeof(ParameterAttribute)));
                ValidationSchemeParameters.Clear();
                childViewModels.Clear();
                foreach (var parameter in parameters)
                {
                    IntParameterViewModel parameterViewModel = new IntParameterViewModel(parameter.Name, this);
                    ValidationSchemeParameters.Add(parameterViewModel);
                    childViewModels.Add(parameterViewModel);
                }
                ValidationSchemeHasParameters = ValidationSchemeParameters.Any();

                modelService.ValidationScheme = selectedValidationScheme;
            }
        }

        private bool validationSchemeHasParameters;

        public bool ValidationSchemeHasParameters
        {
            get => validationSchemeHasParameters;
            set => SetProperty(ref validationSchemeHasParameters, value);
        }

        public ValidationSchemeSettingsViewModel(IModelService modelService, IPluginLoader<IValidationScheme> validationSchemePluginLoader,
            ValidatableBindableBase parentViewModel)
        {
            this.modelService = modelService;
            this.parentViewModel = parentViewModel;

            ValidationSchemePlugins = LoadValidationSchemePlugins(validationSchemePluginLoader);
            ValidationSchemeParameters = new ObservableCollection<IParameterViewModel>();

            ErrorsChanged += NotifyParentChildErrorsChanged;
            ChildErrorsChanged += ValidationSchemeSettingsViewModelChildErrorsChanged;
        }

        private void ValidationSchemeSettingsViewModelChildErrorsChanged(object sender, ChildErrorsChangedEventArgs e)
        {
            parentViewModel.OnChildErrorsChanged("ValidationSchemeSettingsViewModel", e.Errors);
        }

        private NotifyTask<IEnumerable<IValidationScheme>> LoadValidationSchemePlugins(IPluginLoader<IValidationScheme> validationSchemePluginLoader)
        {
            string validationSchemesPluginsPath =
                            Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDi‌​rectory, Settings.Default.ValidationSchemesPluginsPath));
            return NotifyTask.Create(validationSchemePluginLoader.LoadPluginsAsync(validationSchemesPluginsPath));
        }

        public void SetParameters()
        {
            SetParameterValues(SelectedValidationScheme, ValidationSchemeParameters);
        }

        public override void Initialize()
        {
            ValidationSchemeHasParameters = false;
            ValidationSchemeParameters.Clear();
            childViewModels.Clear();
        }        
    }
}