using System;

namespace MLCS.Entities
{
    public interface IInterval : IComparable, ICoverable
    {
        object UntypedFrom { get; set; }
        object UntypedTo { get; set; }

        bool Equals(IInterval other);
    }

    public interface IInterval<T> : IInterval, IEquatable<IInterval<T>>, IComparable<IInterval<T>>
        where T : IComparable<T>
    {
        T FromValue { get; set; }
        T ToValue { get; set; }
    }
}