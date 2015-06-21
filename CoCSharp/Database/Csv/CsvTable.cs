using System.IO;

namespace CoCSharp.Database.Csv
{
    public class CsvTable
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
    }
}
