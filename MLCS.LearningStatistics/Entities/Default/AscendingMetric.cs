using System;

namespace MLCS.LearningStatistics.Entities.Default
{
    public abstract class AscendingMetric : BaseMetric
    {
        protected AscendingMetric()
            : base() { }

        protected AscendingMetric(Type type, double result)
            : base(type, result) { }

        public override int CompareTo(IMetric other)
        {
            return Utils.Compare(Result, other.Result);
        }
    }
}