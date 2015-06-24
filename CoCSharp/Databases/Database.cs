using CoCSharp.Databases.Csv;
using CoCSharp.Logic;

namespace CoCSharp.Databases
{
    public abstract class BaseDatabase
    {
        public BaseDatabase(string path)
        {
            this.CsvTable = new CsvTable(path);
            this.Path = path;
        }

        public string Path { get; set; }
        public virtual CsvTable CsvTable { get; set; }

        public virtual void ReloadDatabase()
        {
            CsvTable = new CsvTable(CsvTable.CsvFile.Name);
            LoadDatabase();
        }

        public abstract void LoadDatabase();
    }
}
