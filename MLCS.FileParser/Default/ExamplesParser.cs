using MLCS.Entities.Features;
using MLCS.Entities.Vectors;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TinyCsvParser;
using TinyCsvParser.Mapping;

namespace MLCS.FileParser.Default
{
    public class ExamplesParser : IExamplesParser
    {
        private ICsvMappingFactory csvMappingFactory;

        public ExamplesParser(ICsvMappingFactory csvMappingFactory)
        {
            this.csvMappingFactory = csvMappingFactory;
        }

        public IEnumerable<IVector> Parse(IEnumerable<string> examplesData, IEnumerable<IFeature> features)
        {
            CsvParser csvParser = InitializeCsvParser(features);
            return csvParser.ReadFromStrings(examplesData, Encoding.ASCII).Select(e => e.Result);
        }

        public int IsCorrectFormat(IEnumerable<string> examplesData, IEnumerable<IFeature> features)
        {
            CsvParser csvParser = InitializeCsvParser(features);
            return csvParser.CheckFormatFromStrings(examplesData, Encoding.ASCII);
        }

        private CsvParser InitializeCsvParser(IEnumerable<IFeature> features)
        {
            CsvParserOptions csvParserOptions = new CsvParserOptions(false, ',', 1, true); // TODO change after test

            ICsvMapping csvMapper = csvMappingFactory.Create();
            csvMapper.MapFeatures(features);

            return new CsvParser(csvParserOptions, csvMapper);
        }
    }
}