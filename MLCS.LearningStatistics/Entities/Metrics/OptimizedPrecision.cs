using MLCS.LearningStatistics.Entities.Default;
using System;

namespace MLCS.LearningStatistics.Entities.Metrics
{
    // R. Ranawana, and V. Palade, “Optimized precision-A new measure for classifier performance
    // evaluation”, in Proc.of the IEEE World Congress on Evolutionary Computation(CEC 2006), 2006,
    // pp. 2254-2261.
    public class OptimizedPrecision : AscendingMetric
    {
        public OptimizedPrecision() : base()
        {
        }

        public OptimizedPrecision(Type type, double result)
            : base(type, result) { }

        protected internal override double? OnCalculate(IConfusionMatrix cm)
        {
            Accuracy accuracy = new Accuracy();
            double? accuracyValue = accuracy.OnCalculate(cm);
            Specificity specificity = new Specificity();
            double? specificityValue = specificity.OnCalculate(cm);
            Recall recall = new Recall();
            double? recallValue = recall.OnCalculate(cm);

            Result = accuracyValue - Math.Abs(specificityValue.Value - recallValue.Value)
                / (specificityValue + recallValue);
            return Result;
        }
    }
}