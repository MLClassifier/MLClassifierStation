using System;

namespace MLCS.Entities.Vectors.Default
{
    public class VectorFactory : IVectorFactory
    {
        public IVector CreateVector()
        {
            return new Vector();
        }
    }

    public class VectorFactory<T> : VectorFactory, IVectorFactory<T>
        where T : IComparable<T>, ICoverable<T>
    {
        public IVector<T> Create()
        {
            return new Vector<T>();
        }
    }
}