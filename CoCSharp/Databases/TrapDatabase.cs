using CoCSharp.Databases.Csv;
using CoCSharp.Logic;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CoCSharp.Databases
{
    public class TrapDatabase : BaseDatabase
    {
        public const int ID = 12000000;

        #region Columns
        private const int NameColumn = 0;
        private const int BuildResourceColumn = 22;
        private const int BuildTimeDayColumn = 23;
        private const int BuildTimeHourColumn = 24;
        private const int BuildTimeMinuteColumn = 25;
        private const int BuildCostColumn = 26;
        private const int RearmCostColumn = 27;
        private const int TownHallLevelColumn = 28;
        #endregion

        public TrapDatabase(string path) : base(path)
        {
            this.Traps = new List<Trap>();
        }

        public List<Trap> Traps { get; set; }

        public bool TryGetTrap(int id, int level, out Trap trap)
        {
            trap = Traps.Where(t => t.Level == level && t.ID == id).FirstOrDefault();
            return trap == null ? false : true;
        }

        public bool TryGetTrap(string name, int level, out Trap trap)
        {
            trap = Traps.Where(t => t.Name == name && t.Level == level).FirstOrDefault();
            return trap == null ? false : true;
        }

        public override void LoadDatabase()
        {
            var id = ID - 1;
            var level = 0;
            var parentRow = (CsvRow)null;
            while (!CsvTable.EndOfFile)
            {
                var row = CsvTable.ReadNextRow();

                var name = row.ReadRecordAsString(NameColumn);
                var buildResource = row.ReadRecordAsResource(BuildResourceColumn);

                var days = row.ReadRecordAsInt(BuildTimeDayColumn);
                var hours = row.ReadRecordAsInt(BuildTimeHourColumn);
                var minutes = row.ReadRecordAsInt(BuildTimeMinuteColumn);

                var buildCost = row.ReadRecordAsInt(BuildCostColumn);
                var rearmCost = row.ReadRecordAsInt(RearmCostColumn);
                var townHallLevel = row.ReadRecordAsInt(TownHallLevelColumn);

                if (!string.IsNullOrEmpty(name))
                {
                    parentRow = row;
                    level = 0;
                    id++;
                }
                else
                {
                    name = parentRow.ReadRecordAsString(NameColumn);
                    level++;
                }

                var trap = new Trap(id, level);
                trap.Name = parentRow.ReadRecordAsString(NameColumn);
                trap.BuildResource = parentRow.ReadRecordAsResource(BuildResourceColumn);
                trap.BuildTime = new TimeSpan(days, hours, minutes, 0);
                trap.BuildCost = buildCost;
                trap.RearmCost = rearmCost;
                trap.TownHallLevel = townHallLevel;

                Traps.Add(trap);
            }
        }
    }
}
