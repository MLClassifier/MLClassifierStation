using System;

namespace MLCS.Entities.Values
{
    public interface IValue : IEquatable<object>, IComparable<object>, ICoverable
    {
        object UntypedValue { get; set; }
    }

    public interface IValue<T> : IValue
        where T : IComparable<T>
    {
        T Value { get; set; }
    }
}