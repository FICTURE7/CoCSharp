using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace CoCSharp.Csv
{
    /// <summary>
    /// Represents a collection of <see cref="CsvDataColumn"/>.
    /// Base class of <see cref="CsvDataColumnCollection{TCsvData}"/>.
    /// </summary>
    public abstract class CsvDataColumnCollection : ICollection<CsvDataColumn>
    {
        internal CsvDataColumnCollection(Type csvDataType, CsvDataTable table)
        {
            _table = table;
        }

        #region Fields & Properties
        private readonly CsvDataTable _table;

        bool ICollection<CsvDataColumn>.IsReadOnly => false;
        internal CsvDataTable Table => _table;

        /// <summary>
        /// Gets the number of <see cref="CsvDataColumn"/> in the <see cref="CsvDataColumnCollection"/>.
        /// </summary>
        public abstract int Count { get; }
        #endregion

        #region Methods
        /// <summary>
        /// 
        /// </summary>
        /// <param name="column"></param>
        public abstract void Add(CsvDataColumn column);

        /// <summary>
        /// 
        /// </summary>
        public abstract void Clear();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
        public abstract bool Contains(CsvDataColumn column);

        /// <summary>
        /// Copies the entire collection into an existing array, starting at a specified index within the array. 
        /// </summary>
        /// <param name="array">An array of <see cref="CsvDataColumn" /> objects to copy the collection into.></param>
        /// <param name="arrayIndex"></param>
        public abstract void CopyTo(CsvDataColumn[] array, int arrayIndex);

        /// <summary>
        /// Removes the specified <see cref="CsvDataColumn"/> from the collection.
        /// </summary>
        /// <param name="column">The <see cref="CsvDataColumn"/> to remove.</param>
        /// <returns><c>true</c> if success; otherwise; <c>false</c>.</returns>
        public abstract bool Remove(CsvDataColumn column);

        /// <summary>
        /// Returns an enumerator that iterates through the <see cref="CsvDataColumnCollection"/>.
        /// </summary>
        /// <returns>An enumerator that iterates through the <see cref="CsvDataColumnCollection"/>.</returns>
        public abstract IEnumerator<CsvDataColumn> GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        // Creates a new instance of the generic CsvDataColumnCollection<> with 'type' as the generic
        // parameter and returns it.
        internal static CsvDataColumnCollection CreateInternal(Type type, CsvDataTable table)
        {
            const BindingFlags BINDING_FLAGS = BindingFlags.CreateInstance | BindingFlags.Instance | BindingFlags.NonPublic;

            var ntype = typeof(CsvDataColumnCollection<>).MakeGenericType(type);
            return (CsvDataColumnCollection)Activator.CreateInstance(ntype, bindingAttr: BINDING_FLAGS, args: new object[] { table }, binder: null, culture: null);
        }
        #endregion
    }
}
