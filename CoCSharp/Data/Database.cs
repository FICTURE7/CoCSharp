using CoCSharp.Data.Csv;
using System;
using System.IO;

namespace CoCSharp.Data
{
    public abstract class Database
    {
        public Database(string path)
        {
            TablePath = path;
            Table = new CsvTable(path);
        }

        public string TablePath { get; set; }
        public virtual CsvTable Table { get; set; }

        public void ReloadDatabase()
        {
            Table = new CsvTable(TablePath);
            LoadDatabase();
        }

        public abstract void LoadDatabase();
    }
}
