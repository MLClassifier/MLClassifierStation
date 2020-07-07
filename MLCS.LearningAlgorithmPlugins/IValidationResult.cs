using MLCS.Entities.Model;
using MLCS.LearningStatistics.Entities;

namespace MLCS.LearningAlgorithmPlugins
{
    public interface IValidationResult
    {
        IModel Model { get; }
        IStatisticsDictionary Statistics { get; }
        string ValidationLog { get; }
    }
}