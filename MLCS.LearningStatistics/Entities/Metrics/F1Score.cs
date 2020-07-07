using MLCS.LearningStatistics.Entities.Default;
using System;

namespace MLCS.LearningStatistics.Entities.Metrics
{
    public class F1Score : AscendingMetric
    {
        public F1Score() : base()
        {
        }

        public F1Score(Type type, double result)
            : base(type, result) { }

        protected internal override double? OnCalculate(IConfusionMatrix cm)
        {
            Precision precision = new Precision();
            double? precisionValue = precision.OnCalculate(cm);
            Recall recall = new Recall();
            double? recallValue = recall.OnCalculate(cm);

            Result = 2d / (1d / precisionValue + 1d / recallValue);
            return Result;
        }
    }
}