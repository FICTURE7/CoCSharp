using System;
using System.Collections;
using System.Collections.Generic;

namespace CoCSharp.Logic
{
    /// <summary>
    /// Represents a collection of <see cref="VillageObject"/>.
    /// </summary>
    public sealed class VillageObjectCollection : ICollection<VillageObject>
    {
        private const int MinID = 500 * InternalConstants.IdBase;

        internal VillageObjectCollection()
        {
            _sync = new object();
            _table = new List<VillageObject>[8];
            for (int i = 0; i < _table.Length; i++)
                _table[i] = new List<VillageObject>();
        }

        // The syncing object.
        private readonly object _sync;
        // Table of VillageObjects.
        private List<VillageObject>[] _table;

        bool ICollection<VillageObject>.IsReadOnly => false;

        /// <summary>
        /// Gets or sets a <see cref="VillageObject"/> with the specified game ID.
        /// </summary>
        /// <param name="id">Game ID of the <see cref="VillageObject"/> to get or set.</param>
        /// <returns>The <see cref="VillageObject"/> with the specified game ID.</returns>
        /// 
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="id"/> is less than 500000000.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="value"/>.</exception>
        public VillageObject this[int id]
        {
            get
            {
                if (id < MinID)
                    throw new ArgumentOutOfRangeException("id", "id must be greater than " + MinID + ".");

                var rowIndex = GetRowIndex(id);
                var columnIndex = GetColumnIndex(id, rowIndex);
                lock (_sync)
                {
                    return GetInternal(rowIndex, columnIndex);
                }
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");

                var rowIndex = GetRowIndex(id);
                var columnIndex = GetColumnIndex(id, rowIndex);
                lock (_sync)
                {
                    if (!SetInternal(rowIndex, columnIndex, value))
                        throw new ArgumentOutOfRangeException("id");

                    value._columnIndex = columnIndex;
                }
            }
        }

        /// <summary>
        /// Gets the total number of <see cref="VillageObject"/> in the <see cref="VillageObjectCollection"/>.
        /// </summary>
        public int Count
        {
            get
            {
                lock (_sync)
                {
                    var count = 0;
                    for (int i = 0; i < _table.Length; i++)
                        count += _table[i].Count;

                    return count;
                }
            }
        }

        /// <summary>
        /// Adds the specified <see cref="VillageObject"/> to the <see cref="VillageObjectCollection"/>.
        /// </summary>
        /// <param name="villageObj"><see cref="VillageObject"/> to add to the <see cref="VillageObjectCollection"/>.</param>
        /// 
        /// <exception cref="ArgumentNullException"><paramref name="villageObj"/> is null.</exception>
        public void Add(VillageObject villageObj)
        {
            if (villageObj == null)
                throw new ArgumentNullException(nameof(villageObj));

            lock (_sync)
            {
                AddInternal(villageObj);
            }
        }

        /// <summary>
        /// Removes the specified <see cref="VillageObject"/> from the <see cref="VillageObjectCollection"/> by using
        /// its <see cref="VillageObject.Id"/> value.
        /// </summary>
        /// <param name="villageObj"><see cref="VillageObject"/> to remove from the <see cref="VillageObjectCollection"/>.</param>
        /// <returns><c>true</c> if success; otherwise, returns <c>false</c>.</returns>
        /// 
        /// <exception cref="ArgumentNullException"><paramref name="villageObj"/> is null.</exception>
        public bool Remove(VillageObject villageObj)
        {
            if (villageObj == null)
                throw new ArgumentNullException("villageObj");

            var gameId = villageObj.Id;
            var rowIndex = GetRowIndex(gameId);
            var columnIndex = GetColumnIndex(gameId, rowIndex);
            lock (_sync)
            {
                return RemoveInternal(rowIndex, columnIndex);
            }
        }

        /// <summary>
        /// Removes a <see cref="VillageObject"/> with the specified game ID from the <see cref="VillageObjectCollection"/>.
        /// </summary>
        /// <param name="gameId">Game ID of the <see cref="VillageObject"/> to remove from the <see cref="VillageObjectCollection"/>.</param>
        /// <returns><c>true</c> if success; otherwise, returns <c>false</c>.</returns>
        public bool Remove(int gameId)
        {
            if (gameId < MinID)
                throw new ArgumentOutOfRangeException("gameId", "gameId must be greater than " + MinID + ".");

            var rowIndex = GetRowIndex(gameId);
            var columnIndex = GetColumnIndex(gameId, rowIndex);
            lock (_sync)
            {
                return RemoveInternal(rowIndex, columnIndex);
            }
        }

        /// <summary>
        /// Removes all <see cref="VillageObject"/> from the <see cref="VillageObjectCollection"/>.
        /// </summary>
        public void Clear()
        {
            lock (_sync)
            {
                for (int i = 0; i < _table.Length; i++)
                    _table[i].Clear();
            }
        }

