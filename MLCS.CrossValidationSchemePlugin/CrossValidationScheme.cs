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

namespace MLCS.CrossValidationSchemePlugin
{
    [Export(typeof(IValidationScheme))]
    public class CrossValidationScheme : IValidationScheme
    {
        [Parameter]
        public int FoldsCount { get; set; }

        public IValidationResult ValidateModel(IEnumerable<IVector> examples,
            ILearningAlgorithm learningAlgorithm, IMetric metric = null)
        {
            IEnumerable<IEnumerable<IVector>> folds = examples.PartitionExamplesInFolds(FoldsCount);
            IFeature classFeature = examples.FirstOrDefault().GetClassFeature();

            IList<IModel> foldModels = new List<IModel>(FoldsCount);
            IList<IStatisticsDictionary> foldStatistics = new List<IStatisticsDictionary>(FoldsCount);
            foreach (IEnumerable<IVector> fold in folds)
            {
                IEnumerable<IVector> trainingExamples = examples.Where(e => !fold.Contains(e));
                (IEnumerable<IVector> trainingTrainingExamples, IEnumerable<IVector> trainingValidationExamples) =
                    trainingExamples.PartitionExamples(100 / FoldsCount);

                IModel trainingModel = learningAlgorithm.GenerateModel(trainingTrainingExamples, trainingValidationExamples);
                IStatisticsDictionary statistics = Utils.CalculateAll(trainingModel, fold, classFeature);

                IModel model = learningAlgorithm.GenerateModel(trainingExamples, fold);

                foldModels.Add(model);
                foldStatistics.Add(statistics);
            }

            IStatisticsDictionary statisticsAvg = Utils.AverageStatistics(foldStatistics);

            (IEnumerable<IVector> trainingExamplesFinal, IEnumerable<IVector> validationExamplesFinal) =
                examples.PartitionExamples(100 / FoldsCount);
            IModel modelFinal = learningAlgorithm.GenerateModel(trainingExamplesFinal, validationExamplesFinal, metric);

            return new ValidationResult(modelFinal, statisticsAvg, string.Empty);
        }
    }
}