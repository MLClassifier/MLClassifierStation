using MLCS.Entities.Values.Numerical;
using System;

namespace MLCS.Entities.Features.Numerical
{
    public class NumericalFeatureFactory<T> : INumericalFeatureFactory<T>
        where T : IComparable<T>
    {
        public INumericalFeature<T> Create(int index, string name, T from, T to, bool isSkipFeature = false)
        {
            return new NumericalFeature<T>
            {
                Index = index,
                Name = name,
                BorderValues = new Interval<T>()
                {
                    FromValue = from,
                    ToValue = to
                },
                UntypedBorderValues = new Interval()
                {
                    UntypedFrom = from,
                    UntypedTo = to
                },
                IsSkip = isSkipFeature
            };
        }
    }
}