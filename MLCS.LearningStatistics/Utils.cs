using MLCS.Common;
using MLCS.Entities;
using MLCS.Entities.Features;
using MLCS.Entities.Vectors;
using MLCS.Entities.Model;
using MLCS.LearningStatistics.Entities;
using MLCS.LearningStatistics.Entities.Default;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MLCS.LearningStatistics
{
    public static class Utils
    {
        public static IConfusionMatrix CalculateConfusionMatrix(IModel model, IEnumerable<IVector> testExamples)
        {
            var example = testExamples.FirstOrDefault();
            if (example is null) return null;

            var classFeature = example.GetClassFeature();

            var testExamplesPredictions = testExamples.Select(e => new
            {
                Class = e[classFeature].UntypedValue,
                Prediction = model.Classify(e)
            });

            int[][] confisionMatrix = testExamplesPredictions.GroupBy(
                ep => ep.Class,
                ep => ep.Prediction,
                (key, g) => new
                {
                    Class = key,
                    Predictions = new int[]
                    {
                        g.Count(p => p == ClassType.Positive),
                        g.Count(p => p == ClassType.Negative)
                    }
                }).Select(g => g.Predictions).ToArray();

            bool existsPositive = testExamples.Any(e => e.IsPositive());

            return existsPositive
                ? new ConfusionMatrix(confisionMatrix[0][0], confisionMatrix[0][1],
                    confisionMatrix.Length > 1 ? confisionMatrix[1][0] : 0,
                    confisionMatrix.Length > 1 ? confisionMatrix[1][1] : 0)
                : new ConfusionMatrix(0, 0, confisionMatrix[0][0], confisionMatrix[0][1]);
        }

        public static IEnumerable<IMetric> GetMetrics()
        {
            foreach (Type metricType in Assembly.GetExecutingAssembly().GetTypes()
                 .Where(metricType => !metricType.IsAbstract && metricType.GetInterfaces().Contains(typeof(IMetric))))
                yield return (IMetric)Activator.CreateInstance(metricType);
        }

        public static IStatisticsDictionary CalculateAll(IModel model,
            IEnumerable<IVector> testExamples, IFeature classFeature)
        {
            IStatisticsDictionary results = new StatisticsDictionary();
            IConfusionMatrix confusionMatrix = CalculateConfusionMatrix(model, testExamples);
            IEnumerable<IMetric> metrics = GetMetrics();
            foreach (IMetric metric in metrics)
            {
                IStatistics statistics = new Statistics(metric);
                statistics.Calculate(confusionMatrix);
                results.Add(metric.Type, statistics);
            }
            return results;
        }

        public static IStatisticsDictionary AverageStatistics(IEnumerable<IStatisticsDictionary> statistics)
        {
            return statistics.SelectMany(s => s)
                         .GroupBy(kvp => kvp.Key)
                         .ToDictionary(g => g.Key,
                                       g => g.Average(kvp => kvp.Value.Metric.Result ?? 0d))
                         .Select(kvp => new Statistics((IMetric)Activator.CreateInstance(
                             kvp.Key, new object[] { kvp.Key, kvp.Value })))
                         as IStatisticsDictionary;
        }

        public static int Compare(double? x, double? y)
        {
            var comparer = new NullableComparer<double>();
            return comparer.Compare(x, y);
        }
    }
}