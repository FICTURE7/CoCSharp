using System;

namespace CoCSharp.Csv
{
    /// <summary>
    /// Represents a Clash of Clans .csv file.
    /// </summary>
    public abstract class CsvData
    {
        internal const int MaxIndex = 1000000;

        /// <summary>
        /// Gets the data ID of the <see cref="CsvData"/>.
        /// </summary>
        [CsvIgnore]
        public int ID
        {
            get
            {
                return Index + BaseDataID;
            }
        }

        /// <summary>
        /// Gets or sets the text ID of the <see cref="CsvData"/>.
        /// </summary>
        public string TID { get; set; }

        private int _level;
        /// <summary>
        /// Gets the level of <see cref="CsvData"/>.
        /// </summary>
        [CsvIgnore]
        public int Level
        {
            get
            {
                return _level;
            }
            //TODO: Make this internal.
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException("value", "Level must be non-negative.");

                _level = value;
            }
        }

        private int _index;
        // Index of the CsvData in the CSV file. Aka parent ID.
        [CsvIgnore]
        internal int Index
        {
            get
            {
                return _index;
            }
            set
            {
                // This exception can occur if there is more than 1000000 child object.
                if (value < 0 || value > MaxIndex)
                    throw new ArgumentOutOfRangeException("value");

                _index = value;
            }
        }

        // Base data ID, Example: 1000000 for BuildingData
        [CsvIgnore]
        internal abstract int BaseDataID { get; }

        [CsvIgnore]
        internal string ArgumentExceptionMessage
        {
            get
            {
                return "value must be between " + BaseDataID + " and " + (BaseDataID + MaxIndex) + ".";
            }
        }

        // Determines if the specified dataId is invalid (not between valid range).
        internal bool InvalidDataID(int dataId)
        {
            return dataId < BaseDataID || dataId > BaseDataID + MaxIndex;
        }
    }
}
