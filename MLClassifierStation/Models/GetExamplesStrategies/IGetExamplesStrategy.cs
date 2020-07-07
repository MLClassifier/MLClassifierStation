using MLCS.Entities.Vectors;
using MLCS.FileParser;
using MLCS.Services;
using System.Collections.Generic;

namespace MLClassifierStation.Models.GetExamplesStrategies
{
    public interface IGetExamplesStrategy
    {
        IEnumerable<IVector> GetExamples(IModelService modelService,
            IDataProvider examplesDataProvider, IExamplesProvider examplesProvider);
    }
}