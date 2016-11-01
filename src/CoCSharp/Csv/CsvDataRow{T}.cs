using System;
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
        public new TCsvData[] DataArray => GetResizedDataArray();

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

            if (columnIndex >= Table.Columns.Count || columnIndex >= _data.Length)
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
            if (index >= Table.Columns.Count)
                throw new InvalidOperationException("Table does not contain the specified column.");

            if (data != null)
            {
                var tdata = (TCsvData)data;
                // Resize the _data array to be size of the number of columns in the table.
                if (Table.Columns.Count > _data.Length)
                    Array.Resize(ref _data, Table.Columns.Count);

                tdata._ref = Ref;
                _data[index] = tdata;
            }
            else
            {
                var tdata = _data[index];
                tdata._ref = null;
            }
        }

        /// <summary/>
        protected override CsvData[] GetDataArray() => Array.ConvertAll(GetResizedDataArray(), (element) => (CsvData)element);

        private TCsvData[] GetResizedDataArray()
        {
            Debug.Assert(_data.Length <= Table.Columns.Count, "Row width was larger than number of columns.");

            if (_data.Length < Table.Columns.Count)
                Array.Resize(ref _data, Table.Columns.Count);

            return _data;
        }

        internal void UpdateRefs()
        {
            for (int i = 0; i < _data.Length; i++)
            {
                var tdata = _data[i];
                if (tdata != null)
                    tdata._ref = Ref;
            }
        }
        #endregion
    }
}
