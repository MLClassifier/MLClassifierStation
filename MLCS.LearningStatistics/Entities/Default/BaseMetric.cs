using System;

namespace MLCS.LearningStatistics.Entities.Default
{
    public abstract class BaseMetric : IMetric
    {
        public double? Result { get; protected set; }

        public Type Type { get; private set; }

        protected BaseMetric()
        {
            Type = GetType();
        }

        protected BaseMetric(Type type, double result)
        {
            Type = type;
            Result = result;
        }

        public double? Calculate(IConfusionMatrix confusionMatrix)
        {
            OnCalculate(confusionMatrix);

            if (Result.HasValue && Double.IsNaN(Result.Value))
                Result = null;

            return Result;
        }

        protected internal abstract double? OnCalculate(IConfusionMatrix confusionMatrix);

        public abstract int CompareTo(IMetric other);
    }
}