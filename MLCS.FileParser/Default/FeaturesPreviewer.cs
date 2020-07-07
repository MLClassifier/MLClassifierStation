using MLCS.Entities.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MLCS.FileParser.Default
{
    public class FeaturesPreviewer : IFeaturesPreviewer
    {
        public string Preview(IEnumerable<IFeature> features)
        {
            StringBuilder result = new StringBuilder();

            result.AppendFormat("Total count: {0}{1}", features.Count(), Environment.NewLine);
            result.AppendFormat("Numerical features count: {0}{1}",
                features.Count(a => a.GetType().GetInterfaces().Contains(typeof(INumericalFeature))),
                Environment.NewLine);
            result.AppendFormat("Categorial features count: {0}{1}",
                features.Count(a => a.GetType().GetInterfaces().Contains(typeof(ICategorialFeature))),
                Environment.NewLine);

            var categorialFeatures = features.Where(a => a.GetType() is ICategorialFeature);
            foreach (var categorialFeature in categorialFeatures)
            {
                result.AppendFormat("- feature No {0} values count - {1}{2}",
                    categorialFeature.Index,
                    (categorialFeature as ICategorialFeature).UntypedValues.Count(),
                    Environment.NewLine);
            }

            return result.ToString();
        }
    }
}