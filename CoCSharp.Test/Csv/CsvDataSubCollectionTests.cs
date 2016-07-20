using CoCSharp.Csv;
using CoCSharp.Data.Models;
using NUnit.Framework;
using System;
using System.Linq;

namespace CoCSharp.Test.Csv
{
    [TestFixture]
    public class CsvDataSubCollectionTests
    {
        #region SetUp
        [SetUp]
        public void SetUp()
        {
            _collection = new CsvDataSubCollection<BuildingData>(1000000, "SOME_TID");
        }

        private CsvDataSubCollection<BuildingData> _collection;
        #endregion

        #region Constructors
        [Test]
        public void Constructors_InvalidArgs_Exception()
        {
            // id
            Assert.Throws<ArgumentOutOfRangeException>(() => new CsvDataSubCollection<BuildingData>(999999, "SOME_TID"));
            Assert.Throws<ArgumentOutOfRangeException>(() => new CsvDataSubCollection<BuildingData>(2000000, "SOME_TID"));

            // tid
            Assert.Throws<ArgumentNullException>(() => new CsvDataSubCollection<BuildingData>(1000000, null));
        }

        [Test]
        public void Constructors_ValidArgs_TIDnID()
        {
            var col1 = new CsvDataSubCollection<BuildingData>(1000000, "SOME_TID");
            Assert.AreEqual("SOME_TID", col1.TID);
            Assert.AreEqual(1000000, col1.ID);

            var col2 = new CsvDataSubCollection<BuildingData>(1999999, "SOME_TID");
            Assert.AreEqual("SOME_TID", col2.TID);
            Assert.AreEqual(1999999, col2.ID);
        }

        [Test]
        public void Constructors_TCsvDataInstanceCached_CachedInstanceUsed()
        {
            var instance1 = _collection._instance;
            var col1 = new CsvDataSubCollection<BuildingData>(1000000, "SOME_TID");
            var instance2 = col1._instance;

            Assert.AreSame(instance1, instance2);
        }

        [Test, Ignore("Test a bit inconsistent.")]
        public void Constructors_TCsvDataInstanceCacheCollected_CreateNewInstance()
        {
            var weakRef = new WeakReference(_collection._instance);
            Assert.True(weakRef.IsAlive);

            // Set _collection to null to remove all references to _instance
            // so that the GC can reclaim it.
            _collection = null;

            GC.Collect();
            Assert.False(weakRef.IsAlive);

            var col = new CsvDataSubCollection<BuildingData>(1000000, "SOME_TID");
            var instance = col._instance;

            Assert.Null(weakRef.Target);
            Assert.NotNull(instance);
        }
        #endregion

        #region Indexer
        [Test]
        public void Indexer_Getter__ItemNotFound_ReturnsNull()
        {
            Assert.Null(_collection[0]);
            Assert.Null(_collection[999]);
        }

        [Test]
        public void Indexer_Getter__ItemFound_ReturnsItem()
        {
            var data = new BuildingData();
            data.ID = 1000000;
            data.TID = "SOME_TID";
            data.Level = 1;
            _collection.Add(data);

            var ndata = _collection[1];
            Assert.NotNull(_collection[1]);
            Assert.AreSame(data, ndata);
            Assert.True(ndata._isInCollection);
        }

