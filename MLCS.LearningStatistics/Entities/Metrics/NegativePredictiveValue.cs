using MLCS.LearningStatistics.Entities.Default;
using System;

namespace MLCS.LearningStatistics.Entities.Metrics
{
    public class NegativePredictiveValue : AscendingMetric
    {
        public NegativePredictiveValue() : base()
        {
        }

        public NegativePredictiveValue(Type type, double result)
            : base(type, result) { }

        protected internal override double? OnCalculate(IConfusionMatrix cm)
        {
            Result = (double)cm.TrueNegatives / cm.PredictedNegatives;
            return Result;
        }
    }
}