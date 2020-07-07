using MLCS.LearningStatistics.Entities.Default;
using System;

namespace MLCS.LearningStatistics.Entities.Metrics
{
    // https://bioinfopublication.org/files/articles/2_1_1_JMLT.pdf
    public class Markedness : AscendingMetric
    {
        public Markedness() : base()
        {
        }

        public Markedness(Type type, double result)
            : base(type, result) { }

        protected internal override double? OnCalculate(IConfusionMatrix cm)
        {
            Precision precision = new Precision();
            double? precisionValue = precision.OnCalculate(cm);
            double inversePrecision = cm.TrueNegatives
                / cm.TrueNegatives + cm.FalseNegatives;

            Result = precisionValue + inversePrecision - 1d;
            return Result;
        }
    }
}