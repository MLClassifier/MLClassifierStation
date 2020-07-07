using MLClassifierStation.Common;
using MLCS.FileParser;
using MLCS.LearningAlgorithmPlugins;
using MLCS.Services;
using MvvmDialogs;
using PluginLoader;
using System.Windows.Input;

namespace MLClassifierStation.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private IDialogService dialogService;
        private IModelService modelService;
        private IDataProviderFactory dataProviderFactory;
        private IFeaturesProvider featuresProvider;
        private IFeaturesPreviewer featuresPreviewer;
        private IExamplesProvider examplesProvider;
        private IExamplesPreviewer examplesPreviewer;
        private IPluginLoader<ILearningAlgorithm> learningAlgorithmPluginLoader;
        private IPluginLoader<IValidationScheme> validationSchemePluginLoader;
        private IEvaluation evaluation;

        private bool isClassifyButtonVisible;

        public bool IsClassifyButtonVisible
        {
            get => isClassifyButtonVisible;
            set => SetProperty(ref isClassifyButtonVisible, value);
        }

        private bool isLearnNewModelVisible;

        public bool IsLearnNewModelVisible
        {
            get => isLearnNewModelVisible;
            set => SetProperty(ref isLearnNewModelVisible, value);
        }

        private bool isLoadModelVisible;

        public bool IsLoadModelVisible
        {
            get => isLoadModelVisible;
            set => SetProperty(ref isLoadModelVisible, value);
        }

        private bool isClassifyVisible;

        public bool IsClassifyVisible
        {
            get => isClassifyVisible;
            set => SetProperty(ref isClassifyVisible, value);
        }

        private WizardElementViewModelBase learnNewModelViewModel;

        public WizardElementViewModelBase LearnNewModelViewModel
        {
            get => learnNewModelViewModel;
            set => SetProperty(ref learnNewModelViewModel, value);
        }

        public BindableBase LoadModelViewModel { get; set; }
        public BindableBase ClassifyViewModel { get; set; }

        public ICommand LearnNewModelCommand { get; set; }
        public ICommand LoadModelCommand { get; set; }
        public ICommand ClassifyCommand { get; set; }
        public ICommand CloseWindowCommand { get; set; }

        public MainWindowViewModel(IDialogService dialogService, IModelService modelService,
            IClassifyService classifyService, IDataProviderFactory dataProviderFactory,
            IFeaturesProvider featuresProvider, IFeaturesPreviewer featuresPreviewer,
            IExamplesProvider examplesProvider, IExamplesPreviewer examplesPreviewer,
            IPluginLoader<ILearningAlgorithm> learningAlgorithmPluginLoader,
            IPluginLoader<IValidationScheme> validationSchemePluginLoader,
            IEvaluation evaluation)
        {
            this.dialogService = dialogService;
            this.modelService = modelService;
            this.dataProviderFactory = dataProviderFactory;
            this.featuresProvider = featuresProvider;
            this.featuresPreviewer = featuresPreviewer;
            this.examplesProvider = examplesProvider;
            this.examplesPreviewer = examplesPreviewer;
            this.learningAlgorithmPluginLoader = learningAlgorithmPluginLoader;
            this.validationSchemePluginLoader = validationSchemePluginLoader;
            this.evaluation = evaluation;

            LoadModelViewModel = new LoadModelViewModel(dialogService, modelService, dataProviderFactory,
                featuresProvider, featuresPreviewer, examplesProvider, examplesPreviewer);
            ClassifyViewModel = new ClassifyViewModel(dialogService, classifyService);

            IsLearnNewModelVisible = false;
            IsLoadModelVisible = false;

            LearnNewModelCommand = new FLCommand(p => LearnNewModel());
            LoadModelCommand = new FLCommand(p => LoadModel());
            ClassifyCommand = new FLCommand(p => Classify());
            CloseWindowCommand = new FLCommand(p => CloseWindow());
        }

        private void LearnNewModel()
        {
            LearnNewModelViewModel = new LearnNewModelViewModel(dialogService, modelService, dataProviderFactory,
                featuresProvider, featuresPreviewer, examplesProvider, examplesPreviewer,
                learningAlgorithmPluginLoader, validationSchemePluginLoader, evaluation, this);
            LearnNewModelViewModel.Initialize();
            IsLearnNewModelVisible = true;
            IsLoadModelVisible = false;
            IsClassifyVisible = false;
            IsClassifyButtonVisible = false;
        }

        private void LoadModel()
        {
            IsLearnNewModelVisible = false;
            IsLoadModelVisible = true;
            IsClassifyVisible = false;
            IsClassifyButtonVisible = false;
        }

        private void Classify()
        {
            IsLearnNewModelVisible = false;
            IsLoadModelVisible = false;
            IsClassifyVisible = true;
            IsClassifyButtonVisible = false;
        }

        private void CloseWindow()
        {
            // dispose
        }
    }
}