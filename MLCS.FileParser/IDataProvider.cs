using System.Collections.Generic;

namespace MLCS.FileParser
{
    public interface IDataProvider
    {
        IEnumerable<string> GetData();
    }
}