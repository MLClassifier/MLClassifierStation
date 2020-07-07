using MLCS.Entities.Features;
using System;

namespace MLCS.Entities.Values.Default
{
    public class CategorialValue : Value
    {
        public override int CompareTo(object other)
        {
            return ToString().CompareTo(other);
        }

        public override bool Equals(object other)
        {
            CategorialValue otherValue = other as CategorialValue;
            return UntypedValue.Equals(otherValue.UntypedValue);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Covers(ICoverable other)
        {
            CategorialValue otherValue = other as CategorialValue;
            return UntypedValue.Equals(otherValue.UntypedValue);
        }

        public override ICoverable FindCovering(ICoverable other)
        {
            object otherValue = (other as CategorialValue).UntypedValue;
            return new CategorialValue
            {
                UntypedValue = UntypedValue == null
                    ? null
                    : UntypedValue.Equals(otherValue)
                        ? null
                        : UntypedValue
            };
        }

        public override ICoverable FindCovering(ICoverable other, IFeature feature)
        {
            return FindCovering(other);
        }

        public override ICoverable FindCommon(ICoverable other)
        {
            object otherValue = (other as CategorialValue).UntypedValue;
            return new CategorialValue
            {
                UntypedValue = UntypedValue == null
                    ? null
                    : UntypedValue.Equals(otherValue)
                        ? UntypedValue
                        : null
            };
        }

        public override string ToString()
        {
            return UntypedValue.ToString();
        }
    }

    public class CategorialValue<T> : CategorialValue, IValue<T>
        where T : IComparable<T>
    {
        public T Value { get; set; }
    }
}