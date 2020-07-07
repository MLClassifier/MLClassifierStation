using MLCS.Entities.Features;
using MLCS.Entities.Vectors;
using MLCS.Entities.Values;
using TinyCsvParser.TypeConverter;

namespace TinyCsvParser.Mapping
{
    public class CsvFeatureMapping : ICsvFeatureMapping
    {
        private ITypeConverter typeConverter;
        private IFeature feature;
        private IValueFactory valueFactory;

        public CsvFeatureMapping(IFeature feature, ITypeConverter typeConverter, IValueFactory valueFactory)
        {
            this.feature = feature;
            this.typeConverter = typeConverter;
            this.valueFactory = valueFactory;
        }

        public bool DoesMap(string value)
        {
            dynamic convertedValue;
            if (!typeConverter.TryConvert(value, out convertedValue))
                return false;

            IValue featureValue = valueFactory.Create(convertedValue);
            return feature.AllowsValue(featureValue);
        }

        public bool TryMapValue(IVector vector, string value)
        {
            dynamic convertedValue;
            if (!typeConverter.TryConvert(value, out convertedValue))
                return false;

            IValue featureValue = valueFactory.Create(convertedValue);
            return vector.AddFeatureValuePair(feature, featureValue);
        }
    }
}