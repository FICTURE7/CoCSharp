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
            var deBytes = CsvTable.DecompressCsvTable(originalbytes);

            // Compresses the decompressed bytes.
            var coBytes = CsvTable.CompressCsvTable(deBytes);

            // Check if compressed/produced bytes are equal to original compressed bytes.
            Assert.AreEqual(originalbytes, coBytes);

            // Decompress the compressed bytes.
            var newDeBytes = CsvTable.DecompressCsvTable(coBytes);

            // Check if decompressed/produced bytes are equal to original decompressed bytes.
            Assert.AreEqual(deBytes, newDeBytes);
        }
    }
}
