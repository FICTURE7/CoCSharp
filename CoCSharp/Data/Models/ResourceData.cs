using CoCSharp.Csv;

namespace CoCSharp.Data.Models
{
    /// <summary>
    /// Defines data from logic/resources.csv.
    /// </summary>
    public class ResourceData : CsvData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceData"/> class.
        /// </summary>
        public ResourceData()
        {
            // Space
        }

        internal override int BaseDataID
        {
            get { return 3000000; }
        }

        /// <summary>
        /// Gets or sets the Name.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Gets or sets the TID.
        /// </summary>
        public string SWF { get; set; }
        /// <summary>
        /// Gets or sets the Collect effect.
        /// </summary>
        public string CollectEffect { get; set; }
        /// <summary>
        /// Gets or sets the Resource icon export name.
        /// </summary>
        public string ResourceIconExportName { get; set; }
        /// <summary>
        /// Gets or sets the Steal effect.
        /// </summary>
        public string StealEffect { get; set; }
        /// <summary>
        /// Gets or sets the Premium currency.
        /// </summary>
        public bool PremiumCurrency { get; set; }
        /// <summary>
        /// Gets or sets the HUD instance name.
        /// </summary>
        [CsvAlias("HudInstanceName")]
        public string HUDInstanceName { get; set; }
        /// <summary>
        /// Gets or sets the Cap full TID.
        /// </summary>
        public string CapFullTID { get; set; } 
        /// <summary>
        /// Gets or sets the Text red.
        /// </summary>
        public int TextRed { get; set; }
        /// <summary>
        /// Gets or sets the Text green.
        /// </summary>
        public int TextGreen { get; set; }
        /// <summary>
        /// Gets or sets the Text blue.
        /// </summary>
        public int TextBlue { get; set; }
        /// <summary>
        /// Gets or sets the War ref resource.
        /// </summary>
        public string WarRefResource { get; set; }
    }
}
