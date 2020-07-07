using MLCS.LearningStatistics.Entities.Default;
using System;

namespace MLCS.LearningStatistics.Entities.Metrics
{
    public class Fallout : AscendingMetric
    {
        public Fallout() : base()
        {
        }

        public Fallout(Type type, double result)
            : base(type, result) { }

        protected internal override double? OnCalculate(IConfusionMatrix cm)
        {
            Result = (double)cm.FalsePositives / cm.Negatives;
            return Result;
        }
    }
}