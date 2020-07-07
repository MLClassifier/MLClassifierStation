using MLCS.LearningStatistics.Entities.Default;
using System;

namespace MLCS.LearningStatistics.Entities.Metrics
{
    public class CohensKappa : AscendingMetric
    {
        public CohensKappa() : base()
        {
        }

        public CohensKappa(Type type, double result)
            : base(type, result) { }

        protected internal override double? OnCalculate(IConfusionMatrix cm)
        {
            Accuracy accuracy = new Accuracy();
            double? accuracyValue = accuracy.OnCalculate(cm);

            double probabylityOfRandomAgreement = ((cm.TruePositives + cm.FalsePositives)
                    * (cm.TruePositives + cm.FalseNegatives)
                    + (cm.FalseNegatives + cm.TrueNegatives)
                    * (cm.FalsePositives + cm.TrueNegatives))
                / Math.Pow((cm.TruePositives + cm.FalsePositives
                    + cm.FalseNegatives + cm.TrueNegatives), 2);

            Result = (accuracyValue - probabylityOfRandomAgreement) / (1 - probabylityOfRandomAgreement);
            return Result;
        }
    }
}