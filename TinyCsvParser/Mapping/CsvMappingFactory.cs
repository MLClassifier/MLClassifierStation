using MLCS.Entities.Vectors;
using MLCS.Entities.Values;
using TinyCsvParser.TypeConverter;

namespace TinyCsvParser.Mapping
{
    public interface ICsvMappingFactory
    {
        ICsvMapping Create();
    }

    public class CsvMappingFactory : ICsvMappingFactory
    {
        private ITypeConverterProvider typeConverterProvider;
        private IVectorFactory vectorFactory;
        private IValueFactory valueFactory;

        public CsvMappingFactory(ITypeConverterProvider typeConverterProvider, IVectorFactory vectorFactory, IValueFactory valueFactory)
        {
            this.typeConverterProvider = typeConverterProvider;
            this.vectorFactory = vectorFactory;
            this.valueFactory = valueFactory;
        }

        public ICsvMapping Create()
        {
            return new CsvMapping(typeConverterProvider, vectorFactory, valueFactory);
        }
    }
}