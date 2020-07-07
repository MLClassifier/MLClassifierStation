using MLCS.LearningAlgorithmPlugins;
using MLCS.Services;

namespace MLClassifierStation.Models.ExamplesActionStrategies
{
    public class LearnExamplesStrategy : IProcessExamplesStrategy
    {
        private const string ModelFileName = "model.txt";
        private const string StatisticsFileName = "statistics.txt";

        public void ProcessExamples(IModelService modelService, IEvaluation evaluation, string outputFolderPath)
        {
            evaluation.EvaluateModel(modelService.LearningExamples, modelService.ValidationScheme, modelService.LearningAlgorithm, modelService.Metric);

            string modelFilePathName = string.Format("{0}//{1}", outputFolderPath, ModelFileName);
            evaluation.Model.Export(modelFilePathName);
            string statisticsFilePathName = string.Format("{0}//{1}", outputFolderPath, StatisticsFileName);
            evaluation.Statistics.Export(statisticsFilePathName);

            modelService.ModelInformation = evaluation.Model.Preview();
            modelService.StatisticsInformation = evaluation.Statistics.Preview();
        }
    }
}