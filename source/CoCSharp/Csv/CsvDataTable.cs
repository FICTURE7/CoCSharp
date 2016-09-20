using System;
using System.Diagnostics;

namespace CoCSharp.Csv
{
    /// <summary>
    /// Represents a table of <see cref="CsvData"/>.
    /// Base <see cref="CsvDataTable{TCsvData}"/> class. It is not intended to be used.
    /// </summary>
    [DebuggerTypeProxy(typeof(CsvDataTableDebugView))]
    public abstract class CsvDataTable
    {
        /// <summary>
        /// Value indicating the maximum width of a <see cref="CsvDataTable"/>. This field is constant.
        /// </summary>
        public const int MaxWidth = 999999;

        internal CsvDataTable()
        {
            // Space
        }

        internal abstract int KindID { get; }

        // Proxy method to call CsvDataTable<>.Add.
        // This is needed for CsvConvert to add rows to returning table.
        internal abstract void ProxyAdd(object row);

        // Needed to get the CsvDataRowDebugView to work correctly.
        internal abstract object[] GetAllColumns();

        // Needed for the CsvDataCollectionRef to work properly.
        internal abstract CsvDataRow GetByIndex(int index);

        // Returns a new instance of the CsvDataTable<> class with the specified type as generic parameter.
        internal static CsvDataTable CreateInstance(Type type)
        {
            var ntype = typeof(CsvDataTable<>).MakeGenericType(type);
            return (CsvDataTable)Activator.CreateInstance(ntype);
        }
    }
}
