using MLCS.LearningStatistics.Entities.Default;
using System;

namespace MLCS.LearningStatistics.Entities.Metrics
{
    public class FalseNegativeRate : AscendingMetric
    {
        public FalseNegativeRate() : base()
        {
        }

        public FalseNegativeRate(Type type, double result)
            : base(type, result) { }

        protected internal override double? OnCalculate(IConfusionMatrix cm)
        {
            Result = (double)cm.FalseNegatives / cm.Positives;
            return Result;
        }
    }
}