using MLCS.LearningStatistics.Entities.Default;
using System;

namespace MLCS.LearningStatistics.Entities.Metrics
{
    public class Accuracy : AscendingMetric
    {
        public Accuracy() : base()
        {
        }

        public Accuracy(Type type, double result)
            : base(type, result) { }

        protected internal override double? OnCalculate(IConfusionMatrix cm)
        {
            Result = (double)cm.CorrectPredictions / cm.Total;
            return Result;
        }
    }
}