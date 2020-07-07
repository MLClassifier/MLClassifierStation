using MLCS.Entities.Features;
using MLCS.Entities.Vectors;
using MLCS.Entities.Model;
using MLCS.LearningAlgorithmPlugins;
using MLCS.LearningStatistics.Entities;
using System.Collections.Generic;

namespace MLCS.Services
{
    public interface IModelService
    {
        IEnumerable<IFeature> Features { get; set; }
        IEnumerable<IVector> LearningExamples { get; set; }
        ILearningAlgorithm LearningAlgorithm { get; set; }
        IValidationScheme ValidationScheme { get; set; }
        IMetric Metric { get; set; }
        string ModelInformation { get; set; }
        string StatisticsInformation { get; set; }
        string LearningResultFolderPath { get; set; }
        IEnumerable<IVector> ClassificationExamples { get; set; }
        IModel Model { get; set; }
        string ClassificationResultInformation { get; set; }
        string ClassificationResultFolderPath { get; set; }

        void Clear();
    }
}