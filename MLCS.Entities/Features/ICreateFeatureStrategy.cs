namespace MLCS.Entities.Features
{
    public interface ICreateFeatureStrategy
    {
        IFeature Create(int index, string name, string line, bool isClass = false, bool isSkipFeature = false);
    }
}