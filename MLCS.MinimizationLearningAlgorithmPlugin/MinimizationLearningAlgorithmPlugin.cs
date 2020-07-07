using MLCS.Entities.Vectors;
using MLCS.Entities.Model;
using MLCS.Entities.Model.Features;
using MLCS.LearningAlgorithmPlugins;
using MLCS.LearningAlgorithmPlugins.Default;
using MLCS.LearningStatistics.Entities;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using MinimizationAlgorithm = MLCS.MinimizationLearningAlgorithm.MinimizationLearningAlgorithm;

namespace MLCS.MinimizationLearningAlgorithmPlugin
{
    [Export(typeof(ILearningAlgorithm))]
    public class MinimizationLearningAlgorithmPlugin : BaseLearningAlgorithm
    {
        [LearningParameter]
        public int TuningFactor { get; set; }

        protected override IModel OnGenerateModel(IEnumerable<IVector> trainingExamples,
            IEnumerable<IVector> validationExamples, IMetric metric = null)
        {
            dynamic typedTrainingExamples = trainingExamples;
            dynamic typedValidationExamples = validationExamples;

            MinimizationAlgorithm algorithm = new MinimizationAlgorithm();
            return algorithm.GenerateModel(trainingExamples, validationExamples, metric);
        }
    }
}