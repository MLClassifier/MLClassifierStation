namespace MLCS.Entities.Features
{
    public interface IFeatureFactory
    {
        IFeature Create(int index, string line);
    }
}