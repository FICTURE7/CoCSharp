using System;
using System.Diagnostics;
using System.Reflection;

namespace CoCSharp.Csv
{
    /// <summary>
    /// Represents a row in the <see cref="CsvDataRowCollection"/>.
    /// Base class of <see cref="CsvDataRow{TCsvData}"/> class. 
    /// </summary>
    [DebuggerTypeProxy(typeof(CsvDataRowDebugView))]
    [DebuggerDisplay("Name = {Name}")]
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
        private readonly string _name;
        private readonly CsvDataTable _table;

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
        public int ID => Ref == null ? -1 : Ref.ID;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="columnIndex"></param>
        /// <returns></returns>
        public CsvData this[int columnIndex]
        {
            get
            {
                return null;
            }
        }

        /// <summary>
        /// Gets the <see cref="CsvDataRowRef"/> of the <see cref="CsvDataRow"/>.
        /// </summary>
        public abstract CsvDataRowRef Ref { get; }
        #endregion

        #region Methods
        // Needed to get the CsvDataCollectionDebugView to work correctly.
        internal abstract object[] GetAllData();

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
