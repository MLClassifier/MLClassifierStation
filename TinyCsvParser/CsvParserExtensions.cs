// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using TinyCsvParser.Mapping;
using TinyCsvParser.Model;

namespace TinyCsvParser
{
    public static class CsvParserExtensions
    {
        public static ParallelQuery<CsvMappingResult> ReadFromFile(this CsvParser csvParser, string fileName, Encoding encoding)
        {
            if (fileName == null)
            {
                throw new ArgumentNullException("fileName");
            }

            var lines = File
                .ReadLines(fileName, encoding)
                .Select((line, index) => new Row(index, line));

            return csvParser.Parse(lines);
        }

        public static ParallelQuery<CsvMappingResult> ReadFromString(this CsvParser csvParser, CsvReaderOptions csvReaderOptions, string csvData)
        {
            var lines = csvData
                .Split(csvReaderOptions.NewLine, StringSplitOptions.None)
                .Select((line, index) => new Row(index, line));

            return csvParser.Parse(lines);
        }

        public static ParallelQuery<CsvMappingResult> ReadFromStrings(this CsvParser csvParser, IEnumerable<string> examplesData, Encoding encoding)
        {
            var lines = examplesData
                .Select((line, index) => new Row(index, line));

            return csvParser.Parse(lines);
        }

        public static int CheckFormatFromStrings(this CsvParser csvParser, IEnumerable<string> examplesData, Encoding encoding)
        {
            var lines = examplesData
                .Select((line, index) => new Row(index, line));

            return csvParser.CheckFormat(lines);
        }
    }
}