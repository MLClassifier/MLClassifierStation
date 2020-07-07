using System;

namespace MLCS.Entities.Model
{
    public interface ILearningParameter
    {
        string Name { get; set; }
        Type Type { get; set; }
        object Value { get; set; }
    }
}