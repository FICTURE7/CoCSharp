using CoCSharp.Csv;
using CoCSharp.Data.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace CoCSharp.Test.Csv
{
    [TestFixture]
    public class CsvDataRowTests
    {
        private CsvDataRow<BuildingData> _row;

        [SetUp]
        public void SetUp()
        {
            _row = new CsvDataRow<BuildingData>();
        }

        [Test]
        public void KindID_Is_Same_As_CsvData_GenericArgs_KindID()
        {
            Assert.AreEqual(1, _row.KindID);
        }

        [Test]
        public void IndexerGetter_Using_Column_Index()
        {
            var data = new CsvDataCollection<BuildingData>("Test Collection");
            _row.Add(data);

            var retData = _row[0];

            Assert.AreEqual(1, _row.Count);
            Assert.AreEqual(0, data._columnIndex);
            Assert.AreSame(data, retData);
        }

        [Test]
        public void IndexerGetter_Using_Column_Name()
        {
            var data = new CsvDataCollection<BuildingData>("Test Collection");
            _row.Add(data);

            var retData = _row["Test Collection"];

            Assert.AreEqual(1, _row.Count);
            Assert.AreEqual(0, data._columnIndex);
            Assert.AreSame(data, retData);
        }

        [Test]
        public void IndexerGetter_Using_Column_Names_MultipleItems_In_Collection()
        {
            var dataList = new List<CsvDataCollection<BuildingData>>();

            for (int i = 0; i < 50; i++)
            {
                var data = new CsvDataCollection<BuildingData>("Test Collection " + i);
                dataList.Add(data);
                _row.Add(data);
            }

            Assert.AreEqual(50, _row.Count);

            for (int i = 0; i < 50; i++)
            {
                var data = dataList[i];
                var retData = _row["Test Collection " + i];

                Assert.AreEqual(data._columnIndex, i);
                Assert.AreSame(data, retData);
            }
        }

        [Test]
        public void Add_Item_Null_Exception()
        {
            Assert.Throws<ArgumentNullException>(() => _row.Add(null));
        }

        [Test]
        public void Add_Item_With_Same_Name_Exists_Exception()
        {
            _row.Add(new CsvDataCollection<BuildingData>("Already Exists"));

            Assert.AreEqual(1, _row.Count);
            Assert.Throws<ArgumentException>(() => _row.Add(new CsvDataCollection<BuildingData>("Already Exists")));
        }

        [Test]
        public void Add_Item_Valid_CountIncreases_columnIndexChanges()
        {
            var data = new CsvDataCollection<BuildingData>("Tests");
            _row.Add(data);

            Assert.AreEqual(0, data._columnIndex);
            Assert.AreEqual(1, _row.Count);
        }
    }
}
