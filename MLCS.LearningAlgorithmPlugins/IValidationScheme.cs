using MLCS.Entities.Vectors;
using MLCS.LearningStatistics.Entities;
using System.Collections.Generic;

namespace MLCS.LearningAlgorithmPlugins
{
    public interface IValidationScheme
    {
        IValidationResult ValidateModel(IEnumerable<IVector> examples,
            ILearningAlgorithm learningAlgorithm, IMetric metric);
    }
}