using System;
using Xunit;

namespace CoCSharp.Csv.Tests
{
    public class CsvDataTests
    {
        [Fact]
        public void Constructors_CollectionRef_IsNull()
        {
            var data = new TestData();

            Assert.Null(data.CollectionRef);
        }

        [Fact]
        public void When_CollectionRef_IsNull_ID_Throws()
        {
            var data = new TestData();

            Assert.Throws<InvalidOperationException>(() => data.ID);
        }

        [Fact]
        public void When_CollectionRef_IsNull_Level_Throws()
        {
            var data = new TestData();

            Assert.Throws<InvalidOperationException>(() => data.Level);
        }

        private class TestData : CsvData
        {
            internal override int KindID
            {
                get
                {
                    return 69;
                }
            }
        }
    }
}
