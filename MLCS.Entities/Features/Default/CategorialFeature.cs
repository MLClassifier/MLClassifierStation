using MLCS.Entities.Values;
using System;
using System.Collections.Generic;

namespace MLCS.Entities.Features.Default
{
    public class CategorialFeature<T> : ICategorialFeature<T>
        where T : IComparable<T>
    {
        public string Name { get; set; }
        public int Index { get; set; }
        public bool IsClass { get; set; }
        public bool IsSkip { get; set; }

        public IEnumerable<IValue<T>> Values { get; set; }
        public IEnumerable<IValue> UntypedValues { get; set; }

        public Type FeatureType => typeof(T);

        public bool AllowsValue(IValue value)
        {
            foreach (IValue<T> allowedValue in Values)
                if (value.Equals(allowedValue))
                    return true;

            return false;
        }

        public int CompareTo(object obj)
        {
            return Index.CompareTo((obj as IFeature).Index);
        }
    }
}