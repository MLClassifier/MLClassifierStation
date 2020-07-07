using MLClassifierStation.Common;
using MLClassifierStation.Providers;
using MLClassifierStation.Views;
using MLCS.FileParser;
using MLCS.LearningAlgorithmPlugins;
using MLCS.Services;
using MvvmDialogs;
using PluginLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using Xceed.Wpf.Toolkit;
using Xceed.Wpf.Toolkit.Core;

namespace MLClassifierStation.ViewModels
{
    public class LearnNewModelViewModel : WizardElementViewModelBase
    {
        public enum NewModelWizardPages
        {
            Features,
            LearningExamples,
            LearningAlgorithmSettings,
            ValidationSchemeSettings,
            LearningOutputFolder,
            LearningResult,
            ClassificationExamples,
            ClassificationOutputFolder,
            ClassificationResult
        }

        private string nextButtonDefaultContent = "Next >";
        private string cancelButtonDefaultContent = "Cancel";

        private MainWindowViewModel mainViewModel;
        private IModelService modelService;

        private IList<WizardPage> wizardPages;

        public IList<WizardPage> WizardPages
        {
            get => wizardPages;
            set => SetProperty(ref wizardPages, value);
        }

        private WizardPage currentPage;

        public WizardPage CurrentPage
        {
            get => currentPage; 
            set => SetProperty(ref currentPage, value);
        }

        public FeaturesViewModel FeaturesViewModel { get; set; }
        public ExamplesViewModel LearningExamplesViewModel { get; set; }
        public LearningAlgorithmSettingsViewModel LearningAlgorithmSettingsViewModel { get; set; }
        public ValidationSchemeSettingsViewModel ValidationSchemeSettingsViewModel { get; set; }
        public OutputFolderViewModel LearningOutputFolderViewModel { get; set; }
        public LearningResultViewModel LearningResultViewModel { get; set; }
        public ExamplesViewModel ClassificationExamplesViewModel { get; set; }
        public OutputFolderViewModel ClassificationOutputFolderViewModel { get; set; }
        public ClassificationResultViewModel ClassificationResultViewModel { get; set; }

        private UserControl currentErrorView;

        public UserControl CurrentErrorView
        {
            get => currentErrorView;
            set => SetProperty(ref currentErrorView, value);
        }

        private string nextButtonContent;

        public string NextButtonContent
        {
            get => nextButtonContent;
            set => SetProperty(ref nextButtonContent, value);
        }

        private string cancelButtonContent;

        public string CancelButtonContent
        {
            get => cancelButtonContent;
            set => SetProperty(ref cancelButtonContent, value);
        }

        private bool canProceed;

        public bool CanProceed
        {
            get => canProceed;
            set => SetProperty(ref canProceed, value);
        }

        public FLCommand PreviousStage { get; set; }
        public FLCommand Finish { get; set; }
        public FLCommand Cancel { get; set; }

        public LearnNewModelViewModel(IDialogService dialogService, IModelService modelService, IDataProviderFactory dataProviderFactory,
            IFeaturesProvider featuresProvider, IFeaturesPreviewer featuresPreviewer, IExamplesProvider examplesProvider, IExamplesPreviewer examplesPreviewer,
            IPluginLoader<ILearningAlgorithm> learningAlgorithmPluginLoader, IPluginLoader<IValidationScheme> validationSchemePluginLoader, IEvaluation evaluation,
            MainWindowViewModel mainViewModel)
        {
            FeaturesViewModel = new FeaturesViewModel(dialogService, modelService, dataProviderFactory,
                featuresProvider, featuresPreviewer, this);
            LearningExamplesViewModel = new ExamplesViewModel(dialogService, modelService, dataProviderFactory,
                examplesProvider, examplesPreviewer, this, ExampleType.Learning);
            LearningAlgorithmSettingsViewModel = new LearningAlgorithmSettingsViewModel(modelService, learningAlgorithmPluginLoader, this);
            ValidationSchemeSettingsViewModel = new ValidationSchemeSettingsViewModel(modelService, validationSchemePluginLoader, this);
            LearningOutputFolderViewModel = new OutputFolderViewModel(dialogService, modelService, evaluation, this, ExampleType.Learning);
            LearningResultViewModel = new LearningResultViewModel(modelService);
            ClassificationExamplesViewModel = new ExamplesViewModel(dialogService, modelService, dataProviderFactory,
                examplesProvider, examplesPreviewer, this, ExampleType.Classification);
            ClassificationOutputFolderViewModel = new OutputFolderViewModel(dialogService, modelService, evaluation, this, ExampleType.Classification);
            ClassificationResultViewModel = new ClassificationResultViewModel(modelService);

            this.modelService = modelService;
            this.mainViewModel = mainViewModel;

            ChildErrorsChanged += LearnNewModelViewModelChildErrorsChanged;

            WizardPages = InitializeWizardPages(dialogService, modelService, dataProviderFactory, featuresProvider, featuresPreviewer,
                examplesProvider, examplesPreviewer, learningAlgorithmPluginLoader, validationSchemePluginLoader);

            NextButtonContent = nextButtonDefaultContent;
            CancelButtonContent = cancelButtonDefaultContent;

            PreviousStage = new FLCommand(p => OnPreviousStage());
            Finish = new FLCommand(p => OnFinish());
            Cancel = new FLCommand(p => OnFinish());

            SetValidationContextServices(dataProviderFactory, featuresProvider);
        }

