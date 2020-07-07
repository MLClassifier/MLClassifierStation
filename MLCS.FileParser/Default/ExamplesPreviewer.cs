using MLCS.Entities;
using MLCS.Entities.Vectors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MLCS.FileParser.Default
{
    public class ExamplesPreviewer : IExamplesPreviewer
    {
        public string Preview(IEnumerable<IVector> examples)
        {
            StringBuilder result = new StringBuilder();

            int count = examples.Count();
            int positivesCount = examples.Count(e => e.IsPositive());
            int negativesCount = examples.Count(e => !e.IsPositive());
            result.AppendFormat("Total count: {0}{1}", count, Environment.NewLine);

            if (positivesCount != 0 && negativesCount != 0)
            {
                result.AppendFormat("Positive examples count: {0}{1}", positivesCount, Environment.NewLine);
                result.AppendFormat("Negative examples count: {0}{1}", negativesCount, Environment.NewLine);
                result.AppendFormat("Positive examples percent: {0:0.##}%{1}", positivesCount * 100d / count, Environment.NewLine);
                result.AppendFormat("Negative examples percent: {0:0.##}%{1}", negativesCount * 100d / count, Environment.NewLine);
            }

            return result.ToString();
        }
    }
}