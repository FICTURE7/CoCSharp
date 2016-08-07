using System;
using System.Collections;
using System.Collections.Generic;

namespace CoCSharp.Logic
{
    internal class VillageObjectCollection : ICollection<VillageObject>
    {
        public VillageObjectCollection()
        {
            _sync = new object();
            _table = new List<VillageObject>[8];
            for (int i = 0; i < _table.Length; i++)
                _table[i] = new List<VillageObject>();
        }

        // The syncing object.
        private object _sync;
        // Table of VillageObjects.
        private List<VillageObject>[] _table;

        bool ICollection<VillageObject>.IsReadOnly { get { return false; } }

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

        public void Add(VillageObject villageObj)
        {
            if (villageObj == null)
                throw new ArgumentNullException("villageObj");

            lock (_sync)
            {
                AddInternal(villageObj);
            }
        }

        public bool Remove(VillageObject villageObj)
        {
            if (villageObj == null)
                throw new ArgumentNullException("villageObj");

            return Remove(villageObj.ID);
        }

        public bool Remove(int gameId)
        {
            var rowIndex = GetRowIndex(gameId);
            var columnIndex = GetColumnIndex(gameId, rowIndex);

            lock (_sync)
            {
                return RemoveInternal(rowIndex, columnIndex);
            }
        }

        public void Clear()
        {
            lock (_sync)
            {
                for (int i = 0; i < _table.Length; i++)
                    _table[i].Clear();
            }
        }

        public bool Contains(VillageObject villageObj)
        {
            if (villageObj == null)
                throw new ArgumentNullException("villageObj");

            lock (_sync)
            {
                return Contains(villageObj.ID);
            }
        }

        public bool Contains(int gameId)
        {
            var rowIndex = GetRowIndex(gameId);
            var columnIndex = GetColumnIndex(gameId, rowIndex);

            lock (_sync)
            {
                return ContainsInternal(rowIndex, columnIndex);
            }
        }        

        public IEnumerator<VillageObject> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        void ICollection<VillageObject>.CopyTo(VillageObject[] array, int arrayIndex)
        {
            throw new NotSupportedException();
        }

        private void AddInternal(VillageObject villageObj)
        {
            var rowIndex = villageObj.KindID;
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
            row.RemoveAt(columnIndex);
            for (; columnIndex < row.Count; columnIndex++)
                row[columnIndex]._columnIndex--;

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
            return (gameId / 1000000) - 500;
        }

        private static int GetColumnIndex(int gameId, int rowIndex)
        {
            // return gameId - ((500 + GetRowIndex(gameId)) * 1000000);
            return gameId - ((500 + rowIndex) * 1000000);
        }
    }
}
