using MLCS.Entities.Features;
using System;

namespace MLCS.Entities
{
    public interface ICoverable
    {
        bool Covers(ICoverable other);

        ICoverable FindCovering(ICoverable other);

        ICoverable FindCovering(ICoverable other, IFeature feature);

        ICoverable FindCommon(ICoverable other);
    }

    public interface ICoverable<T> : ICoverable
        where T : IComparable<T>
    {
    }
}