﻿// Copyright (c) Philipp Wagner. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.IO;
using System.Linq;

namespace TinyCsvParser.Tokenizer.RFC4180
{
    public class RFC4180Tokenizer : ITokenizer
    {
        private Reader reader;

        public RFC4180Tokenizer(Options options)
        {
            this.reader = new Reader(options);
        }

        public string[] Tokenize(string input)
        {
            using var stringReader = new StringReader(input);
            return reader.ReadTokens(stringReader)
                         .Select(token => token.Content)
                         .ToArray();
        }

        public override string ToString()
        {
            return string.Format("RFC4180Tokenizer (Reader = {0})", reader);
        }
    }
}