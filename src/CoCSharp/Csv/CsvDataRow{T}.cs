using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace CoCSharp.Csv
{
    /// <summary>
    /// Represents a collection of <see cref="CsvData"/>.
    /// </summary>
    /// <typeparam name="TCsvData">Type of <see cref="CsvData"/>.</typeparam>
    [DebuggerDisplay("Name = {Name}")]
    public class CsvDataRow<TCsvData> : CsvDataRow where TCsvData : CsvData, new()
    {
        #region Constructors
        internal CsvDataRow(CsvDataTable table, string name) : base(table, name)
        {
            // Table should of type CsvDataTable<>.
            Debug.Assert(table.GetType().GetGenericTypeDefinition() == typeof(CsvDataTable<>));

            _data = new List<TCsvData>(16);
            _kindId = CsvData.GetKindID(typeof(TCsvData));
        }
        #endregion

        #region Fields & Properties
        // CsvDataCollectionRef pointing to this.
        internal CsvDataRowRef<TCsvData> _ref;
        // Kind ID of the TCsvData.
        private readonly int _kindId;
        // List containing the TCsvData in the row.
        private readonly List<TCsvData> _data;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="columnIndex"></param>
        /// <returns></returns>
        public new TCsvData this[int columnIndex]
        {
            get
            {
                return _data[columnIndex];
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override CsvDataRowRef Ref => _ref;
        #endregion

        #region Methods
        internal override object[] GetAllData()
        {
            return _data.ToArray();
        }
        #endregion
    }
}
