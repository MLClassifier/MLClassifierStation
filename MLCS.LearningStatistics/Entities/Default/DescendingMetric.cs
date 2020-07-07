using System;

namespace MLCS.LearningStatistics.Entities.Default
{
    public abstract class DescendingMetric : BaseMetric
    {
        protected DescendingMetric() : base()
        {
        }

        protected DescendingMetric(Type type, double result)
            : base(type, result) { }

        public override int CompareTo(IMetric other)
        {
            return Utils.Compare(other.Result, Result);
        }
    }
}