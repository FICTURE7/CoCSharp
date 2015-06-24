using CoCSharp.Databases.Csv;
using CoCSharp.Logic;
using System.Collections.Generic;
using System.Linq;

namespace CoCSharp.Databases
{
    public class DecorationDatabase : BaseDatabase
    {
        public const int ID = 18000000;

        #region Columns
        private const int NameColumn = 0;
        private const int BuildResourceColumn = 6;
        private const int BuildCostColumn = 7;
        private const int RequiredLevelColumn = 8;
        private const int MaxCount = 9;
        #endregion

        public DecorationDatabase(string path) : base(path)
        {
            this.Decorations = new List<Decoration>();
        }

        public List<Decoration> Decorations { get; set; }

        public bool TryGetDecoration(int id, out Decoration decoration)
        {
            decoration = Decorations.Where(d => d.ID == id).FirstOrDefault();
            return decoration == null ? false : true;
        }

        public bool TryGetDecoration(string name, out Decoration decoration)
        {
            decoration = Decorations.Where(d => d.Name == name).FirstOrDefault();
            return decoration == null ? false : true;
        }

        public override void LoadDatabase()
        {
            var id = ID;
            while (!CsvTable.EndOfFile)
            {
                var row = CsvTable.ReadNextRow();

                var name = row.GetRecord(NameColumn);
                var buildResource = row.GetRecordAsResource(BuildResourceColumn);
                var buildCost = row.GetRecordAsInt(BuildCostColumn);
                var requiredLevel = row.GetRecordAsInt(RequiredLevelColumn);
                var maxCount = row.GetRecordAsInt(MaxCount);

                var decoration = new Decoration(id);
                decoration.Name = name;
                decoration.BuildResource = buildResource;
                decoration.BuildCost = buildCost;
                decoration.RequiredLevel = requiredLevel;
                decoration.MaxCount = maxCount;

                Decorations.Add(decoration);
                id++;
            }
        }
    }
}
