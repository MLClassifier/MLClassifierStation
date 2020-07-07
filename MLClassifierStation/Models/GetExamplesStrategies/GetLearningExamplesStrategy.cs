using MLCS.Entities.Vectors;
using MLCS.FileParser;
using MLCS.Services;
using System.Collections.Generic;

namespace MLClassifierStation.Models.GetExamplesStrategies
{
    public class GetLearningExamplesStrategy : IGetExamplesStrategy
    {
        public IEnumerable<IVector> GetExamples(IModelService modelService,
            IDataProvider examplesDataProvider, IExamplesProvider examplesProvider)
        {
            IEnumerable<IVector> examples = examplesProvider.GetExamples(examplesDataProvider, modelService.Features);

            modelService.LearningExamples = examples;

            return examples;
        }
    }
}