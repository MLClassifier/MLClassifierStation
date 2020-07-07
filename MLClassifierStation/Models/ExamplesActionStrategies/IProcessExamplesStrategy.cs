using MLCS.LearningAlgorithmPlugins;
using MLCS.Services;

namespace MLClassifierStation.Models.ExamplesActionStrategies
{
    public interface IProcessExamplesStrategy
    {
        void ProcessExamples(IModelService modelService, IEvaluation evaluation, string outputFolderPath);
    }
}