        private void SetValidationContextServices(IDataProviderFactory dataProviderFactory, IFeaturesProvider featuresProvider)
        {
            ValidationServiceProvider serviceProvider = new ValidationServiceProvider();
            serviceProvider.AddService(typeof(IFeaturesProvider), featuresProvider);
            serviceProvider.AddService(typeof(IDataProviderFactory), dataProviderFactory);
            ValidationContext.InitializeServiceProvider(serviceProvider.GetService);
        }

        private void LearnNewModelViewModelChildErrorsChanged(object sender, ChildErrorsChangedEventArgs e)
        {
            OnErrorsChanged("LearningInputFilesViewModel");
        }

        public void NextStageMethod(object sender, CancelRoutedEventArgs args)
        {
            Validate();
            if (IsValid)
                OnNextStage();
            else
            {
                CurrentErrorView = (UserControl)CurrentPage.Content;
                args.Cancel = true;
            }
        }

        public override void Validate()
        {
            base.Validate();
            (CurrentPage.DataContext as ValidatableBindableBase).Validate();
        }

        public override void Initialize()
        {
            SetCurrentPage(NewModelWizardPages.Features);
            CanProceed = true;
            modelService.Clear();
            NextButtonContent = nextButtonDefaultContent;
            foreach (WizardPage wizardPage in WizardPages)
                ((WizardElementViewModelBase)wizardPage.DataContext).Initialize();
        }

        private void SetCurrentPage(NewModelWizardPages wizardPage)
        {
            CurrentPage = WizardPages.FirstOrDefault(wp => wp.Name == wizardPage.ToString());
        }

        private void OnNextStage()
        {
            NewModelWizardPages currentPage = (NewModelWizardPages)Enum.Parse(typeof(NewModelWizardPages), CurrentPage.Name);
            switch (currentPage)
            {
                case NewModelWizardPages.Features:
                    SetCurrentPage(NewModelWizardPages.Features);
                    break;

                case NewModelWizardPages.LearningExamples:
                    SetCurrentPage(NewModelWizardPages.LearningExamples);
                    break;

                case NewModelWizardPages.LearningAlgorithmSettings:
                    SetCurrentPage(NewModelWizardPages.LearningAlgorithmSettings);
                    LearningAlgorithmSettingsViewModel.SetParameters();
                    break;

                case NewModelWizardPages.ValidationSchemeSettings:
                    SetCurrentPage(NewModelWizardPages.ValidationSchemeSettings);
                    NextButtonContent = "Learn model";
                    ValidationSchemeSettingsViewModel.SetParameters();
                    break;

                case NewModelWizardPages.LearningOutputFolder:
                    SetCurrentPage(NewModelWizardPages.LearningOutputFolder);
                    NextButtonContent = "Test model";
                    CancelButtonContent = "Finish";
                    LearningOutputFolderViewModel.ProcessExamples();
                    LearningResultViewModel.SetInformation();
                    break;

                case NewModelWizardPages.LearningResult:
                    SetCurrentPage(NewModelWizardPages.LearningResult);
                    NextButtonContent = nextButtonDefaultContent;
                    CancelButtonContent = cancelButtonDefaultContent;
                    break;

                case NewModelWizardPages.ClassificationExamples:
                    SetCurrentPage(NewModelWizardPages.ClassificationExamples);
                    NextButtonContent = "Classify";
                    break;

                case NewModelWizardPages.ClassificationOutputFolder:
                    SetCurrentPage(NewModelWizardPages.ClassificationOutputFolder);
                    ClassificationOutputFolderViewModel.ProcessExamples();
                    ClassificationResultViewModel.SetInformation();
                    CanProceed = false;
                    break;
            }
        }

