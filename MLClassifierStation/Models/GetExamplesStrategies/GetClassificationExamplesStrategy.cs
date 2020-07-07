using MLCS.Entities.Vectors;
using MLCS.FileParser;
using MLCS.Services;
using System.Collections.Generic;
using System.Linq;

namespace MLClassifierStation.Models.GetExamplesStrategies
{
    public class GetClassificationExamplesStrategy : IGetExamplesStrategy
    {
        public IEnumerable<IVector> GetExamples(IModelService modelService,
            IDataProvider examplesDataProvider, IExamplesProvider examplesProvider)
        {
            IEnumerable<IVector> examples = examplesProvider.GetExamples(examplesDataProvider, modelService.Features.Where(a => !a.IsClass));

            modelService.ClassificationExamples = examples;

            return examples;
        }
    }
}