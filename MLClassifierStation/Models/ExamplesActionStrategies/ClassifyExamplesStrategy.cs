using MLCS.LearningAlgorithmPlugins;
using MLCS.Services;
using System;

namespace MLClassifierStation.Models.ExamplesActionStrategies
{
    public class ClassifyExamplesStrategy : IProcessExamplesStrategy
    {
        public void ProcessExamples(IModelService modelService, IEvaluation evaluation, string outputFolderPath)
        {
            throw new NotImplementedException();
        }
    }
}