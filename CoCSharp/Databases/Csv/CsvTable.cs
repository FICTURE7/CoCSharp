using System;
using System.IO;

namespace CoCSharp.Databases.Csv
{
    public class CsvTable : IDisposable
    {
        public CsvTable(string path)
        {
            this.CsvFile = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            this.Reader = new StreamReader(CsvFile);
            
            Reader.GotoLine(2);
        }

        public bool EndOfFile { get { return Reader.EndOfStream; } }
        public FileStream CsvFile { get; set; }

        private StreamReader Reader { get; set; }

        public CsvRow ReadNextRow()
        {
            var row = Reader.ReadLine().Split(',');
            return new CsvRow(row);
        }

        public void Dispose()
        {
            CsvFile.Dispose();
            Reader.Dispose();
        }
    }
}
