using CoCSharp.Data;
using CoCSharp.Data.Csv;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace CoCSharp.Tests.Data.Csv
{
    [TestClass]
    public class CsvTableTests
    {
        [TestMethod]
        public void TestUncompressedCsvTable()
        {
            var table = new CsvTable("characters.csv");
            PrintTable(table);
            table.Save("saved_characters.csv");
        }

        [TestMethod]
        public void TestCompressedCsvTable()
        {
            var table = new CsvTable("com_globals.csv", true);
            PrintTable(table);
            table.Save("saved_com_globals.csv", true);
        }

        [TestMethod]
        public void TestCsvSerializer()
        {
            var table = new CsvTable("buildings.csv");
            var data = CsvSerializer.Deserialize(table, typeof(BuildingData));
            for (int i = 0; i < data.Length; i++)
            {
                var type = data[i].GetType();
                var properties = type.GetProperties();
                for (int x = 0; x < properties.Length; x++)
                {
                    Console.Write(properties[x].Name.PadRight(100));
                    Console.Write(properties[x].GetMethod.Invoke(data[i], null));
                    Console.WriteLine();
                }
                Console.WriteLine();
            }
        }

        private void PrintTable(CsvTable table)
        {
            for (int i = 0; i < table.Columns.Count; i++)
                Console.Write(table.Columns[i].ColumnName.PadRight(30));
            Console.WriteLine();

            for (int i = 0; i < table.Rows.Count; i++)
            {
                for (int k = 0; k < table.Rows[i].ItemArray.Length; k++)
                    Console.Write(table.Rows[i].ItemArray[k].ToString().PadRight(30));
                Console.WriteLine();
            }
        }
    }
}
