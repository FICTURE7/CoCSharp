using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace CoCSharp.Csv
{
    /// <summary>
    /// Represents a collection of <see cref="CsvData"/>.
    /// </summary>
    /// <typeparam name="TCsvData">Type of <see cref="CsvData"/> to store.</typeparam>
    [DebuggerDisplay("Count = {Count}, TID = {TID}, ID = {ID}")]
    public class CsvDataSubCollection<TCsvData> : ICollection<TCsvData> where TCsvData : CsvData, new()
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="CsvDataSubCollection{TCsvData}"/> class with the specified
        /// data ID and text ID.
        /// </summary>
        /// 
        /// <param name="id">
        /// Data ID all <typeparamref name="TCsvData"/> which will be stored in this <see cref="CsvDataSubCollection{TCsvData}"/>
        /// must have.
        /// </param>
        /// <param name="tid">
        /// Text ID all <typeparamref name="TCsvData"/> which will be stored in this <see cref="CsvDataSubCollection{TCsvData}"/>
        /// must have.
        /// </param>
        public CsvDataSubCollection(int id, string tid)
        {
            _instance = CsvData.GetInstance<TCsvData>();

            if (_instance.InvalidDataID(id))
                throw new ArgumentOutOfRangeException("id", _instance.GetArgsOutOfRangeMessage("id"));
            if (tid == null)
                throw new ArgumentNullException("tid");

            // Create an array of 16 element. VillageObjects usually doesn't have more than 16 upgrades.
            _array = new TCsvData[16];
            // NOTE: Might be a good idea to switch indexes to -1.
            _indexes = new int[16];
            _index = _instance.GetIndex(id);
            _id = id;
            _tid = tid;
        }
        #endregion

        #region Fields & Properties
        // Instance of TCsvData. 
        internal readonly TCsvData _instance;
        // Array of TCsvData stored in the sub collection. Usually upgrades.
        private TCsvData[] _array;
        // Array of indexes which points to TCsvData in _array.
        internal int[] _indexes;
        // Count of TCsvData in the sub collection.
        private int _count;

        /// <summary>
        /// Gets or sets a value indicating whether the <see cref="CsvDataSubCollection{TCsvData}"/> is read only.
        /// </summary>
        public bool IsReadOnly { get; set; }

        /// <summary>
        /// Gets or sets the <typeparamref name="TCsvData"/> associated with the specified level.
        /// </summary>
        /// <param name="level">The level of the <typeparamref name="TCsvData"/>.</param>
        /// 
        /// <returns>
        /// Returns the <typeparamref name="TCsvData"/> with the specified level; 
        /// returns null if the <typeparamref name="TCsvData"/> with the specified level is not found.
        /// </returns>
        /// 
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="level"/> is less than 0.</exception>
        /// <exception cref="InvalidOperationException"><see cref="IsReadOnly"/> is set to <c>true</c>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is null.</exception>
        /// 
        /// <exception cref="ArgumentException">
        /// <paramref name="value"/>.ID or <paramref name="value"/>.TID is not the same as <see cref="ID"/> and <see cref="TID"/>.
        /// </exception>
        /// 
        /// <exception cref="ArgumentException">
        /// <paramref name="value"/> is already in a <see cref="CsvDataSubCollection{TCsvData}"/>.
        /// </exception>
        public TCsvData this[int level]
        {
            get
            {
                if (level < 0)
                    throw new ArgumentOutOfRangeException("level", "level must be non-negative.");

                if (level > _array.Length - 1)
                    return null;

                return _array[level];
            }
            set
            {
                if (IsReadOnly)
                    throw new InvalidOperationException("CsvDataSubCollection object is readonly.");

                if (level < 0)
                    throw new ArgumentOutOfRangeException("level", "level must be non-negative.");
                if (value == null)
                    throw new ArgumentNullException("value");
                if (value.ID != _id || value.TID != _tid)
                    throw new ArgumentException("value.ID and value.TID must be the same as ID and TID.", "value");
                if (value._isInCollection)
                    throw new ArgumentException("value is already in this or another CsvDataSubCollection.", "value");

                // Update value's level to the specified level.
                // NOTE: Not sure about that one.
                value.Level = level;

                // Resize the array if the level exceeds the bounds
                // to prevent IndexOutOfRangeException.
                if (level > _array.Length - 1)
                {
                    var newSize = (level - _array.Length - 1) + 8;
                    Array.Resize(ref _array, _array.Length + newSize);
                }

                // If the data at level is null then,
                // it means we added a new data.
                if (_array[level] == null)
                {
                    if (_count > _indexes.Length - 1)
                        Array.Resize(ref _indexes, _indexes.Length + 8);

                    _indexes[_count] = level;
                    _count++;
                }

                value._isInCollection = true;
                _array[level] = value;
            }
        }

        private readonly string _tid;
        /// <summary>
        /// Gets the text ID which all <typeparamref name="TCsvData"/> in the <see cref="CsvDataCollection{TCsvData}"/> have.
        /// </summary>
        public string TID
        {
            get
            {
                return _tid;
            }
        }

        private readonly int _id;
        /// <summary>
        /// Gets the data ID which all <typeparamref name="TCsvData"/> in the <see cref="CsvDataCollection{TCsvData}"/> have.
        /// </summary>
        public int ID
        {
            get
            {
                return _id;
            }
        }

        /// <summary>
        /// Gets the number of <typeparamref name="TCsvData"/> in the <see cref="CsvDataSubCollection{TCsvData}"/>.
        /// </summary>
        public int Count
        {
            get
            {
                return _count;
            }
        }

        private readonly int _index;
        internal int Index
        {
            get
            {
                return _index;
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Adds a <typeparamref name="TCsvData"/> to the <see cref="CsvDataSubCollection{TCsvData}"/>.
        /// </summary>
        /// <param name="data">The <typeparamref name="TCsvData"/> to add to the <see cref="CsvDataSubCollection{TCsvData}"/>.</param>
        /// 
        /// <exception cref="InvalidOperationException"><see cref="IsReadOnly"/> is set to <c>true</c>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="data"/> is null.</exception>
        /// 
        /// <exception cref="ArgumentException">
        /// <paramref name="data"/>.ID or <paramref name="data"/>.TID is not the same as <see cref="ID"/> and <see cref="TID"/>.
        /// </exception>
        /// 
        /// <exception cref="ArgumentException">
        /// <paramref name="data"/> is already in a <see cref="CsvDataSubCollection{TCsvData}"/>.
        /// </exception>
        /// 
        /// <exception cref="ArgumentException">
        /// Already contain a <typeparamref name="TCsvData"/> with same level as <paramref name="data"/>.
        /// </exception>
        public void Add(TCsvData data)
        {
            if (IsReadOnly)
                throw new InvalidOperationException("CsvDataSubCollection object is readonly.");

            if (data == null)
                throw new ArgumentNullException("data");
            if (data.ID != _id || data.TID != _tid)
                throw new ArgumentException("data.ID and data.TID must be the same as ID and TID.", "data");
            if (data._isInCollection)
                throw new ArgumentException("data is already in this or another CsvDataSubCollection.", "data");

            // Resize the array if the level exceeds the bounds
            // to prevent IndexOutOfRangeException.
            if (data.Level > _array.Length - 1)
            {
                var newSize = (data.Level - _array.Length - 1) + 8;
                Array.Resize(ref _array, _array.Length + newSize);
            }
            else
            {
                // Don't need to check if _array is null at data.Level if
                // we just resized it.

                if (_array[data.Level] != null)
                    throw new ArgumentException("CsvDataSubCollection already contains a TCsvData with the same level.", "data");
            }

            if (_count > _indexes.Length - 1)
                Array.Resize(ref _indexes, _indexes.Length + 8);

            data._isInCollection = true;

            _indexes[_count] = data.Level;
            // We insert the data with its level as its index.
            _array[data.Level] = data;
            _count++;
        }

        /// <summary>
        /// Removes the specified <typeparamref name="TCsvData"/> from the <see cref="CsvDataSubCollection{TCsvData}"/>.
        /// </summary>
        /// <param name="data">The <typeparamref name="TCsvData"/> to remove from the <see cref="CsvDataSubCollection{TCsvData}"/>.</param>
        /// <returns>Returns <c>true</c> if <paramref name="data"/> was successfully removed; otherwise, <c>false</c>.</returns>
        /// 
        /// <exception cref="InvalidOperationException"><see cref="IsReadOnly"/> is set to <c>true</c>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="data"/> is null.</exception>
        /// 
        /// <exception cref = "ArgumentException">
        /// <paramref name="data"/>.ID or <paramref name="data"/>.TID is not the same as <see cref="ID"/> and <see cref="TID"/>.
        /// </exception>
        public bool Remove(TCsvData data)
        {
            if (IsReadOnly)
                throw new InvalidOperationException("CsvDataSubCollection object is readonly.");

            // Don't really need these checks, because the 'only' way a reference could get into the collection
            // is by the Add or the indexer method which already check this stuff.
            if (data == null)
                throw new ArgumentNullException("data");
            if (data.ID != _id || data.TID != _tid)
                throw new ArgumentException("data.ID and data.TID must be the same as ID and TID.", "data");

            // Return false if data is not in any collection.
            if (!data._isInCollection)
                return false;
            // Avoid IndexOutOfRangeException from TCsvData[].
            if (data.Level > _array.Length - 1)
                return false;

            if (_array[data.Level] != data)
                return false;

            data._isInCollection = false;
            _array[data.Level] = null;

            // Look for the index in _indexes pointing to _array[data.Level]
            // and reset it.
            for (int i = 0; i < _count; i++)
            {
                if (_indexes[i] == data.Level)
                {
                    _indexes[i] = 0;
                    break;
                }
            }
            _count--;
            return true;
        }

        /// <summary>
        /// Removes the <typeparamref name="TCsvData"/> with the specified level from the <see cref="CsvDataSubCollection{TCsvData}"/>.
        /// </summary>
        /// <param name="level">The level of the <typeparamref name="TCsvData"/> to remove.</param>
        /// <returns>
        /// Returns <c>true</c> if <typeparamref name="TCsvData"/> with the specified <paramref name="level"/>
        /// was successfully removed; otherwise, <c>false</c>.
        /// </returns>
        /// 
        /// <exception cref="InvalidOperationException"><see cref="IsReadOnly"/> is set to <c>true</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="level"/> is less than 0.</exception>
        public bool Remove(int level)
        {
            if (IsReadOnly)
                throw new InvalidOperationException("CsvDataSubCollection object is readonly.");

            if (level < 0)
                throw new ArgumentOutOfRangeException("level", "level must be non-negative.");

            // Avoid IndexOutOfRangeException from TCsvData[].
            if (level > _array.Length - 1)
                return false;

            if (_array[level] == null)
                return false;

            _array[level]._isInCollection = false;
            _array[level] = null;

            // Look for the index in _indexes pointing to _array[data.Level]
            // and reset it.
            for (int i = 0; i < _count; i++)
            {
                if (_indexes[i] == level)
                {
                    _indexes[i] = 0;
                    break;
                }
            }
            _count--;
            return true;
        }

        /// <summary>
        /// Removes all <typeparamref name="TCsvData"/> from the <see cref="CsvDataSubCollection{TCsvData}"/>.
        /// </summary>
        /// <exception cref="InvalidOperationException"><see cref="IsReadOnly"/> is set to <c>true</c>.</exception>
        public void Clear()
        {
            if (IsReadOnly)
                throw new InvalidOperationException("CsvDataSubCollection object is readonly.");

            for (int i = 0; i < _count; i++)
            {
                // Clear array.
                _array[_indexes[i]] = null;
                // Clear index as well.
                _indexes[i] = 0;
            }
            _count = 0;
        }

        /// <summary>
        /// Determines whether the <see cref="CsvDataSubCollection{TCsvData}"/> contains a specific <typeparamref name="TCsvData"/>.
        /// </summary>
        /// <param name="data">The <typeparamref name="TCsvData"/> to locate in the <see cref="CsvDataSubCollection{TCsvData}"/>.</param>
        /// <returns>
        /// <c>true</c> if the <paramref name="data"/> was found in the <see cref="CsvDataSubCollection{TCsvData}"/>; otherwise, <c>false</c>.
        /// </returns>
        /// 
        /// <exception cref="ArgumentNullException"><paramref name="data"/> is null.</exception>
        /// <exception cref = "ArgumentException">
        /// <paramref name="data"/>.ID or <paramref name="data"/>.TID is not the same as <see cref="ID"/> and <see cref="TID"/>.
        /// </exception>
        public bool Contains(TCsvData data)
        {
            if (data == null)
                throw new ArgumentNullException("data");
            if (data.ID != _id || data.TID != _tid)
                throw new ArgumentException("data.ID and data.TID must be the same as ID and TID.", "data");

            // Avoid IndexOutOfRangeException from TCsvData[].
            if (data.Level > _array.Length - 1)
                return false;

            if (_array[data.Level] != data)
                return false;

            return true;
        }

        /// <summary>
        /// Determines whether the <see cref="CsvDataSubCollection{TCsvData}"/> contains a <typeparamref name="TCsvData"/>
        /// with the specified level.
        /// </summary>
        /// <param name="level">
        /// The level of <typeparamref name="TCsvData"/> to locate in the <see cref="CsvDataSubCollection{TCsvData}"/>.
        /// </param>
        /// <returns>
        /// <c>true</c> if the <typeparamref name="TCsvData"/> with the specified level was found in 
        /// the <see cref="CsvDataSubCollection{TCsvData}"/>; otherwise, <c>false</c>.
        /// </returns>
        /// 
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="level"/> is less than 0.</exception>
        public bool Contains(int level)
        {
            if (level < 0)
                throw new ArgumentOutOfRangeException("level", "level must be non-negative.");

            // Avoid IndexOutOfRangeException from TCsvData[].
            if (level > _array.Length - 1)
                return false;

            if (_array[level] == null)
                return false;

            return true;
        }

        /// <summary>
        /// Copies all the data in the <see cref="CsvDataSubCollection{TCsvData}"/> to a new array.
        /// </summary>
        /// <returns>A new array containing the copied data of the <see cref="CsvDataSubCollection{TCsvData}"/>.</returns>
        public TCsvData[] ToArray()
        {
            var count = _count;
            var returnArray = new TCsvData[count];
            for (int i = 0; i < count; i++)
            {
                returnArray[i] = _array[_indexes[i]];
            }

            return returnArray;
        }

        /// <summary>
        /// Copies the entire <see cref="CsvDataSubCollection{TCsvData}"/> to a specified compatible one-dimensional array, at the specified
        /// index to array.
        /// </summary>
        /// <param name="array">Target one-dimensional array to copy the <see cref="CsvDataSubCollection{TCsvData}"/></param>
        /// <param name="arrayIndex">Index at which to start copying the <see cref="CsvDataSubCollection{TCsvData}"/> in the target array.</param>
        /// 
        /// <exception cref="ArgumentNullException"><paramref name="array"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="arrayIndex"/> is less than 0.</exception>
        /// <exception cref="ArgumentException"><paramref name="array"/> is too small.</exception>
        public void CopyTo(TCsvData[] array, int arrayIndex)
        {
            if (array == null)
                throw new ArgumentNullException("array");
            if (arrayIndex < 0)
                throw new ArgumentOutOfRangeException("arrayIndex", "arrayIndex must be non-negative.");

            var count = _count;
            if (array.Length - arrayIndex < count)
                throw new ArgumentException("array is too small, check array's length and arrayIndex.", "array");

            var j = 0;
            for (int i = arrayIndex; i < array.Length; i++)
            {
                array[i] = _array[_indexes[j]];
                j++;
            }
        }

        /// <summary>
        /// Returns an enumerator that iterates through the <see cref="CsvDataSubCollection{TCsvData}"/>.
        /// </summary>
        /// <returns>An <see cref="IEnumerable{TCsvData}"/> for the <see cref="CsvDataSubCollection{TCsvData}"/>.</returns>
        public IEnumerator<TCsvData> GetEnumerator()
        {
            return new Enumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private class Enumerator : IEnumerator<TCsvData>
        {
            public Enumerator(CsvDataSubCollection<TCsvData> collection)
            {
                if (collection == null)
                    throw new ArgumentNullException("collection");

                _collection = collection;
                _position = -1;
            }

            private readonly CsvDataSubCollection<TCsvData> _collection;
            private int _position;

            public TCsvData Current
            {
                get
                {
                    return _collection._array[_collection._indexes[_position]];
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
                _position++;
                return _position < _collection.Count;
            }

            public void Reset()
            {
                _position = -1;
            }

            public void Dispose()
            {
                // Space
            }
        }
        #endregion
    }
}
