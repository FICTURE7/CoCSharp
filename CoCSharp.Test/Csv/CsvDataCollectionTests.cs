using CoCSharp.Csv;
using CoCSharp.Data.Models;
using NUnit.Framework;
using System;
using System.IO;

namespace CoCSharp.Test.Csv
{
    [TestFixture]
    public class CsvDataCollectionTests
    {
        public CsvDataCollectionTests()
        {
            var tablePath = Path.Combine(TestUtils.CsvDirectory, "buildings.csv");
            _table = new CsvTable();
        }

        private CsvDataCollection<BuildingData> _data;
        private readonly CsvTable _table;

        [OneTimeSetUp]
        public void Constructors_()
        {
            _data = new CsvDataCollection<BuildingData>();
        }

        [Test]
        public void Add_NullArgs_Exception()
        {
            Assert.Throws<ArgumentNullException>(() => _data.Add(null));
        }

        [Test]
        public void Add_Item_AddedToDictionary()
        {
            var buildingData = new BuildingData();
            buildingData.Name = "Test_BuildingData";
            _data.Add(buildingData);

            Assert.AreEqual(1, _data[buildingData.ID].Length);
            Assert.AreSame(_data[buildingData.ID, buildingData.Level], buildingData);
        }
    }
}
