using MLCS.Entities.Vectors;

namespace TinyCsvParser.Mapping
{
    public interface ICsvFeatureMapping
    {
        bool TryMapValue(IVector vector, string value);

        bool DoesMap(string value);
    }
}