        [Test]
        public void Indexer_Getter__NegativeLevel_Exception()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => { var d = _collection[-1]; });
            Assert.Throws<ArgumentOutOfRangeException>(() => { var d = _collection[-999]; });
        }

        [Test]
        public void Indexer_Setter__Null_Exception()
        {
            Assert.Throws<ArgumentNullException>(() => _collection[0] = null);
        }

        [Test]
        public void Indexer_Setter__DifferingID_Exception()
        {
            var data = new BuildingData();
            data.ID = 1000001; // Not same.
            data.TID = "SOME_TID";

            Assert.Throws<ArgumentException>(() => _collection[0] = data);
        }

        [Test]
        public void Indexer_Setter__DifferingTID_Exception()
        {
            var data = new BuildingData();
            data.ID = 1000000;
            data.TID = "DIFFERING_TID";

            Assert.Throws<ArgumentException>(() => _collection[0] = data);
        }

        [Test]
        public void Indexer_Setter__NegativeLevel_Exception()
        {
            var data = new BuildingData();
            data.ID = 1000000;
            data.TID = "SOME_TID";

            Assert.Throws<ArgumentOutOfRangeException>(() => _collection[-1] = data);
        }

        [Test]
        public void Indexer_Setter__ItemAlreadyInAnotherCollection_Exception()
        {
            var data = new BuildingData();
            data.ID = 1000000;
            data.TID = "SOME_TID";

            var collection2 = new CsvDataSubCollection<BuildingData>(1000000, "SOME_TID");
            Assert.False(data._isInCollection);

            collection2.Add(data);
            Assert.True(data._isInCollection);

            // Item already added to collection2 so we throw an exception.
            Assert.Throws<ArgumentException>(() => _collection[0] = data);
        }

        [Test]
        public void Indexer_Setter__ItemFound_ReturnsItem()
        {
            var data = new BuildingData();
            data.ID = 1000000;
            data.TID = "SOME_TID";

            _collection[0] = data;
            Assert.True(data._isInCollection);
        }
        #endregion

        #region Add
        [Test]
        public void Add_IsReadOnly_Exception()
        {
            _collection.IsReadOnly = true;
            Assert.Throws<InvalidOperationException>(() => _collection.Add(null));
        }

        [Test]
        public void Add_Null_Exception()
        {
            Assert.Throws<ArgumentNullException>(() => _collection.Add(null));
        }

        [Test]
        public void Add_DifferingID_Exception()
        {
            var data = new BuildingData();
            data.ID = 1000001; // Not same.
            data.TID = "SOME_TID";
            Assert.Throws<ArgumentException>(() => _collection.Add(data));
        }

        [Test]
        public void Add_DifferingTID_Exception()
        {
            var data = new BuildingData();
            data.ID = 1000000; // Same.
            data.TID = "DIFFERING_TID";
            Assert.Throws<ArgumentException>(() => _collection.Add(data));
        }

        [Test]
        public void Add_ItemWithSameLevel_Exception()
        {
            var data1 = new BuildingData();
            data1.ID = 1000000;
            data1.TID = "SOME_TID";
            data1.Level = 1;

            var data2 = new BuildingData();
            data2.ID = 1000000;
            data2.TID = "SOME_TID";
            data2.Level = 1;

            _collection.Add(data1);
            Assert.Throws<ArgumentException>(() => _collection.Add(data2));
        }

        [Test]
        public void Add_ValidItem_Added()
        {
            var data = new BuildingData();
            data.ID = 1000000;
            data.TID = "SOME_TID";

            _collection.Add(data);
            Assert.AreEqual(true, data._isInCollection);
            Assert.AreEqual(1, _collection.Count);
        }
        #endregion

        #region Remove(TCsvData)
        [Test]
        public void Remove_IsReadOnly_Exception()
        {
            _collection.IsReadOnly = true;
            Assert.Throws<InvalidOperationException>(() => _collection.Remove(null));
        }

        [Test]
        public void Remove_Null_Exception()
        {
            Assert.Throws<ArgumentNullException>(() => _collection.Remove(null));
        }

        [Test]
        public void Remove_DifferingID_Exception()
        {
            var data = new BuildingData();
            data.ID = 1000001;
            data.TID = "SOME_TID";

            Assert.Throws<ArgumentException>(() => _collection.Remove(data));
        }

        [Test]
        public void Remove_DifferingTID_Exception()
        {
            var data = new BuildingData();
            data.ID = 1000000;
            data.TID = "DIFFERING_TID";

            Assert.Throws<ArgumentException>(() => _collection.Remove(data));
        }

        [Test]
        public void Remove_ItemFound_ReturnsTrue()
        {
            var data1 = new BuildingData();
            data1.ID = 1000000;
            data1.TID = "SOME_TID";
            data1.Level = 0;
            _collection.Add(data1);
            Assert.AreEqual(1, _collection.Count);
            Assert.AreEqual(0, _collection._indexes[0]);

            var data2 = new BuildingData();
            data2.ID = 1000000;
            data2.TID = "SOME_TID";
            data2.Level = 999;
            _collection.Add(data2);

            Assert.AreEqual(999, _collection._indexes[1]);

            // Data is in collection.
            Assert.True(data1._isInCollection);
            Assert.True(_collection.Remove(data1));
            // Data is no more in collection.
            Assert.False(data1._isInCollection);
            Assert.AreEqual(1, _collection.Count);
        }
        #endregion

        #region Remove(int)
        [Test]
        public void RemoveInt_IsReadOnly_Exception()
        {
            _collection.IsReadOnly = true;
            Assert.Throws<InvalidOperationException>(() => _collection.Remove(0));
        }

        [Test]
        public void RemoveInt_NegativeLevel_Exception()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => _collection.Remove(-1));
            Assert.Throws<ArgumentOutOfRangeException>(() => _collection.Remove(-999));
        }

        [Test]
        public void RemoveInt_ItemWithLevelNotFound_ReturnFalse()
        {
            Assert.False(_collection.Remove(0));
            Assert.False(_collection.Remove(999));
        }

        [Test]
        public void RemoveInt_ItemFound_ReturnTrue()
        {
            var data = new BuildingData();
            data.ID = 1000000;
            data.TID = "SOME_TID";
            _collection.Add(data);

            // Data is in collection.
            Assert.True(data._isInCollection);
            Assert.True(_collection.Remove(0));
            // Data is no more in collection.
            Assert.False(data._isInCollection);
            Assert.AreEqual(0, _collection.Count);
        }
        #endregion

        #region Contains(TCsvData)
        [Test]
        public void Contains_IsReadOnly_OperatedNormally()
        {
            var data = new BuildingData();
            data.ID = 1000000;
            data.TID = "SOME_TID";
            _collection.Add(data);

            // IsReadOnly should not affect Contains method.
            _collection.IsReadOnly = true;
            Assert.DoesNotThrow(() => _collection.Contains(data));
        }

        [Test]
        public void Contains_Null_Exception()
        {
            Assert.Throws<ArgumentNullException>(() => _collection.Contains(null));
        }

        [Test]
        public void Contains_DifferingID_Exception()
        {
            var data1 = new BuildingData();
            data1.ID = 1000000;
            data1.TID = "SOME_TID";
            _collection.Add(data1);

            var data2 = new BuildingData();
            data2.ID = 1000001; // Not same.
            data2.TID = "SOME_TID";

            // Would return false anyways as data1's ref is not equal to data2's ref.
            Assert.Throws<ArgumentException>(() => _collection.Contains(data2));
        }

        [Test]
        public void Contains_DifferingTID_Exception()
        {
            var data1 = new BuildingData();
            data1.ID = 1000000;
            data1.TID = "SOME_TID";
            _collection.Add(data1);

            var data2 = new BuildingData();
            data2.ID = 1000001;
            data2.TID = "DIFFERING_TID";

            Assert.Throws<ArgumentException>(() => _collection.Contains(data2));
        }

        [Test]
        public void Contains_ValidItem_ReturnsTrue()
        {
            var data = new BuildingData();
            data.ID = 1000000;
            data.TID = "SOME_TID";
            _collection.Add(data);

            Assert.True(_collection.Contains(data));
        }

        [Test]
        public void Contains_DifferingRef_ReturnsFalse()
        {
            var data1 = new BuildingData();
            data1.ID = 1000000;
            data1.TID = "SOME_TID";
            _collection.Add(data1);

            var data2 = new BuildingData();
            data2.ID = 1000000;
            data2.TID = "SOME_TID";

            // Reference to data1 and data2 are the same, so it should return false.
            Assert.False(_collection.Contains(data2));
        }
        #endregion

        #region Contains(int)
        [Test]
        public void ContainsInt_ItemNotFound_ReturnsFalse()
        {
            Assert.False(_collection.Contains(0));
            Assert.False(_collection.Contains(999));
        }

        [Test]
        public void ContainsInt_NegativeLevel_Exception()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => _collection.Contains(-1));
            Assert.Throws<ArgumentOutOfRangeException>(() => _collection.Contains(-999));
        }

        [Test]
        public void Contains_ItemFound_ReturnsTrue()
        {
            var data = new BuildingData();
            data.ID = 1000000;
            data.TID = "SOME_TID";
            data.Level = 1;

            _collection.Add(data);

            Assert.True(_collection.Contains(1));
        }
        #endregion

        #region Clear
        [Test]
        public void Clear_IsReadOnly_Exception()
        {
            _collection.IsReadOnly = true;
            Assert.Throws<InvalidOperationException>(() => _collection.Clear());
        }

        [Test]
        public void Clear_ContainsItems_Cleared()
        {
            // Add 5 items to the collection.
            for (int i = 0; i < 5; i++)
            {
                var data = new BuildingData();
                data.ID = 1000000;
                data.TID = "SOME_TID";
                data.Level = i;
            }

            _collection.Clear();

            Assert.AreEqual(0, _collection.Count);
        }
        #endregion

        #region ToArray
        [Test]
        public void ToArray_Empty_Returns0LengthArray()
        {
            var array = _collection.ToArray();
            Assert.AreEqual(0, array.Length);
        }

        [Test]
        public void ToArray_Contains5Items_Returns5LengthArray()
        {
            // += 2 to try to mess with _array.
            for (int i = 0; i < 10; i += 2)
            {
                var data = new BuildingData();
                data.ID = 1000000; // Same.
                data.TID = "SOME_TID";
                data.Level = i / 2;
                _collection.Add(data);
            }

            Assert.AreEqual(5, _collection.Count);

            var array = _collection.ToArray();
            Assert.AreEqual(5, array.Length);
            for (int i = 0; i < array.Length; i++)
            {
                Assert.AreEqual(i, array[i].Level);
            }
        }
        #endregion

        #region CopyTo
        [Test]
        public void CopyTo_Null_Exception()
        {
            Assert.Throws<ArgumentNullException>(() => _collection.CopyTo(null, 0));
        }

        [Test]
        public void CopyTo_ArrayTooSmall_Exception()
        {
            for (int i = 0; i < 5; i++)
            {
                var data = new BuildingData();
                data.ID = 1000000; // Same.
                data.TID = "SOME_TID";
                data.Level = i;
                _collection.Add(data);
            }

            var array1 = new BuildingData[4];
            Assert.Throws<ArgumentException>(() => _collection.CopyTo(array1, 0));

            var array2 = new BuildingData[5];
            Assert.Throws<ArgumentException>(() => _collection.CopyTo(array1, 1));
        }

        [Test]
        public void CopyTo_ValidArray_Copied()
        {
            for (int i = 0; i < 5; i++)
            {
                var data = new BuildingData();
                data.ID = 1000000; // Same.
                data.TID = "SOME_TID";
                data.Level = i;
                _collection.Add(data);
            }

            var array = new BuildingData[5];
            _collection.CopyTo(array, 0);

            for (int i = 0; i < array.Length; i++)
            {
                Assert.AreEqual(i, array[i].Level);
            }
        }

        [Test]
        public void CopyTo_ValidArrayOffset_Copied()
        {
            for (int i = 0; i < 5; i++)
            {
                var data = new BuildingData();
                data.ID = 1000000; // Same.
                data.TID = "SOME_TID";
                data.Level = i;
                _collection.Add(data);
            }

            var array = new BuildingData[6];
            _collection.CopyTo(array, 1);

            for (int i = 1; i < 6; i++)
            {
                Assert.AreEqual(i - 1, array[i].Level);
            }
        }
        #endregion

        #region GetEnumerator
        [Test]
        public void GetEnumerator_Contains5Items_Iterates5Times()
        {
            for (int i = 0; i < 5; i++)
            {
                var data = new BuildingData();
                data.ID = 1000000; // Same.
                data.TID = "SOME_TID";
                data.Level = i;
                _collection.Add(data);
            }

            var index = 0;
            foreach (var data in _collection)
            {
                Assert.AreEqual(index, data.Level);
                index++;
            }

            Assert.AreEqual(5, index);
        }

        [Test]
        public void GetEnumerator_Linq_()
        {
            for (int i = 0; i < 5; i++)
            {
                var data = new BuildingData();
                data.ID = 1000000; // Same.
                data.TID = "SOME_TID";
                data.Level = i;
                data.Name = "Test Building " + i.ToString();
                _collection.Add(data);
            }

            var count1 = _collection.Count(b => b.Level > 1);
            Assert.AreEqual(3, count1);

            var count2 = _collection.Count(b => b.Name.Contains("Test Building"));
            Assert.AreEqual(5, count2);
        }
        #endregion
    }
}
