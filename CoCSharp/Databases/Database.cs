using CoCSharp.Databases.Csv;
using Newtonsoft.Json.Linq;
using SevenZip;
using System;
using System.IO;
using System.Net;
using System.Linq;

namespace CoCSharp.Databases
{
    public abstract class Database
    {
        public Database(string path)
        {
            this.CsvTable = new CsvTable(path);
            this.CsvPath = path;
        }

        public string CsvPath { get; set; }
        public virtual CsvTable CsvTable { get; set; }

        public virtual void ReloadDatabase()
        {
            CsvTable = new CsvTable(CsvPath);
            LoadDatabase();
        }

        public abstract void LoadDatabase();
    }
}
