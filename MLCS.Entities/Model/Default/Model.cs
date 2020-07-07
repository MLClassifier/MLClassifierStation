using MLCS.Entities.Vectors;
using System.Collections.Generic;

namespace MLCS.Entities.Model.Default
{
    public abstract class Model : IModel
    {
        public IEnumerable<ILearningParameter> Parameters { get; private set; }

        public string LearningLog { get; private set; }

        public abstract ClassType Classify(IVector example);

        public abstract string Preview();

        public abstract bool Export(string fileName);

        public abstract bool Import(string fileName);
    }
}