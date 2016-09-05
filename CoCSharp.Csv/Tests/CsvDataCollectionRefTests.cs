using System;
using Xunit;

namespace CoCSharp.Csv.Tests
{
    public class CsvDataCollectionRefTests
    {
        [Fact]
        public void Constructors_id_LessThan1000000_Throws()
        {
            Assert.Throws<ArgumentOutOfRangeException>("id", () => new CsvDataCollectionRef(-1));
            Assert.Throws<ArgumentOutOfRangeException>("id", () => new CsvDataCollectionRef(int.MinValue));
            Assert.Throws<ArgumentOutOfRangeException>("id", () => new CsvDataCollectionRef(32));
            Assert.Throws<ArgumentOutOfRangeException>("id", () => new CsvDataCollectionRef(999999));
        }

        [Fact]
        public void Constructors_rowIndex_LessThan1_Throws()
        {
            Assert.Throws<ArgumentOutOfRangeException>("rowIndex", () => new CsvDataCollectionRef(-1, 0));
            Assert.Throws<ArgumentOutOfRangeException>("rowIndex", () => new CsvDataCollectionRef(0, 0));
        }

        [Fact]
        public void Constructors_columnIndex_LessThan0_Throws()
        {
            Assert.Throws<ArgumentOutOfRangeException>("columnIndex", () => new CsvDataCollectionRef(1, -1));
        }

        [Theory]
        [InlineData(1000003, 1, 3)]
        [InlineData(3000013, 3, 13)]
        [InlineData(14000345, 14, 345)]
        [InlineData(89000008, 89, 8)]
        [InlineData(9999999, 9, 999999)]
        [InlineData(3000080, 3, 13)]
        public void Constructor_id_ColumnIndex_And_RowIndex_Calculated(int id, int expectedRowIndex, int expectedColumnIndex)
        {
            var dref = new CsvDataCollectionRef(id);
            Assert.Equal(id, dref.ID);
            Assert.Equal(expectedRowIndex, dref.RowIndex);
            Assert.Equal(expectedColumnIndex, dref.ColumnIndex);
        }

        [Theory]
        [InlineData(1, 3, 1000003)]
        [InlineData(33, 33, 33000033)]
        [InlineData(4, 2, 4000002)]
        [InlineData(7, 9, 7000009)]
        [InlineData(6, 555, 6000555)]
        [InlineData(9, 999999, 9999999)]
        public void Constructor_rowIndex_columnIndex_ID_Calculated(int rowIndex, int columnIndex, int expectedId)
        {
            var dref = new CsvDataCollectionRef(rowIndex, columnIndex);
            Assert.Equal(rowIndex, dref.RowIndex);
            Assert.Equal(columnIndex, dref.ColumnIndex);
            Assert.Equal(expectedId, dref.ID);
        }
    }
}