        private void OnPreviousStage()
        {
            NewModelWizardPages currentPage = (NewModelWizardPages)Enum.Parse(typeof(NewModelWizardPages), CurrentPage.Name);
            switch (currentPage)
            {
                case NewModelWizardPages.LearningAlgorithmSettings:
                    SetCurrentPage(NewModelWizardPages.LearningAlgorithmSettings);
                    break;

                case NewModelWizardPages.ValidationSchemeSettings:
                    SetCurrentPage(NewModelWizardPages.ValidationSchemeSettings);
                    break;

                case NewModelWizardPages.LearningOutputFolder:
                    SetCurrentPage(NewModelWizardPages.LearningOutputFolder);
                    NextButtonContent = nextButtonDefaultContent;
                    break;

                case NewModelWizardPages.LearningResult:
                    SetCurrentPage(NewModelWizardPages.LearningResult);
                    NextButtonContent = "Learn model";
                    CancelButtonContent = cancelButtonDefaultContent;
                    break;

                case NewModelWizardPages.ClassificationExamples:
                    SetCurrentPage(NewModelWizardPages.ClassificationExamples);
                    NextButtonContent = "Test model";
                    CancelButtonContent = "Finish";
                    break;

                case NewModelWizardPages.ClassificationOutputFolder:
                    SetCurrentPage(NewModelWizardPages.ClassificationOutputFolder);
                    NextButtonContent = nextButtonDefaultContent;
                    CanProceed = true;
                    break;
            }
        }

        private void OnFinish()
        {
            mainViewModel.IsLearnNewModelVisible = false;
            //Clear();
        }

        private IList<WizardPage> InitializeWizardPages(IDialogService dialogService, IModelService modelService, IDataProviderFactory dataProviderFactory,
            IFeaturesProvider featuresParser, IFeaturesPreviewer featuresPreviewer, IExamplesProvider examplesProvider, IExamplesPreviewer examplesPreviewer,
            IPluginLoader<ILearningAlgorithm> learningAlgorithmPluginLoader, IPluginLoader<IValidationScheme> validationSchemePluginLoader)
        {
            WizardPages = new List<WizardPage>();
            WizardPages.Add(new WizardPage()
            {
                Name = NewModelWizardPages.Features.ToString(),
                Title = "Features file",
                BackButtonVisibility = WizardPageButtonVisibility.Hidden,
                DataContext = FeaturesViewModel,
                Content = new FeaturesView()
            });
            WizardPages.Add(new WizardPage()
            {
                Name = NewModelWizardPages.LearningExamples.ToString(),
                Title = "Examples file",
                DataContext = LearningExamplesViewModel,
                Content = new ExamplesView()
            });
            WizardPages.Add(new WizardPage()
            {
                Name = NewModelWizardPages.LearningAlgorithmSettings.ToString(),
                Title = "Learning algorithm settings",
                DataContext = LearningAlgorithmSettingsViewModel,
                Content = new LearningAlgorithmSettingsView()
            });
            WizardPages.Add(new WizardPage()
            {
                Name = NewModelWizardPages.ValidationSchemeSettings.ToString(),
                Title = "Validation scheme settings",
                DataContext = ValidationSchemeSettingsViewModel,
                Content = new ValidationSchemeSettingsView()
            });
            WizardPages.Add(new WizardPage()
            {
                Name = NewModelWizardPages.LearningOutputFolder.ToString(),
                Title = "Learning output folder",
                DataContext = LearningOutputFolderViewModel,
                Content = new OutputFolderView()
            });
            WizardPages.Add(new WizardPage()
            {
                Name = NewModelWizardPages.LearningResult.ToString(),
                Title = "Learning result",
                DataContext = LearningResultViewModel,
                Content = new LearningResultView()
            });
            WizardPages.Add(new WizardPage()
            {
                Name = NewModelWizardPages.ClassificationExamples.ToString(),
                Title = "Classification examples",
                DataContext = ClassificationExamplesViewModel,
                Content = new ExamplesView()
            });
            WizardPages.Add(new WizardPage()
            {
                Name = NewModelWizardPages.ClassificationOutputFolder.ToString(),
                Title = "Classification output folder",
                DataContext = ClassificationOutputFolderViewModel,
                Content = new OutputFolderView()
            });
            WizardPages.Add(new WizardPage()
            {
                Name = NewModelWizardPages.ClassificationResult.ToString(),
                Title = "Classification result",
                DataContext = ClassificationResultViewModel,
                Content = new ClassificationResultView(),
                NextButtonVisibility = WizardPageButtonVisibility.Hidden,
                CancelButtonVisibility = WizardPageButtonVisibility.Hidden,
                FinishButtonVisibility = WizardPageButtonVisibility.Visible,
                CanFinish = true
            });
            return wizardPages;
        }
    }
}