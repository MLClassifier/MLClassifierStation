using System;

namespace MLCS.Entities.Vectors
{
    public interface IVectorFactory
    {
        IVector CreateVector();
    }

    public interface IVectorFactory<T> : IVectorFactory
        where T : IComparable<T>
    {
        IVector<T> Create();
    }
}