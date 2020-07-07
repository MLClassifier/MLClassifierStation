using MLCS.Entities;
using MLCS.Entities.Features;
using MLCS.Entities.Vectors;
using MLCS.Entities.Model;
using MLCS.Entities.Model.Features;
using MLCS.LearningAlgorithmPlugins;
using MLCS.LearningAlgorithmPlugins.Default;
using MLCS.LearningStatistics;
using MLCS.LearningStatistics.Entities;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;

namespace MLCS.HoldoutValidationSchemePlugin
{
    [Export(typeof(IValidationScheme))]
    public class HoldoutValidationScheme : IValidationScheme
    {
        [Parameter]
        public int HoldoutPercent { get; set; }

        public IValidationResult ValidateModel(IEnumerable<IVector> examples,
            ILearningAlgorithm learningAlgorithm, IMetric metric = null)
        {
            (IEnumerable<IVector> trainingExamples, IEnumerable<IVector> testExamples) =
                examples.PartitionExamples(HoldoutPercent);
            (IEnumerable<IVector> trainingTrainingExamples, IEnumerable<IVector> validationExamples) =
                trainingExamples.PartitionExamples(HoldoutPercent);

            IModel model = learningAlgorithm.GenerateModel(trainingTrainingExamples, validationExamples, metric);
            IFeature classFeature = examples.FirstOrDefault().GetClassFeature();
            IStatisticsDictionary statistics = Utils.CalculateAll(model, testExamples, classFeature);

            return new ValidationResult(model, statistics, string.Empty);
        }
    }
}