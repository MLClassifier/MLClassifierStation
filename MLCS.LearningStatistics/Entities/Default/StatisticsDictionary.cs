using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MLCS.LearningStatistics.Entities.Default
{
    public class StatisticsDictionary : IStatisticsDictionary
    {
        private IDictionary<Type, IStatistics> statistics;

        public StatisticsDictionary()
        {
            statistics = new Dictionary<Type, IStatistics>();
        }

        public IStatistics this[Type key]
        {
            get { return statistics[key]; }
            set { statistics[key] = value; }
        }

        public int Count => statistics.Count;

        public bool IsReadOnly => statistics.IsReadOnly;

        public ICollection<Type> Keys => statistics.Keys;

        public ICollection<IStatistics> Values => statistics.Values;

        public void Add(Type key, IStatistics value)
        {
            statistics.Add(key, value);
        }

        public void Add(KeyValuePair<Type, IStatistics> item)
        {
            statistics.Add(item);
        }

        public void Clear()
        {
            statistics.Clear();
        }

        public int CompareBy(IStatisticsDictionary other, IMetric metric)
        {
            Type metricType = metric.GetType();
            IStatistics statistics = this[metricType];
            IStatistics otherStatistics = other[metricType];

            if (statistics == null || otherStatistics == null)
                throw new ArgumentException();

            return statistics.CompareTo(otherStatistics);
        }

        public bool Contains(KeyValuePair<Type, IStatistics> item)
        {
            return statistics.Contains(item);
        }

        public bool ContainsKey(Type key)
        {
            return statistics.ContainsKey(key);
        }

        public void CopyTo(KeyValuePair<Type, IStatistics>[] array, int arrayIndex)
        {
            CopyTo(array, arrayIndex);
        }

        public bool Remove(Type key)
        {
            return statistics.Remove(key);
        }

        public bool Remove(KeyValuePair<Type, IStatistics> item)
        {
            return statistics.Remove(item);
        }

        public bool TryGetValue(Type key, out IStatistics value)
        {
            return statistics.TryGetValue(key, out value);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return statistics.GetEnumerator();
        }

        IEnumerator<KeyValuePair<Type, IStatistics>> IEnumerable<KeyValuePair<Type, IStatistics>>.GetEnumerator()
        {
            return statistics.GetEnumerator();
        }

        public bool Export(string fileName)
        {
            try
            {
                using StreamWriter outputFile = new StreamWriter(fileName);
                foreach (KeyValuePair<Type, IStatistics> statistics in this)
                    outputFile.WriteLine(statistics.Value);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public string Preview()
        {
            StringBuilder result = new StringBuilder();
            foreach (KeyValuePair<Type, IStatistics> statistics in this)
                result.AppendLine(statistics.Value.ToString());

            return result.ToString();
        }
    }
}