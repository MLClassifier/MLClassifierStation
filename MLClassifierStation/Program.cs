using MLClassifierStation.ViewModels;
using MLCS.Entities.Features;
using MLCS.Entities.Features.Default;
using MLCS.Entities.Features.Numerical;
using MLCS.Entities.Vectors;
using MLCS.Entities.Vectors.Default;
using MLCS.Entities.Values;
using MLCS.Entities.Values.Default;
using MLCS.FileParser;
using MLCS.FileParser.Default;
using MLCS.LearningAlgorithmPlugins;
using MLCS.LearningAlgorithmPlugins.Default;
using MLCS.Services;
using MLCS.Services.Default;
using MvvmDialogs;
using PluginLoader;
using PluginLoader.Default;
using SimpleInjector;
using System;
using TinyCsvParser.Mapping;
using TinyCsvParser.TypeConverter;

namespace MLClassifierStation
{
    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            var container = Bootstrap();

            // Any additional other configuration, e.g. of your desired MVVM toolkit.

            RunApplication(container);
        }

        private static Container Bootstrap()
        {
            // Create the container as usual.
            var container = new Container();

            container.Register<IDialogService>(() => new DialogService());
            container.Register(typeof(IEvaluation), typeof(Evaluation), Lifestyle.Singleton);
            container.Register(typeof(IValueFactory), typeof(ValueFactory), Lifestyle.Singleton);
            container.Register(typeof(IPluginLoader<>), typeof(PluginLoader<>), Lifestyle.Singleton);
            container.Register(typeof(ITypeConverterProvider), typeof(TypeConverterProvider), Lifestyle.Singleton);
            container.Register(typeof(IVectorFactory), typeof(VectorFactory), Lifestyle.Singleton);
            container.Register(typeof(IDataProviderFactory), typeof(DataProviderFactory), Lifestyle.Singleton);
            container.Register(typeof(ICsvMappingFactory), typeof(CsvMappingFactory), Lifestyle.Singleton);
            container.Register(typeof(INumericalFeatureFactory<>), typeof(NumericalFeatureFactory<>), Lifestyle.Singleton);
            container.Register(typeof(ICategorialFeatureFactory<>), typeof(CategorialFeatureFactory<>), Lifestyle.Singleton);
            container.Register(typeof(IFeatureFactory), typeof(FeatureFactory), Lifestyle.Singleton);
            container.Register(typeof(IFeaturesParser), typeof(FeaturesParser), Lifestyle.Singleton);
            container.Register(typeof(IFeaturesProvider), typeof(FeaturesProvider), Lifestyle.Singleton);
            container.Register(typeof(IFeaturesPreviewer), typeof(FeaturesPreviewer), Lifestyle.Singleton);
            container.Register(typeof(IExamplesParser), typeof(ExamplesParser), Lifestyle.Singleton);
            container.Register(typeof(IExamplesProvider), typeof(ExamplesProvider), Lifestyle.Singleton);
            container.Register(typeof(IExamplesPreviewer), typeof(ExamplesPreviewer), Lifestyle.Singleton);
            container.Register(typeof(IModelService), typeof(ModelService), Lifestyle.Singleton);
            container.Register(typeof(IClassifyService), typeof(ClassifyService), Lifestyle.Singleton);

            // Register your windows and view models:
            container.Register<MainWindow>();
            container.Register<MainWindowViewModel>();

            container.Verify();

            return container;
        }

        private static void RunApplication(Container container)
        {
            try
            {
                var app = new App();
                var mainWindow = container.GetInstance<MainWindow>();
                app.Run(mainWindow);
            }
            catch (Exception ex)
            {
                //Log the exception and exit
            }
        }
    }
}