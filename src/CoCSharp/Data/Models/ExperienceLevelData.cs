using System;
using CoCSharp.Csv;

namespace CoCSharp.Data.Models
{
    /// <summary>
    /// Defines data from the logic/experience_levels.csv file.
    /// </summary>
    public class ExperienceLevelData : CsvData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExperienceLevelData"/> class.
        /// </summary>
        public ExperienceLevelData()
        {
            // Space
        }

        internal override int KindId => 11;

        // NOTE: This was generated from the experience_levels.csv using gen_csv_properties.py script.

        /// <summary>
        /// Gets or sets Name.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Gets or sets Exp points.
        /// </summary>
        public int ExpPoints { get; set; }
    }
}
