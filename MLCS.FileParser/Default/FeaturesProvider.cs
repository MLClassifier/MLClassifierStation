using MLCS.Entities.Features;
using System.Collections.Generic;

namespace MLCS.FileParser.Default
{
    public class FeaturesProvider : IFeaturesProvider
    {
        private IFeaturesParser featuresParser;

        public FeaturesProvider(IFeaturesParser featuresParser)
        {
            this.featuresParser = featuresParser;
        }

        public IEnumerable<IFeature> GetFeatures(IDataProvider dataProvider)
        {
            IEnumerable<string> featuresData = dataProvider.GetData();
            return featuresParser.Parse(featuresData);
        }

        public int IsCorrectFormat(IDataProvider dataProvider)
        {
            IEnumerable<string> featuresData = dataProvider.GetData();
            return featuresParser.IsCorrectFormat(featuresData);
        }
    }
}