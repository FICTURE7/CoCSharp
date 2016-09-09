using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace CoCSharp.Csv
{
    /// <summary>
    /// Base class of <see cref="CsvDataCollection{TCsvData}"/> class. It is not indented to be used.
    /// </summary>
    [DebuggerTypeProxy(typeof(CsvDataCollectionDebugView))]
    [DebuggerDisplay("Name = {Name}")]
    public abstract class CsvDataCollection : ICollection<CsvData>
    {
        internal CsvDataCollection()
        {
            // Space
        }

        /// <summary>
        /// Gets the <see cref="CsvDataCollectionRef"/> of the <see cref="CsvDataCollection"/>.
        /// </summary>
        public abstract CsvDataCollectionRef CollectionRef { get; }

        /// <summary>
        /// Gets the ID of the <see cref="CsvDataCollection"/>.
        /// </summary>
        public abstract int ID { get; }

        /// <summary>
        /// Gets the name of the <see cref="CsvDataCollection"/>.
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// Gets the number of <see cref="CsvData"/> in the <see cref="CsvDataCollection"/>.
        /// </summary>
        public abstract int Count { get; }

        private static readonly BindingFlags s_flags = BindingFlags.CreateInstance | BindingFlags.Instance | BindingFlags.Public;
        // Returns an instance of the CsvDataRow<> class with the specified type as generic parameter.
        internal static CsvDataCollection CreateInstance(Type type, string name)
        {
            var ntype = typeof(CsvDataCollection<>).MakeGenericType(type);
            return (CsvDataCollection)Activator.CreateInstance(ntype, bindingAttr: s_flags, args: new object[] { name }, culture: null, binder: null);
        }

        #region ICollection<CsvData>
        bool ICollection<CsvData>.IsReadOnly => false;

        public abstract void Add(CsvData data);

        public abstract void Insert(int level, CsvData data);

        public abstract void Clear();

        public abstract bool Contains(CsvData data);

        public abstract void CopyTo(CsvData[] array, int arrayIndex);

        public abstract bool Remove(CsvData data);

        public abstract IEnumerator<CsvData> GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        #endregion
    }
}
