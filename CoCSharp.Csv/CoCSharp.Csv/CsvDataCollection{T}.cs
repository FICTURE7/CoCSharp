using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace CoCSharp.Csv
{
    /// <summary>
    /// Represents a collection of <see cref="CsvData"/>.
    /// </summary>
    /// <typeparam name="TCsvData">Type of <see cref="CsvData"/>.</typeparam>
    [DebuggerDisplay("Name = {Name}, Count = {Count}")]
    public class CsvDataCollection<TCsvData> : CsvDataCollection, ICollection<TCsvData> where TCsvData : CsvData, new()
    {
        #region Constructors
        // Kind ID of the TCsvData.
        private static readonly int _kindId = new TCsvData().KindID;

        /// <summary>
        /// Initializes a new instance of the <see cref="CsvDataCollection{TCsvData}"/> class with the specified
        /// name.
        /// </summary>
        /// <param name="name">Name of the <see cref="CsvDataCollection{TCsvData}"/>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is null.</exception>
        public CsvDataCollection(string name)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));

            _name = name;
            _data = new List<TCsvData>(16);
            _ref = CsvDataCollectionRef<TCsvData>.NullRef;
        }
        #endregion

        #region Fields & Properties
        // CsvDataCollectionRef pointing to this.
        internal CsvDataCollectionRef<TCsvData> _ref;
        // List containing the TCsvData in the row.
        private readonly List<TCsvData> _data;
        // Name of the collection/column.
        private readonly string _name;

        bool ICollection<TCsvData>.IsReadOnly => false;

        /// <summary>
        /// Gets the <typeparamref name="TCsvData"/> with the specified level; return nulls if a <typeparamref name="TCsvData"/>
        /// with the specified level does not exists in the <see cref="CsvDataCollection{TCsvData}"/>.
        /// </summary>
        /// <param name="level">Level of the <typeparamref name="TCsvData"/>.</param>
        /// <returns>The <typeparamref name="TCsvData"/> with the specified level.</returns>
        public TCsvData this[int level]
        {
            get
            {
                if (!ContainsInternal(level))
                    return null;

                return _data[level];
            }
        }

        /// <summary>
        /// Gets the <see cref="CsvDataCollectionRef"/> of the <see cref="CsvDataCollection{TCsvData}"/>.
        /// </summary>
        public override CsvDataCollectionRef CollectionRef => _ref;

        /// <summary>
        /// Gets the number of <typeparamref name="TCsvData"/> in the <see cref="CsvDataCollection{TCsvData}"/>
        /// </summary>
        public override int Count => _data.Count;

        /// <summary>
        /// Gets the name of the <see cref="CsvDataCollection{TCsvData}"/>.
        /// </summary>
        public override string Name => _name;

        /// <summary>
        /// Gets the ID of the <see cref="CsvDataCollection{TCsvData}"/>.
        /// </summary>
        public override int ID
        {
            get
            {
                Debug.Assert(_ref != null);
                if (_ref == CsvDataCollectionRef<TCsvData>.NullRef)
                    throw new InvalidOperationException("CsvDataCollection must be in a CsvDataRow to have an ID.");

                return _ref.ID;
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

        /// <summary>
        /// Gets the lowest level <typeparamref name="TCsvData"/> in the <see cref="CsvDataCollection{TCsvData}"/>.
        /// </summary>
        public TCsvData Min
        {
            get
            {
                if (_data.Count == 0)
                    return null;

                return _data[0];
            }
        }
        #endregion

        #region Methods
        public bool Contains(TCsvData data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            var level = data.Level;
            return ContainsInternal(level);
        }

        public override bool Contains(CsvData data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            return Contains((TCsvData)data);
        }

        private bool ContainsInternal(int level) => !(level < 0 || level > _data.Count - 1);

        /// <summary>
        /// Adds the specified <typeparamref name="TCsvData"/> at the end of the <see cref="CsvDataCollection{TCsvData}"/>.
        /// </summary>
        /// <param name="data"><typeparamref name="TCsvData"/> to add to the <see cref="CsvDataCollection{TCsvData}"/>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="data"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="data"/> is already in a <see cref="CsvDataCollection{TCsvData}"/>.</exception>
        public void Add(TCsvData data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));
            if (data._ref != null)
                throw new ArgumentException("Data is already in a CsvDataCollection.", nameof(data));

            // Set level of data to that of the last index.
            InsertInternal(_data.Count, data);
        }

        public override void Add(CsvData data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));
            if (data._ref != null)
                throw new ArgumentException("Data is already in a CsvDataCollection.", nameof(data));

            InsertInternal(_data.Count, (TCsvData)data);
        }

        public void Insert(int level, TCsvData data)
        {
            if (level < 0 || level > Count)
                throw new ArgumentOutOfRangeException(nameof(level));
            if (data == null)
                throw new ArgumentNullException(nameof(data));
            if (data._ref != null)
                throw new ArgumentException("Data is already in a CsvDataCollection.", nameof(data));

            InsertInternal(level, data);
        }

        public override void Insert(int level, CsvData data)
        {
            Insert(level, (TCsvData)data);
        }

        private void InsertInternal(int level, TCsvData data)
        {
            data._level = level;
            data._ref = _ref;
            _data.Insert(level, data);
        }

        /// <summary>
        /// Removes the specified <typeparamref name="TCsvData"/> using its <see cref="CsvData.Level"/>.
        /// </summary>
        /// <param name="data"><typeparamref name="TCsvData"/> which will be removed.</param>
        /// <returns><c>true</c> if success; otherwise, <c>false</c>.</returns>
        public bool Remove(TCsvData data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            var level = data.Level;
            return RemoveInternal(level);
        }

        public override bool Remove(CsvData data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            return Remove((TCsvData)data);
        }

        /// <summary>
        /// Removes a <typeparamref name="TCsvData"/> at the specified level.
        /// </summary>
        /// <param name="level">Level of the <typeparamref name="TCsvData"/> to remove.</param>
        /// <returns><c>true</c> if success; otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="level"/> is negative or greater than <see cref="Count"/>.</exception>
        public bool Remove(int level)
        {
            if (!ContainsInternal(level))
                return false;

            return RemoveInternal(level);
        }

        private bool RemoveInternal(int level)
        {
            Debug.Assert(ContainsInternal(level));

            var data = _data[level];
            data._level = -1;
            data._ref = null;

            _data.RemoveAt(level);
            return true;
        }

        /// <summary>
        /// Removes all the <typeparamref name="TCsvData"/> in the <see cref="CsvDataCollection{TCsvData}"/>.
        /// </summary>
        public override void Clear()
        {
            for (int i = 0; i < _data.Count; i++)
            {
                var data = _data[i];
                data._level = -1;
                data._ref = null;
            }
            _data.Clear();
        }

        public void CopyTo(TCsvData[] array, int arrayIndex)
        {
            _data.CopyTo(array, arrayIndex);
        }

        public override void CopyTo(CsvData[] array, int arrayIndex)
        {
            // Not the most efficient way, but it works.
            Array.Copy(_data.ToArray(), arrayIndex, array, arrayIndex, _data.Count - arrayIndex);
        }

        /// <summary>
        /// Returns an enumerator that iterates through the <see cref="CsvDataCollection{TCsvData}"/>.
        /// </summary>
        /// <returns>An enumerator that iterates through the <see cref="CsvDataCollection{TCsvData}"/>.</returns>
        public override IEnumerator<CsvData> GetEnumerator() => _data.GetEnumerator();

        IEnumerator<TCsvData> IEnumerable<TCsvData>.GetEnumerator() => _data.GetEnumerator();
        #endregion
    }
}
