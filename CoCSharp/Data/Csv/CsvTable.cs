using System.Data;
using System.IO;
using System.Text.RegularExpressions;

namespace CoCSharp.Data.Csv
{
    /// <summary>
    /// Represents a Comma Seperated File(CSV) datatable.
    /// </summary>
    public class CsvTable
    {
        /// <summary>
        /// Reads the specified  .csv file and parses it.
        /// </summary>
        /// <param name="path">Path to .csv file.</param>
        public CsvTable(string path)
        {
            //TODO: Seperate TypesRow from table.

            Table = new DataTable();
            var rows = Regex.Split(File.ReadAllText(path).Replace("\"", string.Empty), "\r\n");
            var columns = Regex.Split(rows[0], ",");

            for (int i = 0; i < columns.Length; i++)
                Table.Columns.Add(columns[i]);

            for (int i = 1; i < rows.Length; i++)
                Table.Rows.Add(Regex.Split(rows[i], ","));
        }

        /// <summary>
        /// Gets the DataTable of the CSV file.
        /// </summary>
        public DataTable Table { get; private set; }
        /// <summary>
        /// Gets the rows of the CSV file.
        /// </summary>
        public DataRowCollection Rows { get { return Table.Rows; } }
        /// <summary>
        /// Gets the columns of the CSV file.
        /// </summary>
        public DataColumnCollection Columns { get { return Table.Columns; } }
        /// <summary>
        /// Gets the row of the CSV file which defines the value types of the columns.
        /// </summary>
        public DataRow TypesRow { get { return Table.Rows[0]; } }
        public bool EndOfFile { get { return Reader.EndOfStream; } }

        private StreamReader Reader { get; set; }

        public CsvRow ReadNextRow()
        {
            var row = Reader.ReadLine().Split(',');
            return new CsvRow(row);
        }
    }
}
