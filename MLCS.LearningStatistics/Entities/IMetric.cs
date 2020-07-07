using System;

namespace MLCS.LearningStatistics.Entities
{
    public interface IMetric : IComparable<IMetric>
    {
        Type Type { get; }
        double? Result { get; }

        double? Calculate(IConfusionMatrix confusionMatrix);
    }
}