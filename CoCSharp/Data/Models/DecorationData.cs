using CoCSharp.Csv;

namespace CoCSharp.Data.Models
{
    /// <summary>
    /// Defines data from the logic/decos.csv file.
    /// </summary>
    public class DecorationData : CsvData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DecorationData"/> class.
        /// </summary>
        public DecorationData()
        {
            // Space
        }

        internal override int BaseDataID
        {
            get { return 18000000; }
        }

        // NOTE: This was generated from the decos.csv using gen_csv_properties.py script.

        /// <summary>
        /// Gets or sets the Name of the decoration.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Gets or sets InfoTID.
        /// </summary>
        public string InfoTID { get; set; }
        /// <summary>
        /// Gets or sets SWF asset location.
        /// </summary>
        public string SWF { get; set; }
        /// <summary>
        /// Gets or sets Export name.
        /// </summary>
        public string ExportName { get; set; }
        /// <summary>
        /// Gets or sets Export name construction.
        /// </summary>
        public string ExportNameConstruction { get; set; }
        /// <summary>
        /// Gets or sets Build resource.
        /// </summary>
        public string BuildResource { get; set; }
        /// <summary>
        /// Gets or sets Build cost.
        /// </summary>
        public int BuildCost { get; set; }
        /// <summary>
        /// Gets or sets Required experience level.
        /// </summary>
        public int RequiredExpLevel { get; set; }
        /// <summary>
        /// Gets or sets Max count.
        /// </summary>
        public int MaxCount { get; set; }
        /// <summary>
        /// Gets or sets Width.
        /// </summary>
        public int Width { get; set; }
        /// <summary>
        /// Gets or sets Height.
        /// </summary>
        public int Height { get; set; }
        /// <summary>
        /// Gets or sets Icon.
        /// </summary>
        public string Icon { get; set; }
        /// <summary>
        /// Gets or sets Export name base.
        /// </summary>
        public string ExportNameBase { get; set; }
        /// <summary>
        /// Gets or sets Export name base NPC.
        /// </summary>
        public string ExportNameBaseNpc { get; set; }
        /// <summary>
        /// Gets or sets Export name base war.
        /// </summary>
        public string ExportNameBaseWar { get; set; }
    }
}
