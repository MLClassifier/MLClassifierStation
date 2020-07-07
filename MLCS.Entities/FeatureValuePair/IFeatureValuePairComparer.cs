using MLCS.Entities.Features;
using MLCS.Entities.Values;
using System.Collections.Generic;

namespace MLCS.Entities.FeatureValuePair
{
    public interface IFeatureValuePairComparer : IEqualityComparer<KeyValuePair<IFeature, IValue>>
    {
    }
}