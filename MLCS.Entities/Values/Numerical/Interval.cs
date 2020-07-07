using MLCS.Entities.Features;
using System;

namespace MLCS.Entities.Values.Numerical
{
    public class Interval : IInterval
    {
        public object UntypedFrom { get; set; }
        public object UntypedTo { get; set; }

        public int CompareTo(object obj)
        {
            IInterval otherInterval = obj as IInterval;

            if (UntypedFrom.Equals(otherInterval.UntypedFrom)
                && UntypedTo.Equals(otherInterval.UntypedTo))
                return 0;

            if ((UntypedFrom as IComparable).CompareTo(otherInterval.UntypedFrom) <= 0
                && (UntypedTo as IComparable).CompareTo(otherInterval.UntypedTo) >= 0)
                return 1;

            return -1;
        }

        public bool Covers(ICoverable other)
        {
            IInterval otherInterval = other as IInterval;
            return (UntypedFrom as IComparable).CompareTo(otherInterval.UntypedFrom) <= 0
                && (UntypedTo as IComparable).CompareTo(otherInterval.UntypedTo) >= 0;
        }

        public bool Equals(IInterval other)
        {
            return UntypedFrom.Equals(other.UntypedFrom)
                && UntypedTo.Equals(other.UntypedTo);
        }

        public ICoverable FindCommon(ICoverable other)
        {
            IInterval otherInterval = other as IInterval;
            return new Interval()
            {
                UntypedFrom = (UntypedFrom as IComparable).CompareTo(otherInterval.UntypedFrom as IComparable) >= 0
                        ? UntypedFrom
                        : otherInterval.UntypedFrom,
                UntypedTo = (UntypedTo as IComparable).CompareTo(otherInterval.UntypedTo as IComparable) <= 0
                        ? UntypedTo
                        : otherInterval.UntypedTo
            };
        }

        public ICoverable FindCovering(ICoverable other, IFeature feature)
        {
            IInterval otherInterval = other as IInterval;
            INumericalFeature numericalFeature = feature as INumericalFeature;
            return new Interval()
            {
                UntypedFrom = (UntypedFrom as IComparable).CompareTo(otherInterval.UntypedFrom as IComparable) >= 0
                        ? otherInterval.UntypedFrom
                        : numericalFeature.UntypedBorderValues.UntypedFrom,
                UntypedTo = (UntypedTo as IComparable).CompareTo(otherInterval.UntypedTo as IComparable) <= 0
                        ? otherInterval.UntypedTo
                        : numericalFeature.UntypedBorderValues.UntypedTo
            };
        }

        public ICoverable FindCovering(ICoverable other)
        {
            return FindCovering(other, null);
        }
    }

    public class Interval<T> : Interval, IInterval<T>
        where T : IComparable<T>
    {
        public T FromValue { get; set; }
        public T ToValue { get; set; }

        public int CompareTo(IInterval<T> other)
        {
            if (Equals(other))
                return 0;

            if ((UntypedFrom as IComparable).CompareTo(other.UntypedFrom) <= 0
                && (UntypedTo as IComparable).CompareTo(other.UntypedTo) >= 0)
                return 1;

            return -1;
        }

        public bool Equals(IInterval<T> other)
        {
            return UntypedFrom.Equals(other.UntypedFrom)
                && UntypedTo.Equals(other.UntypedTo);
        }
    }
}