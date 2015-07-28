using CoCSharp.Data.Csv;
using System;

namespace CoCSharp.Data
{
    /// <summary>
    /// Defines data from buildings.csv.
    /// </summary>
    public class BuildingData : VillageObjectData
    {
        private const string BuildingNameColumnName = "          ";

        /// <summary>
        /// Initializes a new instance of the BuildingData class.
        /// </summary>
        public BuildingData() { }

        [CsvProperty(BuildingNameColumnName)]  // its named like this in buildings.csv
        public override string Name { get; set; }
        [CsvProperty("TID_Instructor")]
        public string TIDInstructor { get; set; }
        public int InstructorWeight { get; set; }
        public string BuildingClass { get; set; }
        public string ExportName { get; set; }
        public string ExportNameNpc { get; set; }
        public string ExportNameConstruction { get; set; }
        public int BuildTimeD { get; set; }
        public int BuildTimeH { get; set; }
        public int BuildTimeM { get; set; }
        public int BuildTimeS { get; set; }
    }
}
