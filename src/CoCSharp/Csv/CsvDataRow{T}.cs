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

            _data = new TCsvData[table.Columns.Count];
            _kindId = CsvData.GetKindID(typeof(TCsvData));
        }
        #endregion

        #region Fields & Properties
        // CsvDataCollectionRef pointing to this.
        internal CsvDataRowRef<TCsvData> _ref;
        // Kind ID of the TCsvData.
        private readonly int _kindId;
        // List containing the TCsvData in the row.
        private TCsvData[] _data;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="columnIndex"></param>
        /// <returns></returns>
        public new TCsvData this[int columnIndex]
        {
            get
            {
                return (TCsvData)GetDataAtColumnIndex(columnIndex);
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException(nameof(value));

                SetDataAtColumnIndex(columnIndex, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
        public new TCsvData this[CsvDataColumn column]
        {
            get
            {
                CheckColumn(column);

                return (TCsvData)GetDataAtColumnIndex(column.DataLevel);
            }
            set
            {
                CheckColumn(column);

                SetDataAtColumnIndex(column.DataLevel, value);
            }
        }

        /// <summary>
        /// Gets an array of <typeparamref name="TCsvData"/> that are in this <see cref="CsvDataRow{TCsvData}"/>.
        /// </summary>
        public new TCsvData[] DataArray => _data;

        /// <summary>
        /// 
        /// </summary>
        public override CsvDataRowRef Ref => _ref;
        #endregion

        #region Methods
        /// <summary/>
        protected override CsvData GetDataAtColumnIndex(int columnIndex)
        {
            if (columnIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(columnIndex), "Column index must be non-negative.");

            if (columnIndex >= Table.Columns.Count)
                return null;

            return _data[columnIndex];
        }

        /// <summary/>
        protected override void SetDataAtColumnIndex(int index, CsvData data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));
            if (index < 0)
                throw new ArgumentOutOfRangeException(nameof(index), "Index must be non-negative");

            var tdata = (TCsvData)data;
            if (Table.Columns.Count > _data.Length)
                Array.Resize(ref _data, Table.Columns.Count);

            if (index >= _data.Length)
            {
                throw new InvalidOperationException("Table does not contain the specified column.");
                //var count = index - _data.Count;
                //for (int i = 0; i < count; i++)
                //    _data.Add(default(TCsvData));

                //_data.Add(tdata);
            }
            else
            {
                _data[index] = tdata;
            }
        }

        /// <summary/>
        protected override CsvData[] GetDataArray()
        {
            return Array.ConvertAll(_data, (input) => (CsvData)input);
        }
        #endregion
    }
}
