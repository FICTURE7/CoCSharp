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
    public abstract class CsvDataCollection
    {
        internal CsvDataCollection()
        {
            // Space
        }

        /// <summary>
        /// Gets the ID of the <see cref="CsvDataCollection"/>.
        /// </summary>
        public abstract int ID { get; }

        /// <summary>
        /// Gets the name of the <see cref="CsvDataCollection"/>.
        /// </summary>
        public abstract string Name { get; }

        // Proxy method to call CsvDataRow<>.Add.
        // This is needed for CsvConvert to add data to rows.
        internal abstract void ProxyAdd(object data);

        // Needed to get the CsvDataCollectionDebugView to work correctly.
        internal abstract object[] GetAllData();

        private static BindingFlags s_flags = BindingFlags.CreateInstance | BindingFlags.Instance | BindingFlags.Public;
        // Returns an instance of the CsvDataRow<> class with the specified type as generic parameter.
        internal static CsvDataCollection CreateInstance(Type type, string name)
        {
            var ntype = typeof(CsvDataCollection<>).MakeGenericType(type);
            return (CsvDataCollection)Activator.CreateInstance(ntype, bindingAttr: s_flags, args: new object[] { name }, culture: null, binder: null);
        }

        internal sealed class CsvDataCollectionDebugView
        {
            public CsvDataCollectionDebugView(CsvDataCollection collection)
            {
                if (collection == null)
                    throw new ArgumentNullException("collection");

                _collection = collection;
            }

            private readonly CsvDataCollection _collection;

            public int ID
            {
                get
                {
                    return _collection.ID;
                }
            }

            public string Name
            {
                get
                {
                    return _collection.Name;
                }
            }

            [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
            public object[] Data
            {
                get
                {
                    return _collection.GetAllData();
                }
            }
        }
    }

    /// <summary>
    /// Represents a collection of <see cref="CsvData"/>.
    /// </summary>
    /// <typeparam name="TCsvData">Type of <see cref="CsvData"/>.</typeparam>
    [DebuggerDisplay("Name = {Name}, Count = {Count}")]
    public class CsvDataCollection<TCsvData> : CsvDataCollection, ICollection<TCsvData> where TCsvData : CsvData, new()
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="CsvDataCollection{TCsvData}"/> class with the specified
        /// name.
        /// </summary>
        /// <param name="name">Name of the <see cref="CsvDataCollection{TCsvData}"/>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is null.</exception>
        public CsvDataCollection(string name)
        {
            if (name == null)
                throw new ArgumentNullException("name");

            _name = name;
            _data = new List<TCsvData>(16);
            _kindId = CsvData.GetInstance<TCsvData>().KindID;
            _columnIndex = -1;
        }
        #endregion

        #region Fields & Properties
        // Kind ID of the TCsvData.
        private readonly int _kindId;
        // List containing the TCsvData in the row.
        private readonly List<TCsvData> _data;

        bool ICollection<TCsvData>.IsReadOnly { get { return false; } }

        /// <summary>
        /// Gets the <typeparamref name="TCsvData"/> with the specified level.
        /// </summary>
        /// <param name="level">Level of the <typeparamref name="TCsvData"/>.</param>
        /// <returns>The <typeparamref name="TCsvData"/> with the specified level.</returns>
        public TCsvData this[int level]
        {
            get
            {
                if (level < 0 || level > Count)
                    throw new ArgumentOutOfRangeException("level", "level must be non-negative and less than Count.");

                return _data[level];
            }
        }

        // Row index of the CsvDataCollection{T}.
        // This value is set by the CsvDataRow the collection was added to.
        internal int _columnIndex;
        /// <summary>
        /// Gets the ID of the <see cref="CsvDataCollection{TCsvData}"/>.
        /// </summary>
        public override int ID
        {
            get
            {
                Debug.Assert(_columnIndex >= -1, "_columnIndex was less than -1.");
                if (_columnIndex == -1)
                    throw new InvalidOperationException("CsvDataCollection must be in a CsvDataRow to have an ID.");

                return (_kindId * InternalConstants.IDBase) + _columnIndex;
            }
        }

        private readonly string _name;
        /// <summary>
        /// Gets the name of the <see cref="CsvDataCollection{TCsvData}"/>.
        /// </summary>
        public override string Name
        {
            get
            {
                return _name;
            }
        }

        /// <summary>
        /// Gets the number of <typeparamref name="TCsvData"/> in the <see cref="CsvDataCollection{TCsvData}"/>
        /// </summary>
        public int Count
        {
            get
            {
                return _data.Count;
            }
        }

        /// <summary>
        /// Gets the highest level <typeparamref name="TCsvData"/> in the <see cref="CsvDataCollection{TCsvData}"/>.
        /// </summary>
        public TCsvData Max
        {
            get
            {
                var index = _data.Count - 1;
                if (index < 0)
                    return null;

                return _data[index];
            }
        }
        #endregion

        #region Methods
        internal override void ProxyAdd(object item)
        {
            Add((TCsvData)item);
        }

        internal override object[] GetAllData()
        {
            return _data.ToArray();
        }

        /// <summary>
        /// Adds the specified <typeparamref name="TCsvData"/> at the end of the <see cref="CsvDataCollection{TCsvData}"/>.
        /// </summary>
        /// <param name="data"><typeparamref name="TCsvData"/> to add to the <see cref="CsvDataCollection{TCsvData}"/>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="data"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="data"/> is already in a <see cref="CsvDataCollection{TCsvData}"/>.</exception>
        public void Add(TCsvData data)
        {
            if (data == null)
                throw new ArgumentNullException("data");
            if (data._level != -1)
                throw new ArgumentException("data is already in a CsvDataCollection.", "data");

            // Set level of data to that of the last index.
            data._level = _data.Count;
            _data.Add(data);
        }

        /// <summary>
        /// Removes all the <typeparamref name="TCsvData"/> in the <see cref="CsvDataCollection{TCsvData}"/>.
        /// </summary>
        public void Clear()
        {
            for (int i = 0; i < _data.Count; i++)
            {
                _data[i]._level = -1;
                _data.RemoveAt(i);
                i--;
            }
        }

        bool ICollection<TCsvData>.Contains(TCsvData item)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Removes the specified <typeparamref name="TCsvData"/> using its <see cref="CsvData.Level"/>.
        /// </summary>
        /// <param name="dataCollection"><see cref="CsvDataCollection{TCsvData}"/> which will be removed.</param>
        /// <returns><c>true</c> if success; otherwise, <c>false</c>.</returns>
        public bool Remove(TCsvData dataCollection)
        {
            if (dataCollection == null)
                throw new ArgumentNullException("dataCollection");

            var level = dataCollection.Level;
            return RemoveInternal(level);
        }

        /// <summary>
        /// Removes a <typeparamref name="TCsvData"/> at the specified level.
        /// </summary>
        /// <param name="level">Level of the <typeparamref name="TCsvData"/> to remove.</param>
        /// <returns><c>true</c> if success; otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="level"/> is negative or greater than <see cref="Count"/>.</exception>
        public bool Remove(int level)
        {
            if (level < 0 || level > Count - 1)
                throw new ArgumentOutOfRangeException("level", "level must be non-negative and less than Count.");

            return RemoveInternal(level);
        }

        /// <summary>
        /// Returns an enumerator that iterates through the <see cref="CsvDataCollection{TCsvData}"/>.
        /// </summary>
        /// <returns>An enumerator that iterates through the <see cref="CsvDataCollection{TCsvData}"/>.</returns>
        public IEnumerator GetEnumerator()
        {
            return _data.GetEnumerator();
        }

        IEnumerator<TCsvData> IEnumerable<TCsvData>.GetEnumerator()
        {
            return _data.GetEnumerator();
        }

        void ICollection<TCsvData>.CopyTo(TCsvData[] array, int arrayIndex)
        {
            throw new NotSupportedException();
        }

        private bool RemoveInternal(int level)
        {
            if (level == -1)
                return false;

            if (level > _data.Count - 1)
                return false;

            _data[level]._level = -1;
            _data.RemoveAt(level);
            return true;
        }
        #endregion
    }
}
