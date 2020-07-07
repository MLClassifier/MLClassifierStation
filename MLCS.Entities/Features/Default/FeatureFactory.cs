using MLCS.Common.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace MLCS.Entities.Features.Default
{
    public class FeatureFactory : IFeatureFactory
    {
        private const int ATTRIBUTE_TYPE_LENGTH = 3;
        private const char SKIP_ATTRIBUTE = '-';
        private const char CLASS_ATTRIBUTE = '!';
        private const char NAME_SEPARATOR = ':';

        private IDictionary<FeatureType, ICreateFeatureStrategy> featureTypes;
        private INumericalFeatureFactory<int> intFeatureFactory;
        private INumericalFeatureFactory<double> doubleFeatureFactory;
        private ICategorialFeatureFactory<string> categorialFeatureFactory;

        public FeatureFactory(INumericalFeatureFactory<int> intFeatureFactory,
            INumericalFeatureFactory<double> doubleFeatureFactory,
            ICategorialFeatureFactory<string> categorialFeatureFactory)
        {
            this.intFeatureFactory = intFeatureFactory;
            this.doubleFeatureFactory = doubleFeatureFactory;
            this.categorialFeatureFactory = categorialFeatureFactory;

            featureTypes = new Dictionary<FeatureType, ICreateFeatureStrategy>();
            featureTypes.Add(FeatureType.Nominal, new CreateCategorialFeatureStrategy(categorialFeatureFactory));
            featureTypes.Add(FeatureType.Integer, new CreateIntegerFeatureStrategy(intFeatureFactory));
            featureTypes.Add(FeatureType.Double, new CreateDoubleFeatureStrategy(doubleFeatureFactory));
        }

        public IFeature Create(int index, string line)
        {
            bool isSkipFeature = line.TrimStart().StartsWith(SKIP_ATTRIBUTE.ToString());

            bool isClassFeature = line.TrimStart().StartsWith(CLASS_ATTRIBUTE.ToString());
            int posNameSeparator = line.IndexOf(NAME_SEPARATOR);
            string featureName = line.Substring(0, posNameSeparator).Trim();
            if (isClassFeature)
                featureName = featureName.TrimStart(CLASS_ATTRIBUTE);

            line = line.Substring(posNameSeparator + 1).Trim();
            FeatureType featureType = FeatureType.Nominal;

            if (!isClassFeature)
            {
                string featureTypeString = line.Substring(0, ATTRIBUTE_TYPE_LENGTH).Trim();
                featureType = Enum.GetValues(typeof(FeatureType))
                    .Cast<FeatureType>()
                    .FirstOrDefault(v => v.GetDescription() == featureTypeString);

                line = line.Substring(ATTRIBUTE_TYPE_LENGTH).Trim();
            }

            return featureTypes[featureType].Create(index, featureName, line, isClassFeature, isSkipFeature);
        }
    }

    public enum FeatureType
    {
        None,

        [Description("str")]
        Nominal,

        [Description("int")]
        Integer,

        [Description("dou")]
        Double
    }
}