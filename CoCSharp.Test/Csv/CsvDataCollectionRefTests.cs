using CoCSharp.Csv;
using NUnit.Framework;
using System;

using static NUnit.Framework.Assert;

namespace CoCSharp.Test.Csv
{
    [TestFixture]
    public class CsvDataCollectionRefTests
    {
        [Test]
        public void Constructors_InvalidArgs_Exception()
        {
            Throws<ArgumentOutOfRangeException>(() => new CsvDataCollectionRef(-1));
            Throws<ArgumentOutOfRangeException>(() => new CsvDataCollectionRef(10003));

            Throws<ArgumentOutOfRangeException>(() => new CsvDataCollectionRef(-1, 2));
            Throws<ArgumentOutOfRangeException>(() => new CsvDataCollectionRef(1, -2));
            Throws<ArgumentOutOfRangeException>(() => new CsvDataCollectionRef(1, 10000000));
        }
    }
}
