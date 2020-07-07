using MLCS.LearningStatistics.Entities.Default;
using System;

namespace MLCS.LearningStatistics.Entities.Metrics
{
    // https://bioinfopublication.org/files/articles/2_1_1_JMLT.pdf
    public class Informedness : AscendingMetric
    {
        public Informedness() : base()
        {
        }

        public Informedness(Type type, double result)
            : base(type, result) { }

        protected internal override double? OnCalculate(IConfusionMatrix cm)
        {
            Recall recall = new Recall();
            double? recallValue = recall.OnCalculate(cm);
            Specificity specificity = new Specificity();
            double? specificityValue = specificity.OnCalculate(cm);

            Result = recallValue + specificityValue - 1d;
            return Result;
        }
    }
}