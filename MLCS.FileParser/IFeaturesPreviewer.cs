using MLCS.Entities.Features;
using System.Collections.Generic;

namespace MLCS.FileParser
{
    public interface IFeaturesPreviewer
    {
        string Preview(IEnumerable<IFeature> features);
    }
}