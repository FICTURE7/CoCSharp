namespace CoCSharp.Data
{
    /// <summary>
    /// Defines data from globals.csv.
    /// </summary>
    public class GlobalData : CoCData
    {
        /// <summary>
        /// Initalizes a new instance of the <see cref="GlobalData"/> class.
        /// </summary>
        public GlobalData()
        {
            // Space
        }

        public string Name { get; set; }
        public int NumberValue { get; set; }
        public bool BooleanValue { get; set; }
        public string TextValue { get; set; }
        public int NumberArray { get; set; }
        public string StringArray { get; set; }
        public string AltStringArray { get; set; }

    }
}
