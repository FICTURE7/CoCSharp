using System;
using Xunit;

namespace CoCSharp.Csv.Tests
{
    public class CsvDataRowTests
    {
        [Fact]
        public void Add_NullValue_Throws()
        {
            var row = new CsvDataRow<Test1Data>();
            Assert.Throws<ArgumentNullException>("dataCollection", () => row.Add(null));     
        }

        [Fact]
        public void Add_dataCollection_CollectionRef_Changes()
        {
            var row = new CsvDataRow<Test1Data>();
            var datacol = new CsvDataCollection<Test1Data>("kek");

            Assert.Same(CsvDataCollectionRef<Test1Data>.NullRef, datacol.CollectionRef);

            row.Add(datacol);

            Assert.NotSame(CsvDataCollectionRef<Test1Data>.NullRef, datacol.CollectionRef);
        }
    }
}
