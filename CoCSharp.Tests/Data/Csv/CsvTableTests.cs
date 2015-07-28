using CoCSharp.Data;
using CoCSharp.Data.Csv;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace CoCSharp.Tests.Data.Csv
{
    [TestClass]
    public class CsvTableTests
    {
        [TestMethod]
        public void TestCsvTable()
        {
            var table = new CsvTable("characters.csv");
            for (int i = 0; i < table.Columns.Count; i++)
                Console.Write(table.Columns[i].ColumnName.PadRight(30));
            Console.WriteLine();

            for (int i = 0; i < table.Rows.Count; i++)
            {
                for (int k = 0; k < table.Rows[i].ItemArray.Length; k++)
                    Console.Write(table.Rows[i].ItemArray[k].ToString().PadRight(29));
                Console.WriteLine();
            }
        }

        [TestMethod]
        public void TestCsvSerializer()
        {
            var table = new CsvTable("buildings.csv");
            var data = CsvSerializer.Deserialize(table, typeof(BuildingData));
        }
    }
}
