using CoCSharp.Database.Csv;
namespace CoCSharp.Database
{
    public abstract class BaseDatabase
    {
        public BaseDatabase(string path)
        {
            this.CsvTable = new CsvTable(path);
        }

        public virtual CsvTable CsvTable { get; set; }

        public virtual void ReloadDatabase()
        {
            CsvTable = new CsvTable(CsvTable.CsvFile.Name);
            LoadDatabase();
        }

        public abstract void LoadDatabase();
    }
}
