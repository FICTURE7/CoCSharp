using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace CoCSharp.Csv
{
    /// <summary>
    /// Represents a collection of <see cref="CsvData"/>.
    /// </summary>
    /// <typeparam name="TCsvData">Type of <see cref="CsvData"/>.</typeparam>
    [DebuggerDisplay("Name = {Name}, Count = {Count}")]
    public class CsvDataRow<TCsvData> : CsvDataRow, ICollection<TCsvData> where TCsvData : CsvData, new()
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="CsvDataRow{TCsvData}"/> class with the specified
        /// name.
        /// </summary>
        /// <param name="name">Name of the <see cref="CsvDataRow{TCsvData}"/>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is null.</exception>
        public CsvDataRow(string name)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));

            _name = name;
            _data = new List<TCsvData>(16);
            _kindId = CsvData.GetKindID(typeof(TCsvData)); 
        }
        #endregion

        #region Fields & Properties
        // CsvDataCollectionRef pointing to this.
        internal CsvDataRowRef<TCsvData> _ref;
        // Kind ID of the TCsvData.
        private readonly int _kindId;

        // List containing the TCsvData in the row.
        private readonly List<TCsvData> _data;

        bool ICollection<TCsvData>.IsReadOnly => false;

        public TCsvData this[int level]
        {
            get
            {
                if (level < 0 || level > Count)
                    throw new ArgumentOutOfRangeException("level", "level must be non-negative and less than Count.");

                return _data[level];
            }
        }

        public override CsvDataRowRef Ref => _ref;

        public override int ID
        {
            get
            {
                if (_ref == null)
                    throw new InvalidOperationException("CsvDataCollection must be in a CsvDataRow to have an ID.");

                return _ref.ID;
            }
        }

        private readonly string _name;
        public override string Name => _name;
        public int Count => _data.Count;

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
        /// Adds the specified <typeparamref name="TCsvData"/> at the end of the <see cref="CsvDataRow{TCsvData}"/>.
        /// </summary>
        /// <param name="data"><typeparamref name="TCsvData"/> to add to the <see cref="CsvDataRow{TCsvData}"/>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="data"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="data"/> is already in a <see cref="CsvDataRow{TCsvData}"/>.</exception>
        public void Add(TCsvData data)
        {
            if (data == null)
                throw new ArgumentNullException("data");

            // Set level of data to that of the last index.
            data._ref = _ref;
            _data.Add(data);
        }

        /// <summary>
        /// Removes all the <typeparamref name="TCsvData"/> in the <see cref="CsvDataRow{TCsvData}"/>.
        /// </summary>
        public void Clear()
        {
            for (int i = 0; i < _data.Count; i++)
            {
                _data[i]._ref = null;
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
        /// <param name="dataCollection"><see cref="CsvDataRow{TCsvData}"/> which will be removed.</param>
        /// <returns><c>true</c> if success; otherwise, <c>false</c>.</returns>
        public bool Remove(TCsvData dataCollection)
        {
            if (dataCollection == null)
                throw new ArgumentNullException(nameof(dataCollection));

            return false;
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
                throw new ArgumentOutOfRangeException(nameof(level), "level must be non-negative and less than Count.");

            return RemoveInternal(level);
        }

        /// <summary>
        /// Returns an enumerator that iterates through the <see cref="CsvDataRow{TCsvData}"/>.
        /// </summary>
        /// <returns>An enumerator that iterates through the <see cref="CsvDataRow{TCsvData}"/>.</returns>
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
            if (level > _data.Count - 1)
                return false;

            _data[level]._ref = null;
            _data.RemoveAt(level);
            return true;
        }
        #endregion
    }
}
