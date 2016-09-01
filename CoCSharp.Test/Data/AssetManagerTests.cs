using CoCSharp.Csv;
using CoCSharp.Data;
using CoCSharp.Data.Models;
using NUnit.Framework;
using System;
using System.IO;

namespace CoCSharp.Test.Data
{
    [TestFixture]
    public class AssetManagerTests
    {
        private AssetManager _manager;

        [SetUp]
        public void SetUp()
        {
            _manager = new AssetManager(TestUtils.ContentDirectory);
        }

        [Test]
        public void Constructors_InvalidArgs_Exception()
        {
            Assert.Throws<ArgumentNullException>(() => new AssetManager(null));
            Assert.Throws<DirectoryNotFoundException>(() => new AssetManager("some dir that does not exists"));
        }

        [Test]
        public void Load_InvalidArgs_Exception()
        {
            Assert.Throws<ArgumentNullException>(() => _manager.Load<object>(null));
            // No AssetLoader for the type Object.
            Assert.Throws<InvalidOperationException>(() => _manager.Load<object>("somefoles"));
            Assert.Throws<FileNotFoundException>(() => _manager.Load<CsvDataRow<BuildingData>>("kek"));
        }

        [Test]
        public void Load_CsvDataTableBuildingData_TableGetsLoaded()
        {
            _manager.Load<CsvDataRow<BuildingData>>("csv/buildings.csv");
        }

        [Test]
        public void IsLoaded_CsvDataTableBuildingData_ReturnsTrue()
        {
            _manager.Load<CsvDataRow<BuildingData>>("csv/buildings.csv");
            var value = _manager.IsLoaded<CsvDataRow<BuildingData>>();

            Assert.True(value);
        }

        [Test]
        public void Get_CsvDataTableBuildingData_ReturnsRefToTable()
        {
            _manager.Load<CsvDataRow<BuildingData>>("csv/buildings.csv");
            var table = _manager.Get<CsvDataRow<BuildingData>>();

            Assert.NotNull(table);
            Assert.IsInstanceOf<CsvDataRow<BuildingData>>(table);
        }

        [Test]
        public void Get_CsvDataTable_ReturnsRefToTable()
        {
            _manager.Load<CsvDataRow<BuildingData>>("csv/buildings.csv");
            var table = _manager.Get<CsvDataTable>();

            Assert.NotNull(table);
            Assert.IsInstanceOf<CsvDataTable>(table);
        }
    }
}
