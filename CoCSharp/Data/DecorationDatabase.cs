using CoCSharp.Data.Csv;
using CoCSharp.Logic;
using System.Collections.Generic;
using System.Linq;

namespace CoCSharp.Data
{
    public class DecorationDatabase : Database
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
            while (!Table.EndOfFile)
            {
                var row = Table.ReadNextRow();

                var name = row.ReadRecordAsString(NameColumn);
                var buildResource = row.ReadRecordAsResource(BuildResourceColumn);
                var buildCost = row.ReadRecordAsInt(BuildCostColumn);
                var requiredLevel = row.ReadRecordAsInt(RequiredLevelColumn);
                var maxCount = row.ReadRecordAsInt(MaxCount);

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
