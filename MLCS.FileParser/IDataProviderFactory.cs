namespace MLCS.FileParser
{
    public interface IDataProviderFactory
    {
        IDataProvider Create(string filePath);
    }
}