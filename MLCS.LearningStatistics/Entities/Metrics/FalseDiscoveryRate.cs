using MLCS.LearningStatistics.Entities.Default;
using System;

namespace MLCS.LearningStatistics.Entities.Metrics
{
    public class FalseDiscoveryRate : AscendingMetric
    {
        public FalseDiscoveryRate() : base()
        {
        }

        public FalseDiscoveryRate(Type type, double result)
            : base(type, result) { }

        protected internal override double? OnCalculate(IConfusionMatrix cm)
        {
            Result = (double)cm.FalsePositives / cm.PredictedPositives;
            return Result;
        }
    }
}