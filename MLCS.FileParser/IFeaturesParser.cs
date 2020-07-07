using MLCS.Entities.Features;
using System.Collections.Generic;

namespace MLCS.FileParser
{
    public interface IFeaturesParser
    {
        IEnumerable<IFeature> Parse(IEnumerable<string> featuresData);

        int IsCorrectFormat(IEnumerable<string> data);
    }
}