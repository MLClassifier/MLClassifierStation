using MLCS.LearningStatistics.Entities.Default;
using System;

namespace MLCS.LearningStatistics.Entities.Metrics
{
    public class Recall : AscendingMetric
    {
        public Recall() : base()
        {
        }

        public Recall(Type type, double result)
            : base(type, result) { }

        protected internal override double? OnCalculate(IConfusionMatrix cm)
        {
            Result = (double)cm.TruePositives / cm.Positives;
            return Result;
        }
    }
}