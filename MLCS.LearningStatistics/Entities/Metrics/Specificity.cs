using MLCS.LearningStatistics.Entities.Default;
using System;

namespace MLCS.LearningStatistics.Entities.Metrics
{
    public class Specificity : AscendingMetric
    {
        public Specificity() : base()
        {
        }

        public Specificity(Type type, double result)
            : base(type, result) { }

        protected internal override double? OnCalculate(IConfusionMatrix cm)
        {
            Result = (double)cm.TrueNegatives / cm.Negatives;
            return Result;
        }
    }
}