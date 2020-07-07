using MLCS.Entities;
using System;
using System.Collections.Generic;

namespace MLCS.LearningStatistics.Entities
{
    public interface IStatisticsDictionary : IDictionary<Type, IStatistics>, IExportable
    {
        int CompareBy(IStatisticsDictionary other, IMetric metric);

        string Preview();
    }
}