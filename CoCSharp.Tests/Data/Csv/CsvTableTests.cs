using CoCSharp.Data;
using CoCSharp.Data.Csv;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Text;

namespace CoCSharp.Tests.Data.Csv
{
    [TestClass]
    public class CsvTableTests
    {
        [TestMethod]
        public void TestUncompressedCsvTable()
        {
            var table = new CsvTable("characters.csv");
            WriteTableToTxt(table, "characters_parsed.txt");
            table.Save("saved_characters.csv");
        }

        [TestMethod]
        public void TestCompressedCsvTable()
        {
            var table = new CsvTable("com_globals.csv", true);
            WriteTableToTxt(table, "com_globals_parsed.txt");
            table.Save("saved_com_globals.csv", true);
        }

        [TestMethod]
        public void TestCsvSerializerBuildings()
        {
            var table = new CsvTable("buildings.csv");
            var data = CsvSerializer.Deserialize(table, typeof(BuildingData));
            var strBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                strBuilder.AppendLine((i + 1) + ". ---------------------");
                var type = data[i].GetType();
                var properties = type.GetProperties();
                for (int x = 0; x < properties.Length; x++)
                {
                    strBuilder.Append(properties[x].Name.PadRight(100));
                    strBuilder.Append(properties[x].GetMethod.Invoke(data[i], null));
                    strBuilder.AppendLine();
                }
            }
            File.WriteAllText("buildings_parsed.txt", strBuilder.ToString());
        }

        [TestMethod]
        public void TestCsvSerializerCharacters()
        {
            var table = new CsvTable("characters.csv");
            var data = CsvSerializer.Deserialize(table, typeof(CharacterData));
            var strBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                strBuilder.AppendLine((i + 1) + ". ---------------------");
                var type = data[i].GetType();
                var properties = type.GetProperties();
                for (int x = 0; x < properties.Length; x++)
                {
                    strBuilder.Append(properties[x].Name.PadRight(100));
                    strBuilder.Append(properties[x].GetMethod.Invoke(data[i], null));
                    strBuilder.AppendLine();
                }
            }
            File.WriteAllText("characters_parsed.txt", strBuilder.ToString());
        }

        private void WriteTableToTxt(CsvTable table, string path)
        {
            var strBuilder = new StringBuilder();
            for (int i = 0; i < table.Columns.Count; i++)
                strBuilder.Append(table.Columns[i].ColumnName.PadRight(100));
            strBuilder.AppendLine();

            for (int i = 0; i < table.Rows.Count; i++)
            {
                for (int k = 0; k < table.Rows[i].ItemArray.Length; k++)
                    strBuilder.Append(table.Rows[i].ItemArray[k].ToString().PadRight(100));
                strBuilder.AppendLine();
            }
            File.WriteAllText(path, strBuilder.ToString());
        }
    }
}
