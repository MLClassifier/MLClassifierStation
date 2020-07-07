using MLCS.LearningStatistics.Entities.Default;
using System;

namespace MLCS.LearningStatistics.Entities.Metrics
{
    // https://en.wikipedia.org/wiki/Matthews_correlation_coefficient
    public class MatthewsCorrelationCoefficient : AscendingMetric
    {
        public MatthewsCorrelationCoefficient() : base()
        {
        }

        public MatthewsCorrelationCoefficient(Type type, double result)
            : base(type, result) { }

        protected internal override double? OnCalculate(IConfusionMatrix cm)
        {
            Result = (cm.TruePositives * cm.TrueNegatives - cm.FalsePositives * cm.FalseNegatives)
                / Math.Sqrt(cm.PredictedPositives * cm.Positives * cm.Negatives * cm.PredictedNegatives);
            return Result;
        }
    }
}