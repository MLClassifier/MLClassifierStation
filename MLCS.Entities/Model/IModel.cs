using MLCS.Entities.Vectors;
using System.Collections.Generic;

namespace MLCS.Entities.Model
{
    public interface IModel : IExportable
    {
        IEnumerable<ILearningParameter> Parameters { get; }
        string LearningLog { get; }

        ClassType Classify(IVector example);

        string Preview();
    }
}