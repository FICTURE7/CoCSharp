using System;
using System.Collections;
using System.Collections.Generic;

namespace CoCSharp.Csv
{
    /// <summary>
    /// Represents a collection of <see cref="CsvData"/> which associates data ID and levels
    /// of <see cref="CsvData"/>.
    /// </summary>
    /// <typeparam name="TCsvData">Type of <see cref="CsvData"/>.</typeparam>
    public class CsvDataCollection<TCsvData> : ICollection<TCsvData> where TCsvData : CsvData, new()
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="CsvDataCollection{TCsvData}"/> class.
        /// </summary>
        public CsvDataCollection()
        {
            _listDictionary = new SortedDictionary<int, SortedList<int, TCsvData>>();
            _sampleInstance = new TCsvData();
        }
        #endregion

        #region Fields & Properties
        /// <summary>
        /// Gets or sets the array of <typeparamref name="TCsvData"/> associated with the specified data ID
        /// in the <see cref="CsvDataCollection{TCsvData}"/>.
        /// </summary>
        /// <param name="dataId">Data ID with which the <typeparamref name="TCsvData"/> is associated with.</param>
        /// <returns>
        /// A new array of <typeparamref name="TCsvData"/> associated with the specified data ID
        /// in the <see cref="CsvDataCollection{TCsvData}"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="dataId"/> is not within the range of <typeparamref name="TCsvData"/>.</exception>
        public TCsvData[] this[int dataId]
        {
            get
            {
                if (_sampleInstance.InvalidDataID(dataId))
                    throw new ArgumentOutOfRangeException("value");

                if (!_listDictionary.ContainsKey(dataId))
                    return null;

                // Copy all the references in list to a new array.
                var list = _listDictionary[dataId];
                var array = new TCsvData[list.Values.Count];
                for (int i = 0; i < array.Length; i++)
                    array[i] = list[i];

                return array;
            }
            //TODO: Consider value.ID and value.Level.
            set
            {
                if (IsReadOnly)
                    throw new InvalidOperationException("CsvDataCollection object is readonly.");
                if (value == null)
                    throw new ArgumentNullException("value");
                if (_sampleInstance.InvalidDataID(dataId))
                    throw new ArgumentOutOfRangeException("value");

                // Check if we already have a list of the specified dataId.
                // If we do not, we create a new and add it to the dictionary;
                // otherwise we clear the list and add the value to it.
                if (!_listDictionary.ContainsKey(dataId))
                {
                    var list = new SortedList<int, TCsvData>();
                    for (int i = 0; i < value.Length; i++)
                    {
                        list.Add(value[i].Level, value[i]);
                    }

                    _listDictionary.Add(dataId, list);
                }
                else
                {
                    var list = _listDictionary[dataId];
                    list.Clear();

                    for (int i = 0; i < value.Length; i++)
                    {
                        list.Add(value[i].Level, value[i]);
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the <typeparamref name="TCsvData"/> associated with the specified data ID and level.
        /// </summary>
        /// <param name="dataId">Data ID of the <typeparamref name="TCsvData"/>.</param>
        /// <param name="level">Level of the <typeparamref name="TCsvData"/>.</param>
        /// <returns>
        /// The <typeparamref name="TCsvData"/> associated with the specified data ID and level.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="dataId"/> is not within the range of <typeparamref name="TCsvData"/>.</exception>
        public TCsvData this[int dataId, int level]
        {
            get
            {
                if (_sampleInstance.InvalidDataID(dataId))
                    throw new ArgumentOutOfRangeException("value", _sampleInstance.ArgumentExceptionMessage);

                if (!_listDictionary.ContainsKey(dataId))
                    return null;

                var list = _listDictionary[dataId];
                if (list.ContainsKey(level))
                    return list[level];

                return null;
            }
            set
            {
                if (IsReadOnly)
                    throw new InvalidOperationException("CsvDataCollection object is readonly.");
                if (value == null)
                    throw new ArgumentNullException("value");
                if (_sampleInstance.InvalidDataID(dataId))
                    throw new ArgumentOutOfRangeException("value", _sampleInstance.ArgumentExceptionMessage);

                // Check if we already have a list of the specified dataId.
                // If we do not, we create a new and add it to the dictionary;
                // otherwise we clear the list and add the value to it.
                if (!_listDictionary.ContainsKey(dataId))
                {
                    var list = new SortedList<int, TCsvData>();
                    list.Add(value.Level, value);

                    _listDictionary.Add(dataId, list);
                }
                else
                {
                    var list = _listDictionary[dataId];
                    // Update value in list if we already have one with
                    // same level; otherwise, we add it to the list.
                    if (list.ContainsKey(level))
                    {
                        list[level] = value;
                    }
                    else
                    {
                        list.Add(level, value);
                    }
                }
            }
        }

        /// <summary>
        /// Gets the number of <typeparamref name="TCsvData"/> in the <see cref="CsvDataCollection{TCsvData}"/>.
        /// </summary>
        public int Count
        {
            get
            {
                return _listDictionary.Count;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the <see cref="CsvDataCollection{TCsvData}"/> is read only.
        /// </summary>
        public bool IsReadOnly { get; set; }

        // Dictionary of DataID associated with List<TCsvData>.
        private readonly SortedDictionary<int, SortedList<int, TCsvData>> _listDictionary;
        // Instance of TCsvData to use internal functions.
        private readonly TCsvData _sampleInstance;
        #endregion

        #region Methods
        /// <summary>
        /// Adds a <typeparamref name="TCsvData"/> to the <see cref="CsvDataCollection{TCsvData}"/>.
        /// </summary>
        /// <param name="item">The <typeparamref name="TCsvData"/> to add to the <see cref="CsvDataCollection{TCsvData}"/>.</param>
        /// <exception cref="InvalidOperationException"><see cref="IsReadOnly"/> is set to <c>true</c>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="item"/> is null.</exception>
        public void Add(TCsvData item)
        {
            if (IsReadOnly)
                throw new InvalidOperationException("CsvDataCollection object is readonly.");
            if (item == null)
                throw new ArgumentNullException("item");

            // Check If we have a list associated with this item.ID,
            // we use it; otherwise we create a new list.
            if (!_listDictionary.ContainsKey(item.ID))
            {
                var list = new SortedList<int, TCsvData>();
                list.Add(item.Level, item);

                _listDictionary.Add(item.ID, list);
            }
            else
            {
                var list = _listDictionary[item.ID];
                if (list.ContainsKey(item.Level))
                    throw new ArgumentException("Already contains a TCsvData with same level in the same data ID sub-collection.", "item");

                list.Add(item.Level, item);
            }
        }

        /// <summary>
        /// Removes the first occurrence of a specific <typeparamref name="TCsvData"/> from the <see cref="CsvDataCollection{TCsvData}"/>.
        /// </summary>
        /// <param name="item">The <typeparamref name="TCsvData"/> to remove from the <see cref="CsvDataCollection{TCsvData}"/>.</param>
        /// <returns>Returns <c>true</c> if <paramref name="item"/> was successfully removed; otherwise, <c>false</c>.</returns>
        /// <exception cref="InvalidOperationException"><see cref="IsReadOnly"/> is set to <c>true</c>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="item"/> is null.</exception>
        public bool Remove(TCsvData item)
        {
            if (IsReadOnly)
                throw new InvalidOperationException("CsvDataCollection object is readonly.");
            if (item == null)
                throw new ArgumentNullException("item");

            if (!_listDictionary.ContainsKey(item.ID))
                return false;

            return _listDictionary[item.ID].Remove(item.Level);
        }

        /// <summary>
        /// Removes all <see cref="CsvData"/> from the <see cref="CsvDataCollection{TCsvData}"/>.
        /// </summary>
        /// <exception cref="InvalidOperationException"><see cref="IsReadOnly"/> is set to <c>true</c>.</exception>
        public void Clear()
        {
            if (IsReadOnly)
                throw new InvalidOperationException("CsvDataCollection object is readonly.");

            _listDictionary.Clear();
        }

        /// <summary>
        /// Determines whether the <see cref="CsvDataCollection{TCsvData}"/> contains a specific <typeparamref name="TCsvData"/>.
        /// </summary>
        /// <param name="item">The <typeparamref name="TCsvData"/> to locate in the <see cref="CsvDataCollection{TCsvData}"/>.</param>
        /// <returns><c>true</c> if the <paramref name="item"/> was found in the <see cref="CsvDataCollection{TCsvData}"/>; otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="item"/> is null.</exception>
        public bool Contains(TCsvData item)
        {
            if (IsReadOnly)
                throw new InvalidOperationException("CsvDataCollection object is readonly.");

            if (!_listDictionary.ContainsKey(item.ID))
                return false;

            return _listDictionary[item.ID].ContainsValue(item);
        }

        /// <summary>
        /// Currently not supported.
        /// Copies the elements of the <see cref="CsvDataCollection{TCsvData}"/> to an <see cref="Array"/>, starting at a particular <see cref="Array"/> index.
        /// </summary>
        /// <param name="array">
        /// The one-dimensional <see cref="Array"/> that is the destination of the elements copied from <see cref="CsvDataCollection{TCsvData}"/>.
        /// </param>
        /// <param name="arrayIndex">The zero-based index in <paramref name="array"/> at which copying begins.</param>
        /// <exception cref="NotSupportedException"></exception>
        public void CopyTo(TCsvData[] array, int arrayIndex)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Copies the elements of the <see cref="CsvDataCollection{TCsvData}"/> to a new 2-dimensional array.
        /// </summary>
        /// <returns>A new 2-dimensional array which contains the copied elements of the <see cref="CsvDataCollection{TCsvData}"/>.</returns>
        public TCsvData[][] ToArray()
        {
            var retArray = new TCsvData[_listDictionary.Count][];
            var index = 0;
            foreach (var id in _listDictionary.Keys)
            {
                retArray[index] = this[id];
                index++;
            }

            return retArray;
        }

        /// <summary>
        /// Returns an enumerator that iterates through the <see cref="CsvDataCollection{TCsvData}"/>.
        /// </summary>
        /// <returns>An <see cref="IEnumerable{TCsvData}"/> for the <see cref="CsvDataCollection{TCsvData}"/>.</returns>
        public IEnumerator<TCsvData> GetEnumerator()
        {
            //return new Enumerator(_collection.ToArray());
            var all = new List<TCsvData>();
            foreach (var dataId in _listDictionary.Keys)
            {
                var list = _listDictionary[dataId];
                foreach (var csvData in list.Values)
                {
                    all.Add(csvData);
                }
            }
            return new Enumerator(all.ToArray());
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private class Enumerator : IEnumerator<TCsvData>
        {
            public Enumerator(TCsvData[] dataCollection)
            {
                _index = -1;
                _dataCollection = dataCollection;
            }

            private int _index;
            private TCsvData[] _dataCollection;

            public TCsvData Current
            {
                get
                {
                    return _dataCollection[_index];
                }
                set
                {
                    _dataCollection[_index] = value;
                }
            }

            object IEnumerator.Current
            {
                get
                {
                    return Current;
                }
            }

            public bool MoveNext()
            {
                _index++;
                return (_index < _dataCollection.Length);
            }

            public void Reset()
            {
                _index = -1;
            }

            // Nothing to dispose.
            public void Dispose()
            {
                // Space
            }
        }
        #endregion
    }
}
