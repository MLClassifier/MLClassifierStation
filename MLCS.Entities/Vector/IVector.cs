using MLCS.Entities.Features;
using MLCS.Entities.Values;
using System;
using System.Collections.Generic;

namespace MLCS.Entities.Vectors
{
    public interface IVector : IDictionary<IFeature, IValue>,
                                  IEquatable<IVector>,
                                  IComparable<IVector>,
                                  ICoverable<IVector>
    {
        string ToStringLine(IEnumerable<IFeature> features);

        bool AddFeatureValuePair(IFeature feature, IValue value);
    }

    public interface IVector<T> : IVector
        where T : IComparable<T>
    {
    }
}