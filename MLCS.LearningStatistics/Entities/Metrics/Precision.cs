using MLCS.LearningStatistics.Entities.Default;
using System;

namespace MLCS.LearningStatistics.Entities.Metrics
{
    public class Precision : AscendingMetric
    {
        public Precision() : base()
        {
        }

        public Precision(Type type, double result)
            : base(type, result) { }

        protected internal override double? OnCalculate(IConfusionMatrix cm)
        {
            Result = (double)cm.TruePositives / cm.PredictedPositives;
            return Result;
        }
    }
}