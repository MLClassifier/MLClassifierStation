using MLCS.Entities.Values.Default;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MLCS.Entities.Features.Default
{
    public class FeatureComparer : IFeatureComparer
    {
        public bool Equals(IFeature x, IFeature y)
        {
            if (x.FeatureType != y.FeatureType)
                return false;

            if (!x.Name.Equals(y.Name, StringComparison.InvariantCultureIgnoreCase))
                return false;

            if (x.Index != y.Index)
                return false;

            if (x.IsClass != y.IsClass)
                return false;

            if (x.IsSkip != y.IsSkip)
                return false;

            if (x is ICategorialFeature && y is ICategorialFeature)
            {
                ICategorialFeature xCategorial = x as ICategorialFeature;
                ICategorialFeature yCategorial = y as ICategorialFeature;

                if (xCategorial.UntypedValues == null && yCategorial.UntypedValues == null) return true;
                if (xCategorial.UntypedValues == null && yCategorial.UntypedValues != null) return false;
                if (xCategorial.UntypedValues != null && yCategorial.UntypedValues == null) return false;
                if (xCategorial.UntypedValues.Count() != yCategorial.UntypedValues.Count()) return false;

                IEnumerable<CategorialValue> xValues = xCategorial.UntypedValues as IEnumerable<CategorialValue>;
                IEnumerable<CategorialValue> yValues = yCategorial.UntypedValues as IEnumerable<CategorialValue>;

                if (xValues == null || yValues == null) return false;
                if (!xValues.Any() && !yValues.Any()) return true;

                return xValues.All(xv => yValues.Contains(xv))
                    && yValues.All(yv => xValues.Contains(yv));
            }
            else if (x is INumericalFeature && y is INumericalFeature)
            {
                INumericalFeature xNumerical = x as INumericalFeature;
                INumericalFeature yNumerical = y as INumericalFeature;

                return xNumerical.UntypedBorderValues.Equals(yNumerical.UntypedBorderValues);
            }

            return false;
        }

        public int GetHashCode(IFeature obj)
        {
            throw new NotImplementedException();
        }
    }
}