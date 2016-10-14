using System;
using System.Diagnostics;

namespace CoCSharp.Csv
{
    /// <summary>
    /// Represents a table of <see cref="CsvData"/>.
    /// Base class of the <see cref="CsvDataTable{TCsvData}"/> class.
    /// </summary>
    public abstract class CsvDataTable
    {
        #region Constants
        /// <summary>
        /// Value indicating the maximum width of a <see cref="CsvDataTable"/>. This field is constant.
        /// </summary>
        public const int MaxHeight = 999999;
        #endregion

        #region Constructors
        // Prevent the user from initiating a CsvDataTable from here.
        // Therefore forcing them to initiate a strongly typed CsvDataTable<>.
        internal CsvDataTable(Type csvDataType)
        {
            Debug.Assert(csvDataType != null, nameof(csvDataType) + " was null.");
            Debug.Assert(csvDataType.IsAbstract || typeof(CsvData).IsAssignableFrom(csvDataType));

            _csvDataType = csvDataType;
            _kindId = CsvData.GetKindID(csvDataType);
            // Creates an instance of CsvDataColumnCollection.
            _columns = new CsvDataColumnCollection(csvDataType, this);
            // Creates an instance of CsvDataRowCollection<csvDataType>.
            _rows = CsvDataRowCollection.CreateInternal(csvDataType, this);
        }
        #endregion

        #region Fields & Properties
        // Type of TCsvData.
        private readonly Type _csvDataType;
        // Collection of CsvDataColumn.
        private readonly CsvDataColumnCollection _columns;
        // Collection of CsvDataRow<TCsvData>.
        private readonly CsvDataRowCollection _rows;
        // KindID of the type of CsvData the CsvDataTable is storing.
        private readonly int _kindId;

        /// <summary>
        /// Gets the Kind ID of the <see cref="CsvData"/> type stored in this table.
        /// 
        /// <para>
        /// This value will also be the index of where this <see cref="CsvDataTable"/> will be
        /// in a <see cref="CsvDataTableCollection"/>.
        /// </para>
        /// </summary>
        internal int KindID => _kindId;

        /// <summary>
        /// Gets the <see cref="CsvDataColumnCollection"/> associated with this <see cref="CsvDataTable"/>.
        /// </summary>
        public CsvDataColumnCollection Columns => _columns;

        /// <summary>
        /// Gets the <see cref="CsvDataColumnCollection"/> associated with this <see cref="CsvDataTable"/>.
        /// </summary>
        public CsvDataRowCollection Rows => _rows;
        #endregion

        #region Methods
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public CsvDataRow NewRow(string name)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));

            var row = CsvDataRow.CreateInternal(_csvDataType, this, name);
            return row;
        }

        // Returns a new instance of the CsvDataTable<> class with the specified type as generic parameter.
        internal static CsvDataTable CreateInternal(Type type)
        {
            var ntype = typeof(CsvDataTable<>).MakeGenericType(type);
            return (CsvDataTable)Activator.CreateInstance(ntype);
        }
        #endregion
    }
}
