using MLCS.LearningStatistics.Entities.Default;
using System;

namespace MLCS.LearningStatistics.Entities.Metrics
{
    // http://www.statisticshowto.com/uncertainty-coefficient/
    public class UncertaintyCoefficient : AscendingMetric
    {
        public UncertaintyCoefficient() : base()
        {
        }

        public UncertaintyCoefficient(Type type, double result)
            : base(type, result) { }

        protected internal override double? OnCalculate(IConfusionMatrix cm)
        {
            double L = cm.Total * Math.Log(cm.Total);
            double LTP = cm.TruePositives * Math.Log((double)cm.TruePositives / cm.PredictedPositives * cm.Positives);
            double LFP = cm.FalsePositives * Math.Log((double)cm.FalsePositives / cm.PredictedPositives * cm.Negatives);
            double LFN = cm.FalseNegatives * Math.Log((double)cm.FalseNegatives / cm.Positives * cm.PredictedNegatives);
            double LTN = cm.TrueNegatives * Math.Log((double)cm.TrueNegatives / cm.Negatives * cm.PredictedNegatives);
            double LP = cm.Positives * Math.Log((double)cm.Positives / cm.Total);
            double LN = cm.Negatives * Math.Log((double)cm.Negatives / cm.Total);

            Result = (L + LTP + LFP + LFN + LTN) / (L + LP + LN);
            return Result;
        }
    }
}