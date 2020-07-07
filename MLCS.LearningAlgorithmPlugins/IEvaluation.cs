using MLCS.Entities.Vectors;
using MLCS.Entities.Model;
using MLCS.LearningStatistics.Entities;
using System.Collections.Generic;

namespace MLCS.LearningAlgorithmPlugins
{
    public interface IEvaluation
    {
        IModel Model { get; }
        IStatisticsDictionary Statistics { get; }

        void EvaluateModel(IEnumerable<IVector> examples,
            IValidationScheme validationScheme,
            ILearningAlgorithm learningAlgorithm,
            IMetric metric = null);
    }
}