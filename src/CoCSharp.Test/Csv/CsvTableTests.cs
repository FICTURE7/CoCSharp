using CoCSharp.Csv;
using NUnit.Framework;
using System;
using System.IO;

namespace CoCSharp.Test.Csv
{
    [TestFixture]
    public class CsvTableTests
    {
        // Tests if compression are equal to original ones.
        [Test]
        public void Compression_Equality()
        {
            var tablePath = Path.Combine(TestUtils.CsvDirectory, "com_buildings.csv");
            var originalbytes = File.ReadAllBytes(tablePath);

            // Decompress the original bytes.
            var deBytes = CsvTable.Decompress(originalbytes);

            // Compresses the decompressed bytes.
            var coBytes = CsvTable.Compress(deBytes);

            // Check if compressed/produced bytes are equal to original compressed bytes.
            Assert.AreEqual(originalbytes, coBytes);

            // Decompress the compressed bytes.
            var newDeBytes = CsvTable.Decompress(coBytes);

            // Check if decompressed/produced bytes are equal to original decompressed bytes.
            Assert.AreEqual(deBytes, newDeBytes);
        }
    }
}
