using MLCS.Entities.Vectors;
using MLCS.Entities.Model;
using MLCS.LearningStatistics.Entities;
using System.Collections.Generic;

namespace MLCS.LearningAlgorithmPlugins
{
    public interface ILearningAlgorithm
    {
        IModel GenerateModel(IEnumerable<IVector> trainingExamples,
            IEnumerable<IVector> validationExamples, IMetric metric = null);
    }
}