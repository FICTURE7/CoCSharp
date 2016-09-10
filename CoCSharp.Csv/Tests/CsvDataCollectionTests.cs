using System;
using System.Collections.Generic;
using Xunit;

namespace CoCSharp.Csv.Tests
{
    public class CsvDataCollectionTests
    {
        [Fact]
        public void CsvDataCollection_Is_ICollectionCsvData_And_ICollectionTCsvData()
        {
            CsvDataCollection data = new CsvDataCollection<Mock1Data>("name");
            Assert.IsAssignableFrom<ICollection<CsvData>>(data);
            Assert.IsAssignableFrom<ICollection<Mock1Data>>(data); // TCsvData
        }

        [Fact]
        public void ID_Throws_When_Not_In_CsvDataRow()
        {
            var datacol = new CsvDataCollection<Mock1Data>("kek");
            Assert.Throws<InvalidOperationException>(() => datacol.ID);
        }

        [Fact]
        public void Add_Value_Not_TypeOf_CsvData_Throws()
        {
            CsvDataCollection datacol = new CsvDataCollection<Mock1Data>("name");
            var data = new Mock2Data();

            Assert.Throws<InvalidCastException>(() => datacol.Add(data));
        }

        [Fact]
        public void Add_NullValue_Throws()
        {
            var datacol = new CsvDataCollection<Mock1Data>("kek");
            Assert.Throws<ArgumentNullException>("data", () => datacol.Add(null));
        }

        [Fact]
        public void Add_Data_Already_Which_Has_Already_Been_Added_Throws()
        {
            var datacol = new CsvDataCollection<Mock1Data>("kek2");
            var data = new Mock1Data();
            datacol.Add(data);

            Assert.Equal(0, data.Level);

            Assert.Throws<ArgumentException>("data", () => datacol.Add(data));
        }

        [Fact]
        public void Add_ValidData_DataLevel_IsSet()
        {
            var datacol = new CsvDataCollection<Mock1Data>("kek2");
            for (int i = 0; i < 50; i++)
            {
                var data = new Mock1Data();
                datacol.Add(data);
            }

            Assert.Equal(50, datacol.Count);
            for (int i = 0; i < 50; i++)
            {
                var data = datacol[i];
                Assert.Equal(i, data.Level);
                Assert.Same(CsvDataCollectionRef<Mock1Data>.NullRef, data.CollectionRef);
            }
        }

        [Fact]
        public void Indexer_Getter_ReturnsNull_If_Item_DoesNot_Exists()
        {
            var datacol = new CsvDataCollection<Mock1Data>("kek5");
            Assert.Null(datacol[-1]);
            Assert.Null(datacol[int.MinValue]);

            Assert.Null(datacol[0]);
            Assert.Null(datacol[int.MaxValue]);
        }

        [Fact]
        public void If_CsvDataCollection_IsIn_CsvDataRow_Ref_Changes()
        {
            var datarow = new CsvDataRow<Mock1Data>();
            var datacol = new CsvDataCollection<Mock1Data>("kek");
            Assert.Same(CsvDataCollectionRef<Mock1Data>.NullRef, datacol.CollectionRef);

            datarow.Add(datacol);

            Assert.NotSame(CsvDataCollectionRef<Mock1Data>.NullRef, datacol.CollectionRef);
            Assert.Equal(69, datacol.CollectionRef.RowIndex);
            Assert.Equal(0, datacol.CollectionRef.ColumnIndex);
            Assert.Equal(69000000, datacol.CollectionRef.ID);
        }

        [Fact]
        public void If_CsvDataCollection_IsIn_CsvDataRow_NewlyAdded_Data_Ref_Changes()
        {
            var datarow = new CsvDataRow<Mock1Data>();
            var datacol = new CsvDataCollection<Mock1Data>("kek");
            var data = new Mock1Data();
            datarow.Add(datacol);

            datacol.Add(data);
            Assert.Same(datacol.CollectionRef, data.CollectionRef);
        }

        [Fact]
        public void If_CsvDataCollection_IsIn_CsvDataRow_OldData_Ref_Changes()
        {
            var dataRow = new CsvDataRow<Mock1Data>();
            var dataCollection = new CsvDataCollection<Mock1Data>("kek");
            var data = new Mock1Data();

            // dataCollection not yet added to dataRow, therefore dataCollection should have a NullRef value.
            Assert.Same(CsvDataCollectionRef<Mock1Data>.NullRef, dataCollection.CollectionRef);
            dataCollection.Add(data);

            // data added to dataCollection, therefore data should have a NullRef value, 
            // just like dataCollection.
            Assert.Same(dataCollection.CollectionRef, data.CollectionRef);

            // dataCollection added to dataRow, therefore dataCollection should have a new CollectionRef value assigned
            // by dataRow. As well as the old data added to dataCollection's CollectionRef must be updated.
            dataRow.Add(dataCollection);
            Assert.Same(dataCollection.CollectionRef, data.CollectionRef);
        }
    }
}
