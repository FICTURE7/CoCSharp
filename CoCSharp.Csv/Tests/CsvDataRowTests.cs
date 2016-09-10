using System;
using Xunit;

namespace CoCSharp.Csv.Tests
{
    public class CsvDataRowTests
    {
        [Fact]
        public void Add_NullValue_Throws()
        {
            var row = new CsvDataRow<Mock1Data>();
            Assert.Throws<ArgumentNullException>("dataCollection", () => row.Add(null));
        }

        [Fact]
        public void Add_dataCollection_CollectionRef_Changes()
        {
            var row = new CsvDataRow<Mock1Data>();
            var datacol = new CsvDataCollection<Mock1Data>("kek");

            Assert.Same(CsvDataCollectionRef<Mock1Data>.NullRef, datacol.CollectionRef);

            row.Add(datacol);

            Assert.NotSame(CsvDataCollectionRef<Mock1Data>.NullRef, datacol.CollectionRef);
        }
    }
}
