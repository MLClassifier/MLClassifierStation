using MLCS.Entities;
using MLCS.Entities.Features;
using MLCS.Entities.Vectors;
using MLCS.Entities.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MLCS.MinimizationLearningAlgorithm
{
    public class MinimizationLearningAlgorithmModel : IModel
    {
        public IEnumerable<IVector> Vectors { get; private set; }

        public IEnumerable<IFeature> Features { get; private set; }

        public IEnumerable<ILearningParameter> Parameters { get; private set; }

        public string LearningLog { get; private set; }

        public MinimizationLearningAlgorithmModel(IEnumerable<IVector> vectors)
        {
            Vectors = vectors;
        }

        public MinimizationLearningAlgorithmModel(IEnumerable<IVector> vectors,
            IEnumerable<IFeature> features, IEnumerable<ILearningParameter> parameters, string learningLog)
        {
            Vectors = vectors;
            Features = features;
            Parameters = parameters;
            LearningLog = learningLog;
        }

        public ClassType Classify(IVector example)
        {
            bool isPositive = Vectors.Any(i => i.Covers(example));
            return isPositive ? ClassType.Positive : ClassType.Negative;
        }

        public string Preview()
        {
            StringBuilder result = new StringBuilder();

            int count = Vectors.Count();
            result.AppendFormat("Total count: {0}{1}", count, Environment.NewLine);

            var vectorsLengthGroups = Vectors.GroupBy(i => i.Count)
                .Select(group => new
                {
                    Length = group.Key,
                    Count = group.Count()
                }).OrderBy(x => x.Length);

            foreach (var group in vectorsLengthGroups)
                result.AppendFormat("Vectors with length {0} count: {1}{2}",
                    group.Length, group.Count, Environment.NewLine);

            return result.ToString();
        }

        public bool Export(string fileName)
        {
            try
            {
                using StreamWriter outputFile = new StreamWriter(fileName);
                foreach (IVector vector in Vectors)
                    outputFile.WriteLine(vector.ToStringLine(Features));
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Import(string fileName)
        {
            throw new NotImplementedException();
        }
    }
}