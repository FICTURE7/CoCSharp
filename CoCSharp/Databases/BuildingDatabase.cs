using CoCSharp.Databases.Csv;
using CoCSharp.Logic;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CoCSharp.Databases
{
    public class BuildingDatabase : BaseDatabase
    {
        public const int ID = 1000000;

        #region Columns
        // could use header rows name instead of const indexes
        private const int NameColumn = 0;
        private const int BuildTimeDayColumn = 10;
        private const int BuildTimeHourColumn = 11;
        private const int BuildTimeSecondColumn = 12;
        private const int BuildResourceColumn = 14;
        private const int BuildCostColumn = 15;
        private const int TownHallLevelColumn = 16;
        private const int HitpointColumn = 37;
        #endregion

        public BuildingDatabase(string path) : base(path)
        {
            this.Buildings = new List<Building>();
        }

        public List<Building> Buildings { get; set; }

        public bool TryGetBuilding(int id, int level, out Building building)
        {
            building = Buildings.Where(t => t.ID == id && t.Level == level).FirstOrDefault();
            return building == null ? false : true;
        }

        public bool TryGetBuilding(string name, int level, out Building building)
        {
            building = Buildings.Where(t => t.Name == name && t.Level == level).FirstOrDefault();
            return building == null ? false : true;
        }

        public override void LoadDatabase()
        {
            //TODO: Implement other building's data.
            var id = ID - 1;
            var level = 0; // lvls starts from 0
            var parentRow = (CsvRow)null;
            while (!CsvTable.EndOfFile)
            {
                var row = CsvTable.ReadNextRow();

                var name = row.GetRecord(NameColumn);

                var days = row.GetRecordAsInt(BuildTimeDayColumn);
                var hours = row.GetRecordAsInt(BuildTimeHourColumn);
                var seconds = row.GetRecordAsInt(BuildTimeSecondColumn);

                var buildResource = row.GetRecordAsResource(BuildResourceColumn);
                var buildCost = row.GetRecordAsInt(BuildCostColumn);
                var townHallLevel = row.GetRecordAsInt(TownHallLevelColumn);
                var hitpoints = row.GetRecordAsInt(HitpointColumn);

                if (!string.IsNullOrEmpty(name)) // must be a lvl 1 building
                {
                    parentRow = row;
                    level = 0;
                    id++;
                }
                else // building lvl must be > 1
                {
                    name = parentRow.GetRecord(NameColumn);
                    level++;
                }

                var building = new Building(id, level);
                building.Name = parentRow.GetRecord(NameColumn);
                building.BuildTime = new TimeSpan(days, hours, 0, seconds);
                building.BuildCost = buildCost;
                building.BuildResource = parentRow.GetRecordAsResource(BuildResourceColumn);
                building.TownHallLevel = townHallLevel;
                building.Hitpoints = hitpoints;

                Buildings.Add(building);
            }
        }

        public static bool IsBuilding(int id)
        {
            return ID < id && id < 2000000; // locales
        }
    }
}
