using MLCS.Entities.Features;
using MLCS.Entities.Vectors;
using System.Collections.Generic;

namespace MLCS.FileParser
{
    public interface IExamplesProvider
    {
        IEnumerable<IVector> GetExamples(IDataProvider dataProvider, IEnumerable<IFeature> features);

        int IsCorrectFormat(IDataProvider dataProvider, IEnumerable<IFeature> features);
    }
}