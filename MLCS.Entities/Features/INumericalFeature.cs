using System;

namespace MLCS.Entities.Features
{
    public interface INumericalFeature : IFeature
    {
        IInterval UntypedBorderValues { get; set; }
    }

    public interface INumericalFeature<T> : IFeature<T>, INumericalFeature
        where T : IComparable<T>
    {
        IInterval<T> BorderValues { get; set; }
    }
}