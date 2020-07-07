using MLCS.Entities.Values.Default;
using System;
using System.Linq;

namespace MLCS.Entities.Features.Default
{
    public class CategorialFeatureFactory<T> : ICategorialFeatureFactory<T>
        where T : IComparable<T>
    {
        public ICategorialFeature<T> Create(int index, string name, bool isClass, T[] values, bool isSkipFeature = false)
        {
            var valuesList = values.Select(s => new CategorialValue<T>()
            {
                Value = s,
                UntypedValue = s
            }).AsEnumerable();

            return new CategorialFeature<T>()
            {
                Index = index,
                Name = name,
                IsClass = isClass,
                Values = valuesList,
                UntypedValues = valuesList,
                IsSkip = isSkipFeature
            };
        }
    }
}