using System;

namespace MLCS.Common.Utils
{
    public static class Math
    {
        public static T Max<T>(T x, T y) where T : IComparable<T>
        {
            return x.CompareTo(y) >= 0 ? x : y;
        }

        public static T Min<T>(T x, T y) where T : IComparable<T>
        {
            return x.CompareTo(y) <= 0 ? x : y;
        }
    }
}