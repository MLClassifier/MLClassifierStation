using MLCS.Entities.Values;
using System;
using System.Collections.Generic;

namespace MLCS.Entities.Features
{
    public interface ICategorialFeature : IFeature
    {
        IEnumerable<IValue> UntypedValues { get; set; }
    }

    public interface ICategorialFeature<T> : IFeature<T>, ICategorialFeature
        where T : IComparable<T>
    {
        IEnumerable<IValue<T>> Values { get; set; }
    }
}