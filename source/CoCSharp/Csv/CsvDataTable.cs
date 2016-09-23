using System;
using System.Diagnostics;

namespace CoCSharp.Csv
{
    /// <summary>
    /// Represents a table of <see cref="CsvData"/>.
    /// Base class of the <see cref="CsvDataTable{TCsvData}"/> class.
    /// </summary>
    [DebuggerTypeProxy(typeof(CsvDataTableDebugView))]
    public abstract class CsvDataTable
    {
        #region Constants
        /// <summary>
        /// Value indicating the maximum width of a <see cref="CsvDataTable"/>. This field is constant.
        /// </summary>
        public const int MaxWidth = 999999;
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
            // Creates an instance of CsvDataColumnCollection<csvDataType>.
            _columns = CsvDataColumnCollection.CreateInternal(csvDataType);
            // Creates an instance of CsvDataRowCollection<csvDataType>.
            _rows = CsvDataRowCollection.CreateInternal(csvDataType, this);
        }
        #endregion

        #region Fields & Properties
        private readonly Type _csvDataType;
        private readonly CsvDataColumnCollection _columns;
        private readonly CsvDataRowCollection _rows;
        // KindID of the type of CsvData the CsvDataTable is storing.
        // Aka the index at which the table is in CsvDataTableCollection.
        private readonly int _kindId;

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

        // Needed to get the CsvDataRowDebugView to work correctly.
        internal abstract object[] GetAllColumns();

        // Needed for the CsvDataCollectionRef to work properly.
        internal abstract CsvDataRow GetByIndex(int index);

        // Returns a new instance of the CsvDataTable<> class with the specified type as generic parameter.
        internal static CsvDataTable CreateInternal(Type type)
        {
            var ntype = typeof(CsvDataTable<>).MakeGenericType(type);
            return (CsvDataTable)Activator.CreateInstance(ntype);
        }

        public static CsvDataTable Create(Type csvDataType)
        {
            if (csvDataType == null)
                throw new ArgumentNullException(nameof(csvDataType));
            if (csvDataType.IsAbstract || !typeof(CsvData).IsAssignableFrom(csvDataType))
                throw new ArgumentException("Specified type must be non-abstract and assignable from type of CsvData.", nameof(csvDataType));

            return CreateInternal(csvDataType);
        }

        public static CsvDataTable<TCsvData> Create<TCsvData>() where TCsvData : CsvData, new()
        {
            return new CsvDataTable<TCsvData>();
        }
        #endregion
    }
}
