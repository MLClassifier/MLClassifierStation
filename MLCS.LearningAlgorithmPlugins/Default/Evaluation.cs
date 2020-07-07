using MLCS.Entities.Vectors;
using MLCS.Entities.Model;
using MLCS.LearningStatistics.Entities;
using System.Collections.Generic;

namespace MLCS.LearningAlgorithmPlugins.Default
{
    public class Evaluation : IEvaluation
    {
        public IModel Model { get; private set; }
        public IStatisticsDictionary Statistics { get; private set; }

        public void EvaluateModel(IEnumerable<IVector> examples,
            IValidationScheme validationScheme, ILearningAlgorithm learningAlgorithm, IMetric metric = null)
        {
            IValidationResult validationResult = validationScheme.ValidateModel(examples, learningAlgorithm, metric);
            Model = validationResult.Model;
            Statistics = validationResult.Statistics;
        }
    }
}