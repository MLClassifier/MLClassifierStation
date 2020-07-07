using MLCS.Entities;
using MLCS.Entities.Features;
using MLCS.Entities.FeatureValuePair.Default;
using MLCS.Entities.Vectors;
using MLCS.Entities.Vectors.Default;
using MLCS.Entities.Model;
using MLCS.Entities.Values;
using MLCS.LearningAlgorithmPlugins.Default;
using MLCS.LearningStatistics.Entities;
using MLCS.LearningStatistics.Entities.Default;
using System.Collections.Generic;
using System.Linq;

namespace MLCS.MinimizationLearningAlgorithm
{
    public class MinimizationLearningAlgorithm : BaseLearningAlgorithm
    {
        protected override IModel OnGenerateModel(IEnumerable<IVector> trainingExamples,
            IEnumerable<IVector> validationExamples, IMetric metric = null)
        {
            return GenerateModel(trainingExamples, validationExamples, metric);
        }

        public override IModel GenerateModel(IEnumerable<IVector> trainingExamples,
            IEnumerable<IVector> validationExamples, IMetric metric = null)
        {
            IEnumerable<IVector> vectors = GeneratePrimaryVectors(trainingExamples);
            IModel model = PrunePrimaryVectors(vectors, validationExamples, metric);

            return model;
        }

        private IEnumerable<IVector> GeneratePrimaryVectors(IEnumerable<IVector> trainingExamples)
        {
            IFeature classFeature = trainingExamples.First().GetClassFeature();
            IValue positiveClassValue = classFeature.GetPositiveClassValue();
            IValue negativeClassValue = classFeature.GetNegativeClassValue();
            IEnumerable<IVector> positiveExamples = SelectExamplesByClass(trainingExamples, classFeature, positiveClassValue);
            IEnumerable<IVector> negativeExamples = SelectExamplesByClass(trainingExamples, classFeature, negativeClassValue);

            IEnumerable<IEnumerable<IVector>> negOfNegExamples = GenerateNegationOfNegativeExamples(positiveExamples, negativeExamples);

            IEnumerable<IVector> vectors = GenerateCartesianProducts(negOfNegExamples);

            return vectors;
        }

        private IEnumerable<IVector> SelectExamplesByClass(IEnumerable<IVector> examples, IFeature classFeature, IValue classValue)
        {
            var classExamples = examples
                .Where(e => (e as IDictionary<IFeature, IValue>)[classFeature].UntypedValue.Equals(classValue.UntypedValue));
            return classExamples;
        }

        private IEnumerable<IEnumerable<IVector>> GenerateNegationOfNegativeExamples(
            IEnumerable<IVector> positiveExamples, IEnumerable<IVector> negativeExamples)
        {
            IEnumerable<IEnumerable<IVector>> negOfNegExamples = positiveExamples.Select(ePos =>
                negativeExamples.Select(eNeg => ePos.FindCovering(eNeg) as IVector)).Where(i => i.Any());
            return negOfNegExamples;
        }

        private IEnumerable<IVector> GenerateCartesianProducts(IEnumerable<IEnumerable<IVector>> negOfNegExamples)
        {
            List<IEnumerable<KeyValuePair<IFeature, IValue>>> resKeyValuesList = new List<IEnumerable<KeyValuePair<IFeature, IValue>>>();
            foreach (IEnumerable<IVector> disjVectors in negOfNegExamples)
            {
                List<IEnumerable<KeyValuePair<IFeature, IValue>>> conjKeyValuesList = new List<IEnumerable<KeyValuePair<IFeature, IValue>>>();
                foreach (IEnumerable<KeyValuePair<IFeature, IValue>> disjVector in disjVectors)
                {
                    if (conjKeyValuesList.Count == 0)
                    {
                        conjKeyValuesList.Add(disjVector);
                        continue;
                    }

                    FeatureValuePairComparer comparer = new FeatureValuePairComparer();
                    List<IEnumerable<KeyValuePair<IFeature, IValue>>> tempList = new List<IEnumerable<KeyValuePair<IFeature, IValue>>>();
                    foreach (var conjVector in conjKeyValuesList)
                        foreach (var keyValue in disjVector)
                            tempList.Add(Merge(keyValue, comparer, conjVector));

                    conjKeyValuesList = tempList;

                    var conjVectors = conjKeyValuesList.ToVectorList();
                    conjKeyValuesList = SelectPrimaryVectors(conjVectors)
                        .Select(i => i as IEnumerable<KeyValuePair<IFeature, IValue>>).ToList();
                }

                resKeyValuesList.AddRange(conjKeyValuesList);
                resKeyValuesList = SelectPrimaryVectors(resKeyValuesList.ToVectorList())
                        .Select(i => i as IEnumerable<KeyValuePair<IFeature, IValue>>).ToList();
            }

            return resKeyValuesList.ToVectorList();
        }

