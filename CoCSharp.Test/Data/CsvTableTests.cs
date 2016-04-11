using CoCSharp.Csv;
using NUnit.Framework;
using System;
using System.IO;

namespace CoCSharp.Test.Data
{
    [TestFixture]
    public class CsvTableTests
    {
        [Test]
        public void TestCsvTableCompression()
        {
            var originalbytes = File.ReadAllBytes(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Content/csv/com_buildings.csv"));

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
