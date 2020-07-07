using MLCS.Entities.Vectors;
using MLCS.Entities.Model;
using System;
using System.Collections.Generic;

namespace MLCS.LearningStatistics.Entities.Default
{
    // https://richnewman.wordpress.com/about/code-listings-and-diagrams/dependency-injection-examples/dependency-injection-example-interface-injection/
    public class Statistics : IStatistics
    {
        public IMetric Metric { get; private set; }

        public Statistics(IMetric metric)
        {
            Metric = metric;
        }

        public double? Calculate(IModel model, IEnumerable<IVector> testExamples)
        {
            IConfusionMatrix confusionMatrix = Utils.CalculateConfusionMatrix(model, testExamples);

            return Calculate(confusionMatrix);
        }

        public double? Calculate(IConfusionMatrix confusionMatrix)
        {
            double? result;
            try
            {
                result = Metric.Calculate(confusionMatrix);
            }
            catch (DivideByZeroException)
            {
                result = null;
            }
            return result;
        }

        public int CompareTo(IStatistics other)
        {
            if (Metric.GetType() != other.Metric.GetType())
                throw new ArgumentException("Both metrics must be of the same type");

            return Metric.CompareTo(other.Metric);
        }

        public override string ToString()
        {
            return string.Format("{0}: {1}",
                Metric.Type.Name,
                Metric.Result.HasValue
                    ? string.Format("{0:0.##}", Metric.Result)
                    : "Not available");
        }
    }
}