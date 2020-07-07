using MLCS.Entities.Features;
using MLCS.Entities.Vectors;
using MLCS.Entities.Model;
using MLCS.LearningAlgorithmPlugins;
using MLCS.LearningStatistics.Entities;
using System.Collections.Generic;

namespace MLCS.Services.Default
{
    public class ModelService : IModelService
    {
        public IEnumerable<IFeature> Features { get; set; }
        public IEnumerable<IVector> LearningExamples { get; set; }
        public ILearningAlgorithm LearningAlgorithm { get; set; }
        public IValidationScheme ValidationScheme { get; set; }
        public IMetric Metric { get; set; }
        public string ModelInformation { get; set; }
        public string StatisticsInformation { get; set; }
        public string LearningResultFolderPath { get; set; }
        public IEnumerable<IVector> ClassificationExamples { get; set; }
        public string ClassificationResultInformation { get; set; }
        public string ClassificationResultFolderPath { get; set; }
        public IModel Model { get; set; }

        public void Clear()
        {
            Features = null;
            LearningExamples = null;
            LearningAlgorithm = null;
            ValidationScheme = null;
            Metric = null;
            ModelInformation = null;
            StatisticsInformation = null;
            LearningResultFolderPath = null;
            ClassificationExamples = null;
            ClassificationResultInformation = null;
            ClassificationResultFolderPath = null;
        }
    }
}