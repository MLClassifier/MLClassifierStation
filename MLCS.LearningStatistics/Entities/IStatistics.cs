using MLCS.Entities.Vectors;
using MLCS.Entities.Model;
using System;
using System.Collections.Generic;

namespace MLCS.LearningStatistics.Entities
{
    public interface IStatistics : IComparable<IStatistics>
    {
        IMetric Metric { get; }

        double? Calculate(IModel model, IEnumerable<IVector> testExamples);

        double? Calculate(IConfusionMatrix confusionMatrix);
    }
}