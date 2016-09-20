using System;
using System.Diagnostics;
using System.Reflection;

namespace CoCSharp.Csv
{
    /// <summary>
    /// Base class of <see cref="CsvDataRow{TCsvData}"/> class. 
    /// It is not indented to be used.
    /// </summary>
    [DebuggerTypeProxy(typeof(CsvDataRowDebugView))]
    [DebuggerDisplay("Name = {Name}")]
    public abstract class CsvDataRow
    {
        #region Constructors
        internal CsvDataRow()
        {
            // Space
        }
        #endregion

        #region Fields & Properties
        /// <summary>
        /// Gets the <see cref="CsvDataRowRef"/> of the <see cref="CsvDataRow"/>.
        /// </summary>
        public abstract CsvDataRowRef Ref { get; }

        /// <summary>
        /// Gets the ID of the <see cref="CsvDataRow"/>.
        /// </summary>
        public abstract int ID { get; }

        /// <summary>
        /// Gets the name of the <see cref="CsvDataRow"/>.
        /// </summary>
        public abstract string Name { get; }
        #endregion

        #region Methods
        // Proxy method to call CsvDataRow<>.Add.
        // This is needed for CsvConvert to add data to rows.
        internal abstract void ProxyAdd(object data);

        // Needed to get the CsvDataCollectionDebugView to work correctly.
        internal abstract object[] GetAllData();

        // Returns an instance of the CsvDataRow<> class with the specified type as generic parameter.
        internal static CsvDataRow CreateInstance(Type type, string name)
        {
            const BindingFlags BINDING_FLAGS = BindingFlags.CreateInstance | BindingFlags.Instance | BindingFlags.Public;

            var ntype = typeof(CsvDataRow<>).MakeGenericType(type);
            return (CsvDataRow)Activator.CreateInstance(ntype, bindingAttr: BINDING_FLAGS, args: new object[] { name }, culture: null, binder: null);
        }
        #endregion
    }
}
