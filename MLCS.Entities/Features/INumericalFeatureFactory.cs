using System;

namespace MLCS.Entities.Features
{
    public interface INumericalFeatureFactory<T>
        where T : IComparable<T>
    {
        INumericalFeature<T> Create(int index, string name, T from, T to, bool isSkipFeature = false);
    }
}