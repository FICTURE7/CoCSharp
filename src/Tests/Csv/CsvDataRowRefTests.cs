using CoCSharp.Csv;
using NUnit.Framework;
using System;

namespace CoCSharp.Test.Csv
{
    [TestFixture]
    public class CsvDataRowRefTests
    {
        [Test]
        public void Constructors_InvalidArgs_Exception()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new CsvDataRowRef(-1));
            Assert.Throws<ArgumentOutOfRangeException>(() => new CsvDataRowRef(10003));

            Assert.Throws<ArgumentOutOfRangeException>(() => new CsvDataRowRef(-1, 2));
            Assert.Throws<ArgumentOutOfRangeException>(() => new CsvDataRowRef(1, -2));
            Assert.Throws<ArgumentOutOfRangeException>(() => new CsvDataRowRef(1, 10000000));
        }
    }
}
