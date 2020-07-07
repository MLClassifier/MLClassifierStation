using MLCS.Entities.Features;
using MLCS.Entities.Values.Default;
using System;
using System.Collections.Generic;

namespace MLCS.Entities.Values.Numerical
{
    public class NumericalValue : Value, IValue
    {
        public override int CompareTo(object other)
        {
            IInterval otherInterval = (other as NumericalValue).UntypedValue as IInterval;
            IInterval interval = UntypedValue as IInterval;

            return interval.CompareTo(otherInterval);
        }

        public override bool Covers(ICoverable other)
        {
            NumericalValue otherValue = other as NumericalValue;
            return (UntypedValue as ICoverable).Covers(otherValue.UntypedValue as ICoverable);
        }

        public override bool Equals(object other)
        {
            IInterval otherInterval = (other as NumericalValue).UntypedValue as IInterval;
            IInterval interval = UntypedValue as IInterval;
            return interval.Equals(otherInterval);
        }

        public override ICoverable FindCovering(ICoverable other)
        {
            return FindCovering(other, null);
        }

        public override ICoverable FindCovering(ICoverable other, IFeature feature)
        {
            IInterval otherInterval = (other as NumericalValue).UntypedValue as IInterval;
            IInterval interval = UntypedValue as IInterval;
            return new NumericalValue()
            {
                UntypedValue = interval.FindCovering(otherInterval, feature)
            };
        }

        public override ICoverable FindCommon(ICoverable other)
        {
            IInterval otherInterval = (other as NumericalValue).UntypedValue as IInterval;
            IInterval interval = UntypedValue as IInterval;
            return new NumericalValue()
            {
                UntypedValue = interval.FindCommon(otherInterval)
            };
        }

        public override int GetHashCode()
        {
            var hashCode = 1803197125;
            hashCode = hashCode * -1521134295 + EqualityComparer<object>.Default.GetHashCode(UntypedValue);
            return hashCode;
        }

        public override string ToString()
        {
            IInterval interval = UntypedValue as IInterval;
            return string.Format("{0}..{1}", interval.UntypedFrom, interval.UntypedTo);
        }
    }

    public class NumericalValue<T> : NumericalValue, IValue<T>
        where T : IComparable<T>
    {
        public T Value { get; set; }

        public new object UntypedValue
        {
            get => Value;
            set => Value = (T)value;
        }

        public Type ValueType => typeof(T);
    }
}