using MLCS.Entities.Values;
using System;

namespace MLCS.Entities.Features.Numerical
{
    public class NumericalFeature<T> : INumericalFeature<T>
        where T : IComparable<T>
    {
        public string Name { get; set; }
        public int Index { get; set; }
        public bool IsClass { get; set; }
        public bool IsSkip { get; set; }

        public IInterval<T> BorderValues { get; set; }
        public IInterval UntypedBorderValues { get; set; }

        public Type FeatureType => typeof(T);

        public bool AllowsValue(IValue value)
        {
            return BorderValues.Covers(value);
        }

        public int CompareTo(object obj)
        {
            return Index.CompareTo((obj as IFeature).Index);
        }
    }
}