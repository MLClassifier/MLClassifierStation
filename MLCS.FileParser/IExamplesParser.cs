using MLCS.Entities.Features;
using MLCS.Entities.Vectors;
using System.Collections.Generic;

namespace MLCS.FileParser
{
    public interface IExamplesParser
    {
        IEnumerable<IVector> Parse(IEnumerable<string> examplesData, IEnumerable<IFeature> features);

        int IsCorrectFormat(IEnumerable<string> examplesData, IEnumerable<IFeature> features);
    }
}