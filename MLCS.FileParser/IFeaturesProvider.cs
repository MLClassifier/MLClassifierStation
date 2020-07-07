using MLCS.Entities.Features;
using System.Collections.Generic;

namespace MLCS.FileParser
{
    public interface IFeaturesProvider
    {
        IEnumerable<IFeature> GetFeatures(IDataProvider dataProvider);

        int IsCorrectFormat(IDataProvider dataProvider);
    }
}