// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using MLCS.Entities.Features;
using MLCS.Entities.Vectors;
using MLCS.Entities.Values;
using System;
using System.Collections.Generic;
using System.Linq;
using TinyCsvParser.Model;
using TinyCsvParser.TypeConverter;

namespace TinyCsvParser.Mapping
{
    public interface ICsvMapping
    {
        void MapFeatures(IEnumerable<IFeature> features);

        CsvMappingResult Map(TokenizedRow values);

        CsvMappingResult DoesMap(TokenizedRow values);
    }

    public class CsvMapping : ICsvMapping
    {
        private class IndexToFeatureMapping
        {
            public int ColumnIndex { get; set; }

            public ICsvFeatureMapping FeatureMapping { get; set; }

            public override string ToString()
            {
                return string.Format("IndexToFeatureMapping (ColumnIndex = {0}, FeatureMapping = {1}", ColumnIndex, FeatureMapping);
            }
        }

        private readonly ITypeConverterProvider typeConverterProvider;
        private readonly List<IndexToFeatureMapping> csvFeatureMappings;
        private readonly IVectorFactory vectorFactory;
        private readonly IValueFactory valueFactory;

        public CsvMapping(ITypeConverterProvider typeConverterProvider, IVectorFactory vectorFactory, IValueFactory valueFactory)
        {
            this.typeConverterProvider = typeConverterProvider;
            this.vectorFactory = vectorFactory;
            this.valueFactory = valueFactory;
            csvFeatureMappings = new List<IndexToFeatureMapping>();
        }

        public void MapFeatures(IEnumerable<IFeature> features)
        {
            foreach (IFeature feature in features)
                MapFeature(feature.Index, feature);
        }

        protected CsvFeatureMapping MapFeature(int columnIndex, IFeature feature)
        {
            return MapFeature(columnIndex, feature, typeConverterProvider.Resolve(feature.FeatureType));
        }

        protected CsvFeatureMapping MapFeature(int columnIndex, IFeature feature, ITypeConverter typeConverter)
        {
            if (csvFeatureMappings.Any(x => x.ColumnIndex == columnIndex))
            {
                throw new InvalidOperationException(string.Format("Duplicate mapping for column index {0}", columnIndex));
            }

            var featureMapping = new CsvFeatureMapping(feature, typeConverter, valueFactory);

            AddFeatureMapping(columnIndex, featureMapping);

            return featureMapping;
        }

        private void AddFeatureMapping(int columnIndex, CsvFeatureMapping featureMapping)
        {
            var indexToFeatureMapping = new IndexToFeatureMapping
            {
                ColumnIndex = columnIndex,
                FeatureMapping = featureMapping
            };

            csvFeatureMappings.Add(indexToFeatureMapping);
        }

        public CsvMappingResult Map(TokenizedRow values)
        {
            IVector vector = vectorFactory.CreateVector();

            for (int pos = 0; pos < csvFeatureMappings.Count; pos++)
            {
                var indexToFeatureMapping = csvFeatureMappings[pos];

                var columnIndex = indexToFeatureMapping.ColumnIndex;

                if (columnIndex >= values.Tokens.Length)
                    return GenerateOutOfRangeErrorResult(columnIndex, values.Index);

                var value = values.Tokens[columnIndex];

                if (!indexToFeatureMapping.FeatureMapping.TryMapValue(vector, value))
                    return GenerateTypeMappingErrorResult(columnIndex, values.Index, value);
            }

            return new CsvMappingResult()
            {
                RowIndex = values.Index,
                Result = vector
            };
        }

        public CsvMappingResult DoesMap(TokenizedRow values)
        {
            for (int pos = 0; pos < csvFeatureMappings.Count; pos++)
            {
                var indexToFeatureMapping = csvFeatureMappings[pos];

                var columnIndex = indexToFeatureMapping.ColumnIndex;

                if (columnIndex >= values.Tokens.Length)
                    return GenerateOutOfRangeErrorResult(columnIndex, values.Index);

                var value = values.Tokens[columnIndex];

                if (!indexToFeatureMapping.FeatureMapping.DoesMap(value))
                    return GenerateTypeMappingErrorResult(columnIndex, values.Index, value);
            }

            return new CsvMappingResult()
            {
                RowIndex = values.Index,
                Result = null
            };
        }

        private CsvMappingResult GenerateOutOfRangeErrorResult(int columnIndex, int rowIndex)
        {
            return new CsvMappingResult()
            {
                RowIndex = rowIndex,
                Error = new CsvMappingError()
                {
                    ColumnIndex = columnIndex,
                    Value = string.Format("Feature {0} is Out Of Range", columnIndex)
                }
            };
        }

        private CsvMappingResult GenerateTypeMappingErrorResult(int columnIndex, int rowIndex, string value)
        {
            return new CsvMappingResult()
            {
                RowIndex = rowIndex,
                Error = new CsvMappingError
                {
                    ColumnIndex = columnIndex,
                    Value = string.Format("Feature {0} with Value '{1}' cannot be mapped. Please check feature type and allowed values.", columnIndex, value)
                }
            };
        }

        public override string ToString()
        {
            var csvFeatureMappingsString = string.Join(", ", csvFeatureMappings.Select(x => x.ToString()));

            return string.Format("CsvMapping (TypeConverterProvider = {0}, Mappings = {1})", typeConverterProvider, csvFeatureMappingsString);
        }
    }
}