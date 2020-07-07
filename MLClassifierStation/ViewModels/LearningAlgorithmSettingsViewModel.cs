using MLClassifierStation.Features;
using MLClassifierStation.Common;
using MLClassifierStation.Properties;
using MLCS.Entities.Model.Features;
using MLCS.LearningAlgorithmPlugins;
using MLCS.LearningStatistics;
using MLCS.LearningStatistics.Entities;
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
    public class LearningAlgorithmSettingsViewModel : SettingsViewModelBase
    {
        private readonly IModelService modelService;

        public ObservableCollection<IParameterViewModel> LearningAlgorithmParameters { get; set; }

        private NotifyTask<IEnumerable<ILearningAlgorithm>> learningAlgorithmPlugins;

        public NotifyTask<IEnumerable<ILearningAlgorithm>> LearningAlgorithmPlugins
        {
            get => learningAlgorithmPlugins;
            set => SetProperty(ref learningAlgorithmPlugins, value);
        }

        private ObservableCollection<IMetric> metrics;

        public ObservableCollection<IMetric> Metrics
        {
            get => metrics;
            set => SetProperty(ref metrics, value);
        }

        private ILearningAlgorithm selectedLearningAlgorithm;

        [ItemSelected]
        public ILearningAlgorithm SelectedLearningAlgorithm
        {
            get => selectedLearningAlgorithm;
            set
            {
                SetProperty(ref selectedLearningAlgorithm, value);

                if (SelectedLearningAlgorithm == null) return;

                IEnumerable<PropertyInfo> parameters = (selectedLearningAlgorithm.GetType()).GetProperties().Where(
                    prop => Attribute.IsDefined(prop, typeof(ParameterAttribute)));
                LearningAlgorithmParameters.Clear();
                childViewModels.Clear();
                foreach (var parameter in parameters)
                {
                    IntParameterViewModel parameterViewModel = new IntParameterViewModel(parameter.Name, this);
                    LearningAlgorithmParameters.Add(parameterViewModel);
                    childViewModels.Add(parameterViewModel);
                }
                LearningAlgorithmHasParameters = LearningAlgorithmParameters.Any();

                IEnumerable<PropertyInfo> learningParameters = (selectedLearningAlgorithm.GetType()).GetProperties().Where(
                    prop => Attribute.IsDefined(prop, typeof(LearningParameterAttribute)));
                LearningAlgorithmHasLearningParameters = learningParameters.Any();

                if (LearningAlgorithmHasLearningParameters)
                    SelectedMetric = Metrics.FirstOrDefault();

                modelService.LearningAlgorithm = selectedLearningAlgorithm;
            }
        }

        private bool learningAlgorithmHasParameters;

        public bool LearningAlgorithmHasParameters
        {
            get => learningAlgorithmHasParameters;
            set => SetProperty(ref learningAlgorithmHasParameters, value);
        }

        private bool learningAlgorithmHasLearningParameters;

        public bool LearningAlgorithmHasLearningParameters
        {
            get => learningAlgorithmHasLearningParameters; 
            set => SetProperty(ref learningAlgorithmHasLearningParameters, value); 
        }

        private IMetric selectedMetric;

        public IMetric SelectedMetric
        {
            get => selectedMetric;
            set
            {
                SetProperty(ref selectedMetric, value);
                modelService.Metric = selectedMetric;
            }
        }

        public LearningAlgorithmSettingsViewModel(IModelService modelService, IPluginLoader<ILearningAlgorithm> learningAlgorithmPluginLoader,
            ValidatableBindableBase parentViewModel)
        {
            this.modelService = modelService;
            this.parentViewModel = parentViewModel;

            LearningAlgorithmPlugins = LoadLearningAlgorithmPlugins(learningAlgorithmPluginLoader);
            LearningAlgorithmParameters = new ObservableCollection<IParameterViewModel>();
            Metrics = new ObservableCollection<IMetric>(Utils.GetMetrics());

            ErrorsChanged += NotifyParentChildErrorsChanged;
            ChildErrorsChanged += LearningAlgorithmSettingsViewModelChildErrorsChanged;
        }

        private void LearningAlgorithmSettingsViewModelChildErrorsChanged(object sender, ChildErrorsChangedEventArgs e)
        {
            parentViewModel.OnChildErrorsChanged("LearningAlgorithmSettingsViewModel", e.Errors);
        }

        private NotifyTask<IEnumerable<ILearningAlgorithm>> LoadLearningAlgorithmPlugins(IPluginLoader<ILearningAlgorithm> learningAlgorithmPluginLoader)
        {
            string learningAlgorithmsPluginsPath =
                            Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDi‌​rectory, Settings.Default.LearningAlgorithmsPluginsPath));
            return NotifyTask.Create(learningAlgorithmPluginLoader.LoadPluginsAsync(learningAlgorithmsPluginsPath));
        }

        public void SetParameters()
        {
            SetParameterValues(SelectedLearningAlgorithm, LearningAlgorithmParameters);
        }

        public override void Initialize()
        {
            LearningAlgorithmHasParameters = false;
            LearningAlgorithmHasLearningParameters = false;
            LearningAlgorithmParameters.Clear();
            childViewModels.Clear();
        }
    }
}