using System;

namespace CoCSharp.Csv
{
    /// <summary>
    /// Represents a column in a <see cref="CsvDataTable"/>.
    /// </summary>
    public class CsvDataColumn
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="CsvDataColumn"/> class.
        /// </summary>
        public CsvDataColumn()
        {
            // Space
        }

        internal CsvDataColumn(CsvDataTable table)
        {
            if (table == null)
                throw new ArgumentNullException(nameof(table));

            _table = table;
        }
        #endregion

        #region Fields & Properties
        private readonly CsvDataTable _table;

        /// <summary>
        /// 
        /// </summary>
        public CsvDataTable Table => _table;

        /// <summary>
        /// 
        /// </summary>
        public int Level { get; }
        #endregion
    }
}
