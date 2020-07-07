using MLCS.Entities.Vectors;
using System.Collections.Generic;

namespace MLCS.FileParser
{
    public interface IExamplesPreviewer
    {
        string Preview(IEnumerable<IVector> examples);
    }
}