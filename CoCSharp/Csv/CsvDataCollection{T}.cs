using System;
using System.Collections;
using System.Collections.Generic;

namespace CoCSharp.Csv
{
    /// <summary>
    /// Represents a collection of <see cref="CsvDataSubCollection{TCsvData}"/> where each
    /// <see cref="CsvDataSubCollection{TCsvData}"/> have a unique <see cref="CsvDataSubCollection{TCsvData}.ID"/>.
    /// </summary>
    /// <typeparam name="TCsvData">Type of <see cref="CsvData"/> to store.</typeparam>
    public class CsvDataCollection<TCsvData> : ICollection<CsvDataSubCollection<TCsvData>> where TCsvData : CsvData, new()
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="CsvDataCollection{TCsvData}"/> class.
        /// </summary>
        public CsvDataCollection()
        {
            _instance = CsvData.GetInstance<TCsvData>();

            _subCollections = new CsvDataSubCollection<TCsvData>[48];
            _indexes = new int[48];
        }
        #endregion

        #region Fields & Properties
        private readonly TCsvData _instance;
        // Array of CsvDataSubCollection which will store the CsvDataSubCollection added to the collection.
        private CsvDataSubCollection<TCsvData>[] _subCollections;
        // Array of indexes which points to CsvDataSubCollection in _subCollection.
        internal int[] _indexes;
        // Count of CsvDataSubCollection in the collection.
        private int _count;

