using MLCS.Entities.Features;
using MLCS.Entities.Vectors;
using System.Collections.Generic;

namespace MLCS.FileParser.Default
{
    public class ExamplesProvider : IExamplesProvider
    {
        private IExamplesParser examplesParser;

        public ExamplesProvider(IExamplesParser examplesParser)
        {
            this.examplesParser = examplesParser;
        }

        public IEnumerable<IVector> GetExamples(IDataProvider dataProvider, IEnumerable<IFeature> features)
        {
            IEnumerable<string> examplesData = dataProvider.GetData();
            return examplesParser.Parse(examplesData, features);
        }

        public int IsCorrectFormat(IDataProvider dataProvider, IEnumerable<IFeature> features)
        {
            IEnumerable<string> examplesData = dataProvider.GetData();
            return examplesParser.IsCorrectFormat(examplesData, features);
        }
    }
}