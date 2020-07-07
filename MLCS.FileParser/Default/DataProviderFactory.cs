namespace MLCS.FileParser.Default
{
    public class DataProviderFactory : IDataProviderFactory
    {
        public IDataProvider Create(string filePath)
        {
            return new FileDataProvider(filePath);
        }
    }
}