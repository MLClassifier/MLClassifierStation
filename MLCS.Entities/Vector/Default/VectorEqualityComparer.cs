namespace MLCS.Entities.Vectors.Default
{
    public class VectorEqualityComparer : IVectorEqualityComparer
    {
        public bool Equals(IVector x, IVector y)
        {
            foreach (var keyValuePair in x)
            {
                if (!y.Contains(keyValuePair))
                    return false;
                if (!x[keyValuePair.Key].Equals(y[keyValuePair.Key]))
                    return false;
            }

            foreach (var keyValuePair in y)
            {
                if (!x.Contains(keyValuePair))
                    return false;
                if (!y[keyValuePair.Key].Equals(x[keyValuePair.Key]))
                    return false;
            }

            return true;
        }

        public int GetHashCode(IVector obj)
        {
            return obj.Count;
        }
    }
}