        /// <summary>
        /// Determines whether the <see cref="VillageObjectCollection"/> contains a specific <see cref="VillageObject"/> by using
        /// its <see cref="VillageObject.Id"/>.
        /// </summary>
        /// <param name="villageObj"><see cref="VillageObject"/> to locate.</param>
        /// <returns>
        /// <c>true</c> if the <paramref name="villageObj"/> was found in the <see cref="VillageObjectCollection"/>; otherwise, <c>false</c>.
        /// </returns>
        /// 
        /// <exception cref="ArgumentNullException"><paramref name="villageObj"/> is null.</exception>
        public bool Contains(VillageObject villageObj)
        {
            if (villageObj == null)
                throw new ArgumentNullException("villageObj");

            lock (_sync)
            {
                return Contains(villageObj.Id);
            }
        }

        /// <summary>
        /// Determines whether the <see cref="VillageObjectCollection"/> contains a <see cref="VillageObject"/> with the specified game ID. 
        /// </summary>
        /// <param name="gameId"><see cref="VillageObject"/> with the specified game ID to locate.</param>
        /// <returns>
        /// <c>true</c> if a <see cref="VillageObject"/> with the same game ID was found in the <see cref="VillageObjectCollection"/>; otherwise, <c>false</c>.
        /// </returns>
        /// 
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="gameId"/> is less than 500000000.</exception>
        public bool Contains(int gameId)
        {
            if (gameId < MinID)
                throw new ArgumentOutOfRangeException("gameId", "gameId must be greater than " + MinID + ".");

            var rowIndex = GetRowIndex(gameId);
            var columnIndex = GetColumnIndex(gameId, rowIndex);
            lock (_sync)
            {
                return ContainsInternal(rowIndex, columnIndex);
            }
        }

        /// <summary>
        /// Returns an enumerator that iterates through the <see cref="VillageObjectCollection"/>.
        /// </summary>
        /// <returns>An <see cref="IEnumerable{VillageObject}"/> that iterates through the <see cref="VillageObjectCollection"/>.</returns>
        public IEnumerator<VillageObject> GetEnumerator()
        {
            lock (_sync)
            {
                for (int i = 0; i < _table.Length; i++)
                {
                    var row = _table[i];
                    for (int j = 0; j < row.Count; j++)
                    {
                        var obj = row[j];
                        if (obj != null)
                            yield return obj;
                    }
                }
            }
        }        

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        void ICollection<VillageObject>.CopyTo(VillageObject[] array, int arrayIndex)
        {
            throw new NotSupportedException();
        }

        internal List<VillageObject> GetRow(int kind)
        {
            lock (_sync)
            {
                return _table[kind];
            }
        }

        // Calls Tick(int) method with the specified tick to all VillageObject in the collection. 
        internal void TickAll(int tick)
        {
            lock (_sync)
            {
                for (int i = 0; i < _table.Length; i++)
                {
                    var row = _table[i];
                    for (int k = 0; k < row.Count; k++)
                        row[k]?.Tick(tick);
                }
            }
        }

        #region Internals
        private VillageObject GetInternal(int rowIndex, int columnIndex)
        {
            if (rowIndex > _table.Length - 1)
                return null;

            var row = _table[rowIndex];
            if (columnIndex > row.Count - 1)
                return null;

            return row[columnIndex];
        }

        private bool SetInternal(int rowIndex, int columnIndex, VillageObject newValue)
        {
            if (rowIndex > _table.Length - 1)
                return false;

            var row = _table[rowIndex];
            if (columnIndex > row.Count - 1)
                return false;

            row[columnIndex] = newValue;
            return true;
        }

        private void AddInternal(VillageObject villageObj)
        {
            var rowIndex = villageObj.KindId;
            var tableLength = _table.Length - 1;
            if (rowIndex > tableLength)
                Array.Resize(ref _table, rowIndex + 4);

            var row = _table[rowIndex];
            villageObj._columnIndex = row.Count;
            row.Add(villageObj);
        }

        private bool RemoveInternal(int rowIndex, int columnIndex)
        {
            if (rowIndex > _table.Length - 1)
                return false;

            var row = _table[rowIndex];
            if (columnIndex > row.Count - 1)
                return false;

            row[columnIndex]._columnIndex = -1;
            row[columnIndex].PushToPool();
            row[columnIndex] = null;
            return true;
        }

        private bool ContainsInternal(int rowIndex, int columnIndex)
        {
            if (rowIndex > _table.Length - 1)
                return false;

            var row = _table[rowIndex];
            if (columnIndex > row.Count - 1)
                return false;

            return true;
        }

        private static int GetRowIndex(int gameId)
        {
            return (gameId / InternalConstants.IdBase) - 500;
        }

        private static int GetColumnIndex(int gameId, int rowIndex)
        {
            // return gameId - ((500 + GetRowIndex(gameId)) * InternalConstants.IDBase);
            return gameId - ((500 + rowIndex) * InternalConstants.IdBase);
        }
        #endregion
    }
}
