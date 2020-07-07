using MLCS.Entities.Features;
using MLCS.Entities.Values;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MLCS.Entities.Vectors.Default
{
    public class Vector : SortedList<IFeature, IValue>, IVector
    {
        public int CompareTo(IVector other)
        {
            if (Equals(other))
                return 0;

            bool covers = (this as ICoverable<IVector>).Covers(other);
            return covers ? 1 : -1;
        }

        public bool Covers(ICoverable other)
        {
            Vector impl = (Vector)other;
            return this.All(selector => impl.ContainsKey(selector.Key)
                                        && selector.Value.Covers(impl[selector.Key] as ICoverable));
        }

        public bool Equals(IVector other)
        {
            return this.All(selector => other.ContainsKey(selector.Key)
                                        && selector.Value.Equals(other[selector.Key]))
                && other.All(selector => ContainsKey(selector.Key)
                                        && selector.Value.Equals(other[selector.Key]));
        }

        public ICoverable FindCovering(ICoverable other)
        {
            Vector impl = (Vector)other;
            Vector result = new Vector();
            foreach (KeyValuePair<IFeature, IValue> keyValue in this)
            {
                if (keyValue.Key.IsClass)
                    continue;

                if (impl.ContainsKey(keyValue.Key) && !keyValue.Value.Covers(impl[keyValue.Key]))
                {
                    ICoverable covering = keyValue.Key is INumericalFeature
                        ? keyValue.Value.FindCovering(impl[keyValue.Key], keyValue.Key)
                        : keyValue.Value.FindCovering(impl[keyValue.Key]);
                    IValue value = covering as IValue;
                    result.Add(keyValue.Key, value);
                }
            }

            return result;
        }

        public ICoverable FindCovering(ICoverable other, IFeature feature)
        {
            return FindCovering(other);
        }

        public ICoverable FindCommon(ICoverable other)
        {
            Vector impl = (Vector)other;
            Vector result = new Vector();
            foreach (KeyValuePair<IFeature, IValue> keyValue in this)
            {
                if (keyValue.Key.IsClass)
                    continue;

                if (impl.ContainsKey(keyValue.Key))
                {
                    IValue value = keyValue.Value.FindCommon(impl[keyValue.Key]) as IValue;
                    result.Add(keyValue.Key, value);
                }
            }

            return result;
        }

        public string ToStringLine(IEnumerable<IFeature> features)
        {
            StringBuilder result = new StringBuilder();
            foreach (IFeature feature in features)
                result.AppendFormat("{0},",
                    Keys.Contains(feature)
                    ? this[feature].ToString()
                    : string.Empty);

            result.Remove(result.Length - 2, 1);
            return result.ToString();
        }

        public bool AddFeatureValuePair(IFeature feature, IValue value)
        {
            if (!feature.AllowsValue(value))
                return false;

            Add(feature, value);
            return true;
        }
    }

    public class Vector<T> : Vector, IVector<T>
        where T : IComparable<T>, ICoverable<T>
    {
    }
}