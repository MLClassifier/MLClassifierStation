using System;
using System.Collections.Generic;

namespace MLCS.Common.Utils
{
    public static class Partition
    {
        // https://stackoverflow.com/questions/17763664/given-a-list-of-length-n-select-k-random-elements-using-c-sharp
        public static void PartialShuffle<T>(IList<T> source, int count, Random random)
        {
            if (source.Count <= count) return;

            for (int i = 0; i < count; i++)
            {
                int index = i + random.Next(source.Count - i);
                T tmp = source[index];
                source[index] = source[i];
                source[i] = tmp;
            }
        }
    }
}