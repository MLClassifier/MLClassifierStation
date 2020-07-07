using System;

namespace MLCS.Entities.Model.Default
{
    public class LearningParameter : ILearningParameter
    {
        public string Name { get; set; }
        public Type Type { get; set; }
        public object Value { get; set; }
    }
}