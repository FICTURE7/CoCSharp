using System;
using System.Data;
using System.IO;
using System.Text.RegularExpressions;

namespace CoCSharp.Data.Csv
{
    /// <summary>
    /// Represents a Comma Seperated Values(CSV) file as datatable. 
    /// Mainly made for Clash of Clans.
    /// </summary>
    public class CsvTable
    {
        //TODO: Implement save to CSV methods.

        /// <summary>
        /// Reads the specified .csv file and parses it.
        /// </summary>
        /// <param name="path">Path to .csv file.</param>
        public CsvTable(string path)
        {
            Table = new DataTable();
            var rows = Regex.Split(File.ReadAllText(path).Replace("\"", string.Empty), "\r\n");
            var columnNames = Regex.Split(rows[0], ",");
            var typeRow = Regex.Split(rows[1], ",");
            for (int i = 0; i < columnNames.Length; i++)
            {
                var type = (Type)null; // associate a data type with the columns
                switch (typeRow[i])
                {
                    case "String":
                        type = typeof(string);
                        break;
                    case "int":
                        type = typeof(int);
                        break;
                    case "Boolean":
                    case "boolean":
                        type = typeof(bool);
                        break;
                    default:
                        throw new InvalidDataException(string.Format("Unhandled data type '{0}'.", typeRow[i]));
                }
                Table.Columns.Add(columnNames[i], type);
            }

            for (int i = 2; i < rows.Length; i++) // turn empty("") fields to DBNull.Value loop
            {
                var columnValues = (object[])Regex.Split(rows[i], ","); 
                var newColumnValues = new object[columnValues.Length];
                for (int x = 0; x < columnValues.Length; x++)
                {
                    newColumnValues[x] = columnValues[x] == string.Empty ?
                                                         DBNull.Value :
                                                         columnValues[x];
                }
                Table.Rows.Add(newColumnValues);
            }
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
