using MLCS.LearningStatistics.Entities.Default;
using System;

namespace MLCS.LearningStatistics.Entities.Metrics
{
    public class BalancedAccuracy : AscendingMetric
    {
        public BalancedAccuracy() : base()
        {
        }

        public BalancedAccuracy(Type type, double result)
            : base(type, result) { }

        protected internal override double? OnCalculate(IConfusionMatrix cm)
        {
            Result = ((double)cm.TruePositives / (cm.TruePositives + cm.FalseNegatives)
                + (double)cm.TrueNegatives / (cm.TrueNegatives + cm.FalsePositives))
                / 2d;
            return Result;
        }
    }
}