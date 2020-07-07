using MLCS.Entities.Features;
using MLCS.Entities.Values;
using System.Collections.Generic;

namespace MLCS.Entities.FeatureValuePair.Default
{
    public class FeatureValuePairComparer : IFeatureValuePairComparer
    {
        public bool Equals(KeyValuePair<IFeature, IValue> x, KeyValuePair<IFeature, IValue> y)
        {
            if (x.Key != y.Key)
                return false;

            if (!x.Value.Equals(y.Value))
                return false;

            return true;
        }

        public int GetHashCode(KeyValuePair<IFeature, IValue> obj)
        {
            return obj.Key.GetHashCode();
        }
    }
}