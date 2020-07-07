using MLCS.Entities.Model;
using MLCS.LearningStatistics.Entities;

namespace MLCS.LearningAlgorithmPlugins.Default
{
    public class ValidationResult : IValidationResult
    {
        public IModel Model { get; private set; }
        public IStatisticsDictionary Statistics { get; private set; }
        public string ValidationLog { get; private set; }

        public ValidationResult(IModel model, IStatisticsDictionary statistics, string validationLog)
        {
            Model = model;
            Statistics = statistics;
            ValidationLog = validationLog;
        }
    }
}