        public IEnumerable<KeyValuePair<IFeature, IValue>> Merge(KeyValuePair<IFeature, IValue> item,
            IEqualityComparer<KeyValuePair<IFeature, IValue>> comparer, IEnumerable<KeyValuePair<IFeature, IValue>> sequence)
        {
            bool existsFeature = sequence.Any(kvp => kvp.Key.Index == item.Key.Index);
            if (existsFeature)
            {
                KeyValuePair<IFeature, IValue> existingItem = sequence.First(kvp => kvp.Key.Index == item.Key.Index);
                KeyValuePair<IFeature, IValue> newItem = new KeyValuePair<IFeature, IValue>
                    (item.Key, existingItem.Value.FindCommon(item.Value) as IValue);

                if (!comparer.Equals(newItem, existingItem))
                {
                    List<KeyValuePair<IFeature, IValue>> list = sequence.ToList();
                    list.Remove(existingItem);
                    list.Add(newItem);
                    return list;
                }

                return sequence;
            }
            else
            {
                List<KeyValuePair<IFeature, IValue>> list = sequence.ToList();
                list.Add(item);
                return list;
            }
        }

        private IEnumerable<IVector> SelectPrimaryVectors(IEnumerable<IVector> vectors)
        {
            var primaryVectors = vectors.Distinct(new VectorEqualityComparer())
                .Where(i => vectors.All(ii => ii.Equals(i) || !ii.Covers(i)));
            return primaryVectors;
        }

        private IModel PrunePrimaryVectors(IEnumerable<IVector> vectors,
            IEnumerable<IVector> tuningExamples, IMetric metric = null)
        {
            var vectorCoveredTuningCount = vectors.Select(i => new VectorCoveredCount
            {
                Vector = i,
                CoveredCount = tuningExamples.Count(e => i.Covers(e))
            });

            int tuningFactor = FindBestTuningFactor(vectorCoveredTuningCount, tuningExamples, metric);

            IEnumerable<IVector> prunedVectors = vectorCoveredTuningCount
                .Where(ictc => ictc.CoveredCount >= tuningFactor).Select(ictc => ictc.Vector);
            IEnumerable<ILearningParameter> parameters = GetParameters();
            IEnumerable<IFeature> features = tuningExamples.First().Keys;

            return new MinimizationLearningAlgorithmModel(prunedVectors, features, parameters, string.Empty);
        }

        private int FindBestTuningFactor(IEnumerable<VectorCoveredCount> vectorCoveredTuningCount,
            IEnumerable<IVector> tuningExamples, IMetric metric)
        {
            int tuningFactorBest = vectorCoveredTuningCount.Max(ictc => ictc.CoveredCount);
            int tuningFactor = tuningFactorBest;
            IStatistics statistics = new Statistics(metric);
            IStatistics statisticsBest = new Statistics(metric);
            var vectors = vectorCoveredTuningCount.Select(ictc => ictc.Vector);
            IModel tempModel = new MinimizationLearningAlgorithmModel(vectors);
            statisticsBest.Calculate(tempModel, tuningExamples);
            while (tuningFactor > 0)
            {
                var remainingVectors = vectorCoveredTuningCount
                    .Where(ictc => ictc.CoveredCount >= tuningFactor)
                    .Select(i => i.Vector);
                tempModel = new MinimizationLearningAlgorithmModel(remainingVectors);
                statistics.Calculate(tempModel, tuningExamples);

                if (statistics.CompareTo(statisticsBest) >= 0)
                {
                    statisticsBest = statistics;
                    tuningFactorBest = tuningFactor;
                }

                tuningFactor--;
            }

            return tuningFactorBest;
        }

        private class VectorCoveredCount
        {
            public IVector Vector { get; set; }
            public int CoveredCount { get; set; }
        }
    }
}