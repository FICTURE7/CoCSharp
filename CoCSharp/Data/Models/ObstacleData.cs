using CoCSharp.Csv;
using System;

namespace CoCSharp.Data.Models
{
    /// <summary>
    /// Defines data from the logic/obstacles.csv file.
    /// </summary>
    public class ObstacleData : CsvData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ObstacleData"/> class.
        /// </summary>
        public ObstacleData()
        {
            // Space
        }

        internal override int BaseDataID
        {
            get { return 8000000; }
        }

        // NOTE: This was generated from the obstacles.csv using gen_csv_properties.py script.

        /// <summary>
        /// Gets or sets the Name of the obstacle.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Gets or sets Info TID.
        /// </summary>
        public string InfoTID { get; set; }
        /// <summary>
        /// Gets or sets SWF.
        /// </summary>
        public string SWF { get; set; }
        /// <summary>
        /// Gets or sets Export name.
        /// </summary>
        public string ExportName { get; set; }
        /// <summary>
        /// Gets or sets Export name base.
        /// </summary>
        public string ExportNameBase { get; set; }
        /// <summary>
        /// Gets or sets Export name base NPC.
        /// </summary>
        public string ExportNameBaseNpc { get; set; }

        /// <summary>
        /// Gets or sets Clear time.
        /// </summary>
        public TimeSpan ClearTime
        {
            get
            {
                return TimeSpan.FromSeconds(ClearTimeSeconds);
            }
            set
            {
                ClearTimeSeconds = (int)value.TotalSeconds;
            }
        }
        private int ClearTimeSeconds { get; set; }

        /// <summary>
        /// Gets or sets Width.
        /// </summary>
        public int Width { get; set; }
        /// <summary>
        /// Gets or sets Height.
        /// </summary>
        public int Height { get; set; }
        /// <summary>
        /// Gets or sets Resource.
        /// </summary>
        public string Resource { get; set; }
        /// <summary>
        /// Gets or sets Passable.
        /// </summary>
        public bool Passable { get; set; }
        /// <summary>
        /// Gets or sets Clear resource.
        /// </summary>
        public string ClearResource { get; set; }
        /// <summary>
        /// Gets or sets Clear cost.
        /// </summary>
        public int ClearCost { get; set; }
        /// <summary>
        /// Gets or sets Loot resource.
        /// </summary>
        public string LootResource { get; set; }
        /// <summary>
        /// Gets or sets Loot count.
        /// </summary>
        public int LootCount { get; set; }
        /// <summary>
        /// Gets or sets Clear effect.
        /// </summary>
        public string ClearEffect { get; set; }
        /// <summary>
        /// Gets or sets Pick up effect.
        /// </summary>
        public string PickUpEffect { get; set; }
        /// <summary>
        /// Gets or sets Respawn weight.
        /// </summary>
        public int RespawnWeight { get; set; }
        /// <summary>
        /// Gets or sets Is tombstone.
        /// </summary>
        public bool IsTombstone { get; set; }
        /// <summary>
        /// Gets or sets Tomb group.
        /// </summary>
        public int TombGroup { get; set; }
        /// <summary>
        /// Gets or sets Loot multiplier for version2.
        /// </summary>
        public int LootMultiplierForVersion2 { get; set; }

        //TODO: Might wanna change those stuff to TimeSpan as well.
        /// <summary>
        /// Gets or sets Appearance period hours.
        /// </summary>
        public int AppearancePeriodHours { get; set; }
        /// <summary>
        /// Gets or sets Min respawn time hours.
        /// </summary>
        public int MinRespawnTimeHours { get; set; }

        /// <summary>
        /// Gets or sets Spawn obstacle.
        /// </summary>
        public string SpawnObstacle { get; set; }
        /// <summary>
        /// Gets or sets Spawn radius.
        /// </summary>
        public int SpawnRadius { get; set; }

        //TODO: Might wanna change those stuff to TimeSpan as well.
        /// <summary>
        /// Gets or sets Spawn interval seconds.
        /// </summary>
        public int SpawnIntervalSeconds { get; set; }

        /// <summary>
        /// Gets or sets Spawn count.
        /// </summary>
        public int SpawnCount { get; set; }
        /// <summary>
        /// Gets or sets Max spawned.
        /// </summary>
        public int MaxSpawned { get; set; }
        /// <summary>
        /// Gets or sets Max lifetime spawns.
        /// </summary>
        public int MaxLifetimeSpawns { get; set; }
        /// <summary>
        /// Gets or sets Loot defense percentage.
        /// </summary>
        public int LootDefensePercentage { get; set; }
    }
}
