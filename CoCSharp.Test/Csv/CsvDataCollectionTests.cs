using CoCSharp.Csv;
using CoCSharp.Data.Models;
using NUnit.Framework;
using System;
using System.Linq;

namespace CoCSharp.Test.Csv
{
    [TestFixture]
    public class CsvDataCollectionTests
    {
        [SetUp]
        public void SetUp()
        {
            _collection = new CsvDataCollection<BuildingData>();
        }

        private CsvDataCollection<BuildingData> _collection;

        #region Indexer[int]
        [Test]
        public void IndexerInt_Getter__InvalidID_Exception()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => { var d = _collection[0]; });
            Assert.Throws<ArgumentOutOfRangeException>(() => { var d = _collection[-999]; });
        }

        [Test]
        public void IndexerInt_Getter__ItemNotFound_ReturnsNull()
        {
            Assert.Null(_collection[1000000]);
        }

        [Test]
        public void IndexerInt_Getter__ItemFound_ReturnsItem()
        {
            var subcollection = new CsvDataSubCollection<BuildingData>(1000000, "SOME_TID");
            _collection.Add(subcollection);

            Assert.AreSame(subcollection, _collection[1000000]);
        }

        [Test]
        public void IndexerInt_Setter__InvalidID_Exception()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => { _collection[0] = new CsvDataSubCollection<BuildingData>(1000000, "SOME_TID"); });
            Assert.Throws<ArgumentOutOfRangeException>(() => { _collection[-999] = new CsvDataSubCollection<BuildingData>(1000000, "SOME_TID"); });
        }

        [Test]
        public void IndexerInt_Setter__Null_Exception()
        {
            Assert.Throws<ArgumentNullException>(() => { _collection[1000000] = null; });
        }

        [Test]
        public void IndexerInt_Setter__DataIDNotCoresponding_Exception()
        {
            Assert.Throws<ArgumentException>(() => { _collection[1000000] = new CsvDataSubCollection<BuildingData>(1000004, "SOME_TID"); });
        }

        [Test]
        public void IndexerInt_Setter__AddItem_ItemAdded()
        {
            var subcollection1 = new CsvDataSubCollection<BuildingData>(1000004, "SOME_TID");
            _collection[1000004] = subcollection1;

            Assert.AreEqual(1, _collection.Count);

            var subcollection2 = new CsvDataSubCollection<BuildingData>(1000504, "SOME_TID_NEW");
            _collection[1000504] = subcollection2;

            Assert.AreEqual(2, _collection.Count);
        }
        #endregion

        #region Indexer[string]
        [Test]
        public void IndexerString_Getter__Null_Exception()
        {
            Assert.Throws<ArgumentNullException>(() => { var d = _collection[null]; });
        }

        [Test]
        public void IndexerString_Getter_AddItem_ItemAdded()
        {
            Assert.AreEqual(0, _collection.Count);
            var subcollection1 = new CsvDataSubCollection<BuildingData>(1000004, "SOME_TID");
            _collection["SOME_TID"] = subcollection1;

            Assert.AreEqual(1, _collection.Count);
        }
        #endregion

        #region Add
        [Test]
        public void Add_NullArgs_Exception()
        {
            Assert.Throws<ArgumentNullException>(() => _collection.Add(null));
        }

        [Test]
        public void Add__Contains_ItemWith_Same_IDnTID__Exception()
        {
            var subcollection1 = new CsvDataSubCollection<BuildingData>(1000000, "SOME_TID");
            _collection.Add(subcollection1);
            Assert.AreEqual(1, _collection.Count);

            var subcollection2 = new CsvDataSubCollection<BuildingData>(1000000, "SOME_TID");
            Assert.Throws<ArgumentException>(() => _collection.Add(subcollection2));
        }

        [Test]
        public void Add_ValidItem_Added()
        {
            var subcollection1 = new CsvDataSubCollection<BuildingData>(1000000, "SOME_TID");
            _collection.Add(subcollection1);
            Assert.AreEqual(1, _collection.Count);
            Assert.AreEqual(0, _collection._indexes[0]);

            var subcollection2 = new CsvDataSubCollection<BuildingData>(1000001, "SOME_TID_NEW");
            _collection.Add(subcollection2);
            Assert.AreEqual(2, _collection.Count);
            Assert.AreEqual(1, _collection._indexes[1]);

            var subcollection3 = new CsvDataSubCollection<BuildingData>(1009999, "SOME_TID_NEW2");
            _collection.Add(subcollection3);
            Assert.AreEqual(3, _collection.Count);
            Assert.AreEqual(9999, _collection._indexes[2]);
        }
        #endregion

        #region Remove
        [Test]
        public void Remove_Null_Exception()
        {
            Assert.Throws<ArgumentNullException>(() => _collection.Remove(subCollection: null));
        }

        [Test]
        public void Remove_ItemNotFound_ReturnsFalse()
        {
            var subcollection = new CsvDataSubCollection<BuildingData>(1000000, "SOME_TID");
            Assert.False(_collection.Remove(subcollection));
        }

        [Test]
        public void Remove_ItemFound_ReturnsTrue()
        {
            var subcollection = new CsvDataSubCollection<BuildingData>(1000000, "SOME_TID");
            _collection.Add(subcollection);
            Assert.AreEqual(1, _collection.Count);

            Assert.True(_collection.Remove(subcollection));
            Assert.AreEqual(0, _collection.Count);
        }
        #endregion

        #region Remove(int)
        [Test]
        public void RemoveInt_InvalidID_Exception()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => _collection.Remove(0));
        }

        [Test]
        public void RemoveInt_ItemFound_ReturnsTrue()
        {
            var subcollection1 = new CsvDataSubCollection<BuildingData>(1000005, "SOME_TID");
            var subcollection2 = new CsvDataSubCollection<BuildingData>(1000009, "SOME_TID_NEW");
            _collection.Add(subcollection1);
            _collection.Add(subcollection2);
            Assert.AreEqual(5, _collection._indexes[0]);
            Assert.AreEqual(2, _collection.Count);

            Assert.True(_collection.Remove(1000005));
            Assert.AreEqual(1, _collection.Count);
            Assert.AreEqual(0, _collection._indexes[0]);
            Assert.AreEqual(9, _collection._indexes[1]);
        }
        #endregion

        #region Remove(string)
        [Test]
        public void RemoveString_Null_Exception()
        {
            Assert.Throws<ArgumentNullException>(() => _collection.Remove(tid: null));
        }

        [Test]
        public void RemoveString_ItemNotFound_ReturnsFalse()
        {
            Assert.False(_collection.Remove("UNKNOWN_TID"));
        }

        [Test]
        public void RemoveString_ItemFound_ReturnsTrue()
        {
            var subcollection = new CsvDataSubCollection<BuildingData>(1000005, "SOME_TID");
            _collection.Add(subcollection);
            Assert.AreEqual(5, _collection._indexes[0]);
            Assert.AreEqual(1, _collection.Count);

            Assert.True(_collection.Remove("SOME_TID"));
            Assert.AreEqual(0, _collection.Count);
            Assert.AreEqual(0, _collection._indexes[0]);
        }
        #endregion

        #region Clear
        [Test]
        public void Clear_()
        {
            for (int i = 0; i < 5; i++)
            {
                var subcollection = new CsvDataSubCollection<BuildingData>(1000000 + i, "SOME_TID" + i);
                _collection.Add(subcollection);
                Assert.AreEqual(i, _collection._indexes[i]);
            }
            Assert.AreEqual(5, _collection.Count);

            _collection.Clear();
            Assert.AreEqual(0, _collection.Count);
            for (int i = 0; i < 5; i++)
                Assert.AreEqual(0, _collection._indexes[i]);
        }
        #endregion

        #region Contains
        [Test]
        public void Contains_Null_Exception()
        {
            Assert.Throws<ArgumentNullException>(() => _collection.Contains(subCollection: null));
        }

        [Test]
        public void Contains_ItemNotFound_ReturnsFalse()
        {
            var subcollection = new CsvDataSubCollection<BuildingData>(1000007, "SOME_TID");

            Assert.False(_collection.Contains(subcollection));
        }

        [Test]
        public void Contains_ItemFound_ReturnsTrue()
        {
            var subcollection = new CsvDataSubCollection<BuildingData>(1000003, "SOME_TID");
            _collection.Add(subcollection);

            Assert.True(_collection.Contains(subcollection));
        }
        #endregion

        #region Contains(int)
        [Test]
        public void ContainsInt_InvalidID_Exception()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => _collection.Contains(0));
        }

        [Test]
        public void ContainsInt_ItemNotFound_ReturnsFalse()
        {
            Assert.False(_collection.Contains(1000009));
        }

        [Test]
        public void ContainsInt_ItemFound_ReturnsTrue()
        {
            var subcollection = new CsvDataSubCollection<BuildingData>(1000011, "SOME_TID");
            _collection.Add(subcollection);

            Assert.True(_collection.Contains(1000011));
        }
        #endregion

        #region Contains(string)
        [Test]
        public void ContainsString_Null_Exception()
        {
            Assert.Throws<ArgumentNullException>(() => _collection.Contains(tid: null));
        }

        [Test]
        public void ContainsString_ItemNotFound_ReturnsFalse()
        {
            Assert.False(_collection.Contains("UNKNOWN_TID"));
        }

        [Test]
        public void ContainsString_ItemFound_ReturnsTrue()
        {
            var subcollection = new CsvDataSubCollection<BuildingData>(1000001, "SOME_TID");
            _collection.Add(subcollection);

            Assert.True(_collection.Contains("SOME_TID"));
        }
        #endregion

        #region CopyTo
        // Same as CsvDataSubCollectionTests
        #endregion

        #region GetEnumrator
        [Test]
        public void GetEnumerator_Contains5Items_Iterates5Times()
        {
            for (int i = 0; i < 5; i++)
            {
                var subcollection = new CsvDataSubCollection<BuildingData>(1000000 + i, "SOME_TID" + i);
                _collection.Add(subcollection);
                Assert.AreEqual(i, _collection._indexes[i]);
            }
            Assert.AreEqual(5, _collection.Count);

            var j = 0;
            foreach (var subCollection in _collection)
            {
                Assert.AreEqual(j, subCollection.Index);
                j++;
            }
        }

        [Test]
        public void GetEnumerator_Linq()
        {
            for (int i = 0; i < 5; i++)
            {
                var subcollection = new CsvDataSubCollection<BuildingData>(1000000 + i, "SOME_TID" + i);
                _collection.Add(subcollection);
                Assert.AreEqual(i, _collection._indexes[i]);
            }
            Assert.AreEqual(5, _collection.Count);

            var count = _collection.Count(b => b.Index > 2);
            Assert.AreEqual(2, count);
        }
        #endregion
    }
}
