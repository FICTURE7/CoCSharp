using System;
using System.Diagnostics;
using System.Reflection;

namespace CoCSharp.Csv
{
    /// <summary>
    /// Represents a row in the <see cref="CsvDataRowCollection"/>.
    /// Base class of <see cref="CsvDataRow{TCsvData}"/> class. 
    /// </summary>
    [DebuggerDisplay("RowName = {Name}")]
    public abstract class CsvDataRow
    {
        #region Constructors
        internal CsvDataRow(CsvDataTable table, string name)
        {
            if (table == null)
                throw new ArgumentNullException(nameof(table));
            if (name == null)
                throw new ArgumentNullException(nameof(name));

            _table = table;
            _name = name;
        }
        #endregion

        #region Fields & Properties
        // Name of the CsvDataRow.
        private readonly string _name;
        // Table to which this CsvDataRow belongs.
        internal CsvDataTable _table;

        /// <summary>
        /// Gets the name of the <see cref="CsvDataRow"/>.
        /// </summary>
        public string Name => _name;

        /// <summary>
        /// Gets the <see cref="CsvDataTable"/> which contains this <see cref="CsvDataRow"/>.
        /// </summary>
        public CsvDataTable Table => _table;

        /// <summary>
        /// Gets the ID of the <see cref="CsvDataRow"/>; returns -1 if not in a <see cref="CsvDataTable"/>.
        /// </summary>
        public int Id => Ref == null ? -1 : Ref.Id;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="columnIndex"></param>
        /// <returns></returns>
        public CsvData this[int columnIndex]
        {
            get
            {
                return GetDataAtColumnIndex(columnIndex);
            }
            set
            {
                SetDataAtColumnIndex(columnIndex, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
        public CsvData this[CsvDataColumn column]
        {
            get
            {
                CheckColumn(column);

                return GetDataAtColumnIndex(column.DataLevel);
            }
            set
            {
                CheckColumn(column);

                SetDataAtColumnIndex(column.DataLevel, value);
            }
        }

        /// <summary>
        /// Gets an array of <see cref="CsvData"/> that are in this <see cref="CsvDataRow"/>.
        /// </summary>
        public CsvData[] DataArray => GetDataArray();

        /// <summary>
        /// Gets the <see cref="CsvDataRowRef"/> of the <see cref="CsvDataRow"/>.
        /// </summary>
        public abstract CsvDataRowRef Ref { get; }
        #endregion

        #region Methods
        /// <summary/>
        protected abstract CsvData GetDataAtColumnIndex(int index);

        /// <summary/>
        protected abstract void SetDataAtColumnIndex(int index, CsvData data);

        /// <summary/>
        protected abstract CsvData[] GetDataArray();

        /// <summary/>
        protected void CheckColumn(CsvDataColumn column)
        {
            if (column == null)
                throw new ArgumentNullException(nameof(column));
            if (column.Table != Table)
                throw new ArgumentException("Column is not in the same CsvDataTable.", nameof(column));
        }

        // Returns an instance of the CsvDataRow<> class with the specified type as generic parameter.
        internal static CsvDataRow CreateInternal(Type type, CsvDataTable table, string name)
        {
            const BindingFlags BINDING_FLAGS = BindingFlags.CreateInstance | BindingFlags.Instance | BindingFlags.NonPublic;

            var ntype = typeof(CsvDataRow<>).MakeGenericType(type);
            return (CsvDataRow)Activator.CreateInstance(ntype, bindingAttr: BINDING_FLAGS, args: new object[] { table, name }, binder: null, culture: null);
        }
        #endregion
    }
}
