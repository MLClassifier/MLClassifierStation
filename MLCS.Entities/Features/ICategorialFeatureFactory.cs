using System;

namespace MLCS.Entities.Features
{
    public interface ICategorialFeatureFactory<T>
        where T : IComparable<T>
    {
        ICategorialFeature<T> Create(int index, string name, bool isClass, T[] values, bool isSkipFeature = false);
    }
}