using CoCSharp.Csv;
using CoCSharp.Data.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace CoCSharp.Test.Csv
{
    [TestFixture]
    public class CsvDataTableTests
    {
        private CsvDataTable<BuildingData> _table;

        [SetUp]
        public void SetUp()
        {
            _table = new CsvDataTable<BuildingData>();
        }

        [Test]
        public void KindID_Is_Same_As_CsvData_GenericArgs_KindID()
        {
            Assert.AreEqual(1, _table.KindID);
        }

        [Test]
        public void IndexerGetter_Using_Column_Index()
        {
            var data = new CsvDataRow<BuildingData>("Test Collection");
            _table.Add(data);

            var retData = _table[0];

            Assert.AreEqual(1, _table.Count);
            Assert.AreEqual(0, data._ref.ColumnIndex);
            Assert.AreSame(data, retData);
        }

        [Test]
        public void IndexerGetter_Using_Column_Name()
        {
            var data = new CsvDataRow<BuildingData>("Test Collection");
            _table.Add(data);

            var retData = _table["Test Collection"];

            Assert.AreEqual(1, _table.Count);
            Assert.AreEqual(0, data._ref.ColumnIndex);
            Assert.AreSame(data, retData);
        }

        [Test]
        public void IndexerGetter_Using_Column_Names_MultipleItems_In_Collection()
        {
            var dataList = new List<CsvDataRow<BuildingData>>();

            for (int i = 0; i < 50; i++)
            {
                var data = new CsvDataRow<BuildingData>("Test Collection " + i);
                dataList.Add(data);
                _table.Add(data);
            }

            Assert.AreEqual(50, _table.Count);

            for (int i = 0; i < 50; i++)
            {
                var data = dataList[i];
                var retData = _table["Test Collection " + i];

                Assert.AreEqual(data._ref.ColumnIndex, i);
                Assert.AreSame(data, retData);
            }
        }

        [Test]
        public void Add_Item_Null_Exception()
        {
            Assert.Throws<ArgumentNullException>(() => _table.Add(null));
        }

        [Test]
        public void Add_Item_With_Same_Name_Exists_Exception()
        {
            _table.Add(new CsvDataRow<BuildingData>("Already Exists"));

            Assert.AreEqual(1, _table.Count);
            Assert.Throws<ArgumentException>(() => _table.Add(new CsvDataRow<BuildingData>("Already Exists")));
        }

        [Test]
        public void Add_Item_Valid_CountIncreases_columnIndexChanges()
        {
            var data = new CsvDataRow<BuildingData>("Tests");
            _table.Add(data);

            Assert.AreEqual(0, data._ref.ColumnIndex);
            Assert.AreEqual(1, _table.Count);
        }
    }
}
