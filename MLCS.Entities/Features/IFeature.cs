using MLCS.Entities.Values;
using System;

namespace MLCS.Entities.Features
{
    public interface IFeature : IComparable
    {
        Type FeatureType { get; }
        string Name { get; set; }
        int Index { get; set; }
        bool IsClass { get; set; }
        bool IsSkip { get; set; }

        bool AllowsValue(IValue value);
    }

    public interface IFeature<in T> : IFeature
        where T : IComparable<T>
    {
    }
}