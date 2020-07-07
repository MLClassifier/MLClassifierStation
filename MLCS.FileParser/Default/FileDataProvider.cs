using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MLCS.FileParser.Default
{
    public class FileDataProvider : IDataProvider
    {
        private string filePath;

        public FileDataProvider(string filePath)
        {
            this.filePath = filePath;
        }

        public IEnumerable<string> GetData()
        {
            if (!File.Exists(filePath))
                throw new Exception(string.Format("File {0} does not exist", filePath));

            return File.ReadLines(filePath, Encoding.ASCII);
        }
    }
}