        /// <summary>
        /// Gets or sets a value indicating whether the <see cref="CsvDataCollection{TCsvData}"/> is read only.
        /// </summary>
        public bool IsReadOnly { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="CsvDataSubCollection{TCsvData}"/> with the specified data ID.
        /// </summary>
        /// <param name="dataId">The data ID of the <see cref="CsvDataSubCollection{TCsvData}"/>.</param>
        /// 
        /// <returns>
        /// Returns the <see cref="CsvDataSubCollection{TCsvData}"/> with the specified data ID; 
        /// returns null if the <see cref="CsvDataSubCollection{TCsvData}"/> with the specified data ID is not found.
        /// </returns>
        /// 
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="dataId"/> is not within the range of <typeparamref name="TCsvData"/>.
        /// </exception>
        /// 
        /// <exception cref="InvalidOperationException"><see cref="IsReadOnly"/> is set to <c>true</c>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="value"/>.ID is not the same as <paramref name="dataId"/>.</exception>
        public CsvDataSubCollection<TCsvData> this[int dataId]
        {
            get
            {
                if (_instance.InvalidDataID(dataId))
                    throw new ArgumentOutOfRangeException("dataId", _instance.GetArgsOutOfRangeMessage("dataId"));

                var index = _instance.GetIndex(dataId);
                if (index > _subCollections.Length - 1)
                    return null;

                return _subCollections[index];
            }
            set
            {
                if (IsReadOnly)
                    throw new InvalidOperationException("CsvDataCollection object is readonly.");

                if (_instance.InvalidDataID(dataId))
                    throw new ArgumentOutOfRangeException("dataId", _instance.GetArgsOutOfRangeMessage("dataId"));
                if (value == null)
                    throw new ArgumentNullException("value");
                if (value.ID != dataId)
                    throw new ArgumentException("value.ID must be the same as dataId", "value");

                var index = _instance.GetIndex(dataId);
                // Avoid IndexOutOfRangeException.
                if (index > _subCollections.Length - 1)
                {
                    var newSize = (index - _subCollections.Length - 1) + 8;
                    Array.Resize(ref _subCollections, _subCollections.Length + newSize);
                }

                // If _subCollections[index] != null then
                // we're replacing something.
                if (_subCollections[index] != null)
                {
                    _subCollections[index] = value;
                }
                // Otherwise we're adding something.
                else
                {
                    //for (int i = 0; i < _count; i++)
                    //{
                    //    var subCol = _subCollections[_indexes[i]];
                    //    if (subCol.TID == value.TID)
                    //        throw new ArgumentException("Each CsvDataSubCollection in a CsvDataCollection must have a unique pair of ID and TID.", "value");
                    //}

                    if (_count > _indexes.Length - 1)
                        Array.Resize(ref _indexes, _indexes.Length + 8);

                    _indexes[_count] = index;
                    _subCollections[index] = value;
                    _count++;
                }
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="CsvDataSubCollection{TCsvData}"/> with the specified text ID.
        /// </summary>
        /// <param name="textId">he text ID of the <see cref="CsvDataSubCollection{TCsvData}"/>.</param>
        /// 
        /// <returns>
        /// Returns the <see cref="CsvDataSubCollection{TCsvData}"/> with the specified text ID; 
        /// returns null if the <see cref="CsvDataSubCollection{TCsvData}"/> with the specified text ID is not found.
        /// </returns>
        /// 
        /// <exception cref="ArgumentNullException"><paramref name="textId"/> is null.</exception>
        /// <exception cref="InvalidOperationException"><see cref="IsReadOnly"/> is set to <c>true</c>.</exception>
        /// <exception cref="ArgumentException"><paramref name="value"/>.TID is not the same as <paramref name="textId"/>.</exception>
        public CsvDataSubCollection<TCsvData> this[string textId]
        {
            get
            {
                if (textId == null)
                    throw new ArgumentNullException("textId");
                
                for (int i = 0; i < _count; i++)
                {
                    if (_subCollections[_indexes[i]].TID == textId)
                        return _subCollections[_indexes[i]];
                }
                return null;
            }
            set
            {
                if (IsReadOnly)
                    throw new InvalidOperationException("CsvDataCollection object is readonly.");

                if (textId == null)
                    throw new ArgumentNullException("textId");
                if (value.TID != textId)
                    throw new ArgumentException("value.TID must be the same as textId", "value");

                // Look for items with the same ID
                // If we find one we replace its reference.
                //for (int i = 0; i < _count; i++)
                //{
                //    if (_subCollections[_indexes[i]].TID == textId)
                //    {
                //        _subCollections[_indexes[i]] = value;
                //        return;
                //    }
                //}

                // Otherwise we add it to the collection at its Index.

                // Avoid IndexOutOfRangeException.
                if (value.Index > _subCollections.Length - 1)
                {
                    var newSize = (value.Index - _subCollections.Length - 1) + 8;
                    Array.Resize(ref _subCollections, _subCollections.Length + newSize);
                }                

                if (_count > _indexes.Length - 1)
                    Array.Resize(ref _indexes, _indexes.Length + 8);

                _indexes[_count] = value.Index;
                _subCollections[value.Index] = value;
                _count++;
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
        #endregion

        #region Methods
        /// <summary>
        /// Adds a <see cref="CsvDataSubCollection{TCsvData}"/> to the <see cref="CsvDataCollection{TCsvData}"/>.
        /// </summary>
        /// <param name="subCollection">The <see cref="CsvDataSubCollection{TCsvData}"/> to add to the <see cref="CsvDataCollection{TCsvData}"/>.</param>
        /// 
        /// <exception cref="InvalidOperationException"><see cref="IsReadOnly"/> is set to <c>true</c>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="subCollection"/> is null.</exception>
        /// <exception cref="ArgumentException">
        /// The <see cref="CsvDataCollection{TCsvData}"/> already contains a <see cref="CsvDataSubCollection{TCsvData}"/> with the same
        /// ID.
        /// </exception>
        public void Add(CsvDataSubCollection<TCsvData> subCollection)
        {
            if (IsReadOnly)
                throw new InvalidOperationException("CsvDataCollection object is readonly.");
            if (subCollection == null)
                throw new ArgumentNullException("subCollection");

            // Avoid IndexOutOfRangeException.
            if (subCollection.Index > _subCollections.Length - 1)
            {
                var newSize = (subCollection.Index - _subCollections.Length - 1) + 8;
                Array.Resize(ref _subCollections, _subCollections.Length + newSize);
            }
            else
            {
                // Look through the array at its index
                // to look for item with the same Data ID.
                if (_subCollections[subCollection.Index] != null)
                    throw new ArgumentException("Each CsvDataSubCollection in a CsvDataCollection must have a unique ID.", "subCollection");

                // Look through the collection and search for
                // items with the same TID.
                //for (int i = 0; i < _count; i++)
                //{
                //    var subCol = _subCollections[_indexes[i]];
                //    if (subCol.TID == subCollection.TID)
                //        throw new ArgumentException("Each CsvDataSubCollection in a CsvDataCollection must have a unique pair of ID and TID.", "subCollection");
                //}
            }

            if (_count > _indexes.Length - 1)
                Array.Resize(ref _indexes, _indexes.Length + 8);

            _indexes[_count] = subCollection.Index;
            _subCollections[subCollection.Index] = subCollection;
            _count++;
        }

        /// <summary>
        /// Removes the specified <see cref="CsvDataSubCollection{TCsvData}"/> from the <see cref="CsvDataCollection{TCsvData}"/>.
        /// </summary>
        /// <param name="subCollection">The <see cref="CsvDataSubCollection{TCsvData}"/> to remove from the <see cref="CsvDataCollection{TCsvData}"/>.</param>
        /// <returns>Returns <c>true</c> if <paramref name="subCollection"/> was successfully removed; otherwise, <c>false</c>.</returns>
        /// 
        /// <exception cref="InvalidOperationException"><see cref="IsReadOnly"/> is set to <c>true</c>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="subCollection"/> is null.</exception>
        public bool Remove(CsvDataSubCollection<TCsvData> subCollection)
        {
            if (IsReadOnly)
                throw new InvalidOperationException("CsvDataCollection object is readonly.");
            if (subCollection == null)
                throw new ArgumentNullException("subCollection");

            if (subCollection.Index > _subCollections.Length - 1)
                return false;

            if (_subCollections[subCollection.Index] != subCollection)
                return false;

            _subCollections[subCollection.Index] = null;
            for (int i = 0; i < _count; i++)
            {
                if (_indexes[i] == subCollection.Index)
                {
                    _indexes[i] = 0;
                    break;
                }
            }
            _count--;
            return true;
        }

        /// <summary>
        /// Removes the <see cref="CsvDataSubCollection{TCsvData}"/> with the specified data ID from the <see cref="CsvDataCollection{TCsvData}"/>.
        /// </summary>
        /// <param name="id">Data ID of the <see cref="CsvDataSubCollection{TCsvData}"/> to remove.</param>
        /// <returns>
        /// Returns <c>true</c> if the <see cref="CsvDataSubCollection{TCsvData}"/> with the specified data ID
        /// was successfully removed; otherwise, <c>false</c>.
        /// </returns>
        /// 
        /// <exception cref="InvalidOperationException"><see cref="IsReadOnly"/> is set to <c>true</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="id"/> is not within the range of <typeparamref name="TCsvData"/>.</exception>
        public bool Remove(int id)
        {
            if (IsReadOnly)
                throw new InvalidOperationException("CsvDataCollection object is readonly.");
            if (_instance.InvalidDataID(id))
                throw new ArgumentOutOfRangeException("id", _instance.GetArgsOutOfRangeMessage("id"));

            var index = _instance.GetIndex(id);
            if (index > _subCollections.Length - 1)
                return false;

            if (_subCollections[index] == null)
                return false;

            _subCollections[index] = null;
            for (int i = 0; i < _count; i++)
            {
                if (_indexes[i] == index)
                {
                    _indexes[i] = 0;
                    break;
                }
            }
            _count--;
            return true;
        }

        /// <summary>
        /// Removes the <see cref="CsvDataSubCollection{TCsvData}"/> with the specified text ID from the <see cref="CsvDataCollection{TCsvData}"/>.
        /// </summary>
        /// <param name="tid">Text ID of the <see cref="CsvDataSubCollection{TCsvData}"/> to remove.</param>
        /// <returns>
        /// Returns <c>true</c> if the <see cref="CsvDataSubCollection{TCsvData}"/> with the specified text ID
        /// was successfully removed; otherwise, <c>false</c>.
        /// </returns>
        /// 
        /// <exception cref="InvalidOperationException"><see cref="IsReadOnly"/> is set to <c>true</c>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="tid"/> is null.</exception>
        public bool Remove(string tid)
        {
            if (IsReadOnly)
                throw new InvalidOperationException("CsvDataCollection object is readonly.");
            if (tid == null)
                throw new ArgumentNullException("tid");

            var removed = false;
            for (int i = 0; i < _count; i++)
            {
                if (_subCollections[_indexes[i]].TID == tid)
                {
                    _subCollections[_indexes[i]] = null;
                    _indexes[i] = 0;
                    _count--;
                    removed = true;
                }
            }
            return removed;
        }

        /// <summary>
        /// Removes all <see cref="CsvDataSubCollection{TCsvData}"/> from the <see cref="CsvDataCollection{TCsvData}"/>.
        /// </summary>
        /// <exception cref="InvalidOperationException"><see cref="IsReadOnly"/> is set to <c>true</c>.</exception>
        public void Clear()
        {
            if (IsReadOnly)
                throw new InvalidOperationException("CsvDataCollection object is readonly.");

            for (int i = 0; i < _count; i++)
            {
                _subCollections[_indexes[i]] = null;
                _indexes[i] = 0;
            }
            _count = 0;
        }

        /// <summary>
        /// Determines whether the <see cref="CsvDataCollection{TCsvData}"/> contains a specific <see cref="CsvDataSubCollection{TCsvData}"/>.
        /// </summary>
        /// <param name="subCollection">
        /// The <see cref="CsvDataSubCollection{TCsvData}"/> to locate in the <see cref="CsvDataCollection{TCsvData}"/>.
        /// </param>
        /// <returns>
        /// <c>true</c> if the <paramref name="subCollection"/> was found in the <see cref="CsvDataCollection{TCsvData}"/>; otherwise, <c>false</c>.
        /// </returns>
        /// 
        /// <exception cref="ArgumentNullException"><paramref name="subCollection"/> is null.</exception>
        public bool Contains(CsvDataSubCollection<TCsvData> subCollection)
        {
            if (subCollection == null)
                throw new ArgumentNullException("subCollection");

            if (subCollection.Index > _subCollections.Length - 1)
                return false;

            if (_subCollections[subCollection.Index] != subCollection)
                return false;

            return true;
        }

        /// <summary>
        /// Determines whether the <see cref="CsvDataCollection{TCsvData}"/> contains a <see cref="CsvDataSubCollection{TCsvData}"/>
        /// with the specified data ID.
        /// </summary>
        /// <param name="id">
        /// The data ID of <see cref="CsvDataSubCollection{TCsvData}"/> to locate in the <see cref="CsvDataCollection{TCsvData}"/>.
        /// </param>
        /// <returns>
        /// <c>true</c> if the <see cref="CsvDataSubCollection{TCsvData}"/> with the specified data ID
        /// was found in the <see cref="CsvDataCollection{TCsvData}"/>; otherwise, <c>false</c>.
        /// </returns>
        /// 
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="id"/> is not within the range <typeparamref name="TCsvData"/>.</exception>
        public bool Contains(int id)
        {
            if (_instance.InvalidDataID(id))
                throw new ArgumentOutOfRangeException("id", _instance.GetArgsOutOfRangeMessage("id"));

            var index = _instance.GetIndex(id);
            if (index > _subCollections.Length - 1)
                return false;

            if (_subCollections[index] == null)
                return false;

            return true;
        }

        /// <summary>
        /// Determines whether the <see cref="CsvDataCollection{TCsvData}"/> contains a <see cref="CsvDataSubCollection{TCsvData}"/>
        /// with the specified text ID.
        /// </summary>
        /// <param name="tid">
        /// The text ID of <see cref="CsvDataSubCollection{TCsvData}"/> to locate in the <see cref="CsvDataCollection{TCsvData}"/>.
        /// </param>
        /// <returns>
        /// <c>true</c> if the <see cref="CsvDataSubCollection{TCsvData}"/> with the specified text id
        /// was found in the <see cref="CsvDataCollection{TCsvData}"/>; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="tid"/> is null.</exception>
        public bool Contains(string tid)
        {
            if (tid == null)
                throw new ArgumentNullException("tid");

            for (int i = 0; i < _count; i++)
            {
                if (_subCollections[_indexes[i]].TID == tid)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Copies the entire <see cref="CsvDataCollection{TCsvData}"/> to a specified compatible one-dimensional array, at the specified
        /// index to array.
        /// </summary>
        /// <param name="array">Target one-dimensional array to copy the <see cref="CsvDataCollection{TCsvData}"/></param>
        /// <param name="arrayIndex">Index at which to start copying the <see cref="CsvDataCollection{TCsvData}"/> in the target array.</param>
        /// 
        /// <exception cref="ArgumentNullException"><paramref name="array"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="arrayIndex"/> is less than 0.</exception>
        /// <exception cref="ArgumentException"><paramref name="array"/> is too small.</exception>
        public void CopyTo(CsvDataSubCollection<TCsvData>[] array, int arrayIndex)
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
                array[i] = _subCollections[_indexes[j]];
                j++;
            }
        }

        /// <summary>
        /// Returns an enumerator that iterates through the <see cref="CsvDataCollection{TCsvData}"/>.
        /// </summary>
        /// <returns>An <see cref="IEnumerable{TCsvData}"/> for the <see cref="CsvDataCollection{TCsvData}"/>.</returns>
        public IEnumerator<CsvDataSubCollection<TCsvData>> GetEnumerator()
        {
            return new Enumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private class Enumerator : IEnumerator<CsvDataSubCollection<TCsvData>>
        {
            public Enumerator(CsvDataCollection<TCsvData> collection)
            {
                _position = -1;
                _collection = collection;
            }

            private readonly CsvDataCollection<TCsvData> _collection;
            private int _position;

            public CsvDataSubCollection<TCsvData> Current
            {
                get
                {
                    return _collection._subCollections[_collection._indexes[_position]];
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
