using CoCSharp.Data.Csv;
using NUnit.Framework;
using System.IO;
using System.Text;

namespace CoCSharp.Test.Csv.Data
{
    [TestFixture]
    public class CsvTableTests
    {
        [Test]
        public void TestUncompressedCsvTable()
        {
            var table = new CsvTable("Resources/characters.csv");
            //WriteTableToTxt(table, "characters_parsed.txt");
            //table.Save("saved_characters.csv");
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
