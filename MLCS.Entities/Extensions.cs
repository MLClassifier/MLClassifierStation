using MLCS.Common.Utils;
using MLCS.Entities.Features;
using MLCS.Entities.Vectors;
using MLCS.Entities.Values;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MLCS.Entities
{
    public static class Extensions
    {
        public static IEnumerable<IVector<T>> ToTypedVectors<T>(this IEnumerable<IVector> vectors)
            where T : IComparable<T>
        {
            foreach (var vector in vectors)
                yield return vector as IVector<T>;
        }

        public static IEnumerable<IVector> ClearSkipFeatures(this IEnumerable<IVector> examples)
        {
            foreach (var example in examples)
            {
                foreach (var key in example.Keys)
                    if (key.IsSkip)
                        example.Remove(key);

                yield return example;
            }
        }

        public static IVector DistinctFeatures(this IEnumerable<KeyValuePair<IFeature, IValue>> keyValuePairList)
        {
            var uniqueKeyValuPairsList = keyValuePairList.GroupBy(
                kvp => kvp.Key,
                kvp => kvp.Value,
                (key, g) => new KeyValuePair<IFeature, IValue>(key,
                    g.Aggregate(
                        g.FirstOrDefault(),
                        (accumulator, value) => accumulator != null
                            ? (IValue)accumulator.FindCommon(value)
                            : value)));
            return uniqueKeyValuPairsList.Where(kvp => kvp.Value.UntypedValue != null).ToList()
                .ToDictionary(i => i.Key, i => i.Value).ToVector();
        }

        public static IVector ToVector(this IEnumerable<KeyValuePair<IFeature, IValue>> dictionary)
        {
            var vector = new Vectors.Default.Vector();
            foreach (KeyValuePair<IFeature, IValue> keyValuePair in dictionary)
                vector.Add(keyValuePair.Key, keyValuePair.Value);
            return vector;
        }

        public static IEnumerable<IVector> ToVectorList(this IEnumerable<IEnumerable<KeyValuePair<IFeature, IValue>>> keyValuePairsList)
        {
            try
            {
                return keyValuePairsList.Select(kvpl => kvpl.Where(kvp => kvp.Value.UntypedValue != null).ToList()
                            .ToDictionary(i => i.Key, i => i.Value).ToVector()).ToList();
            }
            catch
            { return null; }
        }

        public static (IEnumerable<IVector> trainingExamples, IEnumerable<IVector> testExamples) PartitionExamples(
            this IEnumerable<IVector> examples, int percent)
        {
            int examplesCount = examples.Count();
            if (examplesCount == 0)
                return (null, null);

            int testCount = (int)((examplesCount / 100d) * percent);
            int trainingCount = examplesCount - testCount;
            List<IVector> trainingExamples = new List<IVector>(trainingCount);
            List<IVector> testExamples = new List<IVector>(testCount);

            Random random = new Random();
            IFeature classFeature = examples.First().GetClassFeature();
            IValue posClassValue = classFeature.GetPositiveClassValue();
            IValue negClassValue = classFeature.GetNegativeClassValue();
            int posCount = examples.Count(e => e[classFeature].Equals(posClassValue));
            int negCount = examples.Count(e => e[classFeature].Equals(negClassValue));
            int posTrainCount = (int)(trainingCount * posCount / (double)examplesCount + 0.5);
            int negTrainCount = trainingCount - posTrainCount;
            int posTestCount = testCount * posCount / examplesCount;
            int negTestCount = testCount - posTestCount;

            var posExamples = examples.Where(e => e[classFeature].Equals(posClassValue)).ToList();
            Partition.PartialShuffle(posExamples, posTestCount, random);
            testExamples.AddRange(posExamples.Take(posTestCount));
            trainingExamples.AddRange(posExamples.Skip(posTestCount).Take(posTrainCount));

            var negExamples = examples.Where(e => e[classFeature].Equals(negClassValue)).ToList();
            Partition.PartialShuffle(negExamples, negTestCount, random);
            testExamples.AddRange(negExamples.Take(negTestCount));
            trainingExamples.AddRange(negExamples.Skip(negTestCount).Take(negTrainCount));

            return (trainingExamples, testExamples);
        }

        public static IEnumerable<IEnumerable<IVector>> PartitionExamplesInFolds(
            this IEnumerable<IVector> examples, int foldsCount)
        {
            int examplesCount = examples.Count();
            if (examplesCount == 0)
                return null;

            IFeature classFeature = examples.FirstOrDefault().GetClassFeature();
            IList<List<IVector>> folds = new List<List<IVector>>(foldsCount);

            Random random = new Random();
            for (int i = 0; i < foldsCount; i++)
            {
                folds[i] = new List<IVector>();
                IEnumerable<IValue> classValues = (classFeature as ICategorialFeature).UntypedValues;
                foreach (IFeature classValue in classValues)
                {
                    var classExamples = examples.Where(e => e.ContainsKey(classValue)).ToList();
                    int foldClassCount = classExamples.Count / foldsCount;
                    Partition.PartialShuffle(classExamples, foldClassCount, random);
                    folds[i].AddRange(classExamples.Take(foldClassCount));
                }
            }

            return folds;
        }

        public static IFeature GetClassFeature(this IVector example)
        {
            return example.Keys.FirstOrDefault(a => a.IsClass);
        }

        public static bool IsPositive(this IVector example)
        {
            var classFeature = example.Keys.FirstOrDefault(k => k.IsClass);
            var positiveClassValue = classFeature.GetPositiveClassValue();
            return example[classFeature].UntypedValue.Equals(positiveClassValue.UntypedValue);
        }

        public static IValue GetPositiveClassValue(this IFeature feature)
        {
            if (!feature.IsClass || !(feature is ICategorialFeature))
                return null;

            return (feature as ICategorialFeature).UntypedValues.FirstOrDefault();
        }

        public static IValue GetNegativeClassValue(this IFeature feature)
        {
            if (!feature.IsClass || !(feature is ICategorialFeature))
                return null;

            return (feature as ICategorialFeature).UntypedValues.Skip(1).FirstOrDefault();
        }
    }
}