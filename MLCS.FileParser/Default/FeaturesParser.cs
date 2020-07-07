using MLCS.Common;
using MLCS.Entities.Features;
using System.Collections.Generic;
using System.Linq;
using TinyCsvParser.Model;

namespace MLCS.FileParser.Default
{
    public class FeaturesParser : IFeaturesParser
    {
        private IFeatureFactory featureFactory;

        public FeaturesParser(IFeatureFactory featureFactory)
        {
            this.featureFactory = featureFactory;
        }

        public IEnumerable<IFeature> Parse(IEnumerable<string> featuresData)
        {
            var lines = featuresData
                .Select((line, index) => new Row(index, line));

            return ReadFeatures(lines);
        }

        public int IsCorrectFormat(IEnumerable<string> featuresData)
        {
            var lines = featuresData
               .Select((line, index) => new Row(index, line));

            return CheckFormat(lines);
        }

        private IEnumerable<IFeature> ReadFeatures(IEnumerable<Row> lines)
        {
            return lines.Select(line => featureFactory.Create(line.Index, line.Data));
        }

        private int CheckFormat(IEnumerable<Row> lines)
        {
            foreach (Row line in lines)
            {
                try
                {
                    featureFactory.Create(line.Index, line.Data);
                }
                catch
                {
                    return line.Index;
                }
            }

            return Constants.CorrectFormatLineIndexResult;
        }
    }
}