using CoCSharp.Csv;

namespace CoCSharp.Data.Models
{
    /// <summary>
    /// Defines data from the logic/globals.csv file.
    /// </summary>
    public class GlobalData : CsvData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GlobalData"/> class.
        /// </summary>
        public GlobalData()
        {
            // Space
        }

        // NOTE: This was generated from the globals.csv using gen_csv_properties.py script.
        internal override int KindId => 14;

        /// <summary>
        /// Gets or sets the Name of the global.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Gets or sets Number value.
        /// </summary>
        public int NumberValue { get; set; }
        /// <summary>
        /// Gets or sets Boolean value.
        /// </summary>
        public bool BooleanValue { get; set; }
        /// <summary>
        /// Gets or sets Text value.
        /// </summary>
        public string TextValue { get; set; }
        /// <summary>
        /// Gets or sets Number array.
        /// </summary>
        public int NumberArray { get; set; }
        /// <summary>
        /// Gets or sets Alt number array.
        /// </summary>
        public int AltNumberArray { get; set; }
        /// <summary>
        /// Gets or sets String array.
        /// </summary>
        public string StringArray { get; set; }
        /// <summary>
        /// Gets or sets Alt string array.
        /// </summary>
        public string AltStringArray { get; set; }
    }
}
