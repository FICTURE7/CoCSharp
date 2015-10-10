using SevenZip;
using System;
using System.Data;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace CoCSharp.Data.Csv
{
    /// <summary>
    /// Represents a Comma Seperated Values(CSV) file as a datatable. 
    /// Mainly designed for the Clash of Clans CSV file format.
    /// </summary>
    public sealed class CsvTable
    {
        /// <summary>
        /// Initalizes a new instance of the <see cref="CsvTable"/> class.
        /// </summary>
        public CsvTable()
        {
            Table = new DataTable();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CsvTable"/> class and 
        /// reads the specified .csv file and parses it.
        /// </summary>
        /// <param name="path">Path to the .csv file.</param>
        public CsvTable(string path)
        {
            Table = new DataTable();
            Load(path);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CsvTable"/> class and decompresses
        /// the specified .csv file if compressed, then reads file and parses it.
        /// </summary>
        /// <param name="path">Path to the .csv file.</param>
        /// <param name="compressed">Whether the .csv file is compressed or not.</param>
        public CsvTable(string path, bool compressed)
        {
            Table = new DataTable();
            Load(path, compressed);
        }

        /// <summary>
        /// Gets the <see cref="DataTable"/> of the CSV file.
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

        /// <summary>
        /// Reads the specified .csv file from disk without compression.
        /// </summary>
        /// <param name="path">Path to the .csv file.</param>
        public void Load(string path)
        {
            var bytes = File.ReadAllBytes(path);
            Load(bytes);
        }

        /// <summary>
        /// Reads the specified .csv file from disk and decompresses it if specifed.
        /// </summary>
        /// <param name="path">Path to the .csv file.</param>
        /// <param name="compressed">Whether the .csv file is compressed or not.</param>
        public void Load(string path, bool compressed)
        {
            var bytes = File.ReadAllBytes(path);
            Load(bytes, compressed);
        }

        /// <summary>
        /// Reads the specified .csv file from a <see cref="Byte"/> array without compression.
        /// </summary>
        /// <param name="bytes"><see cref="Byte"/> array of the .csv file.</param>
        public void Load(byte[] bytes)
        {
            var rawCsv = Encoding.UTF8.GetString(bytes);
            var rows = Regex.Split(rawCsv.Replace("\"", string.Empty), "\r\n");
            var columnNames = Regex.Split(rows[0], ",");
            var typeRow = Regex.Split(rows[1], ",");

            for (int i = 0; i < columnNames.Length; i++)  // associate a data type with the columns loop
            {
                var type = (Type)null;
                switch (typeRow[i])
                {
                    case "String":
                    case "string":
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
                        throw new CsvException(string.Format("Unexpected data type '{0}'.", typeRow[i]));
                }
                Table.Columns.Add(columnNames[i], type);
            }

            for (int i = 2; i < rows.Length; i++) // turn empty("") fields to DBNull.Value loop
            {
                var columnValues = (object[])Regex.Split(rows[i], ",");
                var newColumnValues = new object[columnValues.Length];
                for (int x = 0; x < columnValues.Length; x++)
                    newColumnValues[x] = (string)columnValues[x] == string.Empty ? DBNull.Value : columnValues[x];
                Table.Rows.Add(newColumnValues);
            }
        }

        /// <summary>
        /// Reads the specified .csv file from a <see cref="Byte"/> array and decompresses it if specifed.
        /// </summary>
        /// <param name="bytes"><see cref="Byte"/> array of the .csv file.</param>
        /// <param name="compressed">>Whether the .csv file is compressed or not.</param>
        public void Load(byte[] bytes, bool compressed)
        {
            if (compressed)
            {
                using (var mem = new MemoryStream())
                {
                    mem.Write(bytes, 0, 9); // fix the header
                    mem.Write(new byte[4], 0, 4);
                    mem.Write(bytes, 9, bytes.Length - 9);
                    Load(LzmaUtils.Decompress(mem.ToArray()));
                }
            }
            else Load(bytes);
        }

        /// <summary>
        /// Saves this <see cref="CsvTable"/> on disk with the specified path and without compression.
        /// </summary>
        /// <param name="path">Path of the .csv to save to.</param>
        public void Save(string path)
        {
            Save(path, false);
        }

        /// <summary>
        /// Saves this <see cref="CsvTable"/> on disk with the specified path and with compression
        /// if specified.
        /// </summary>
        /// <param name="path">Path of the .csv to save to.</param>
        /// <param name="compressed">Whether to compress the file or not.</param>
        public void Save(string path, bool compressed)
        {
            var csvBuilder = new StringBuilder();
            for (int i = 0; i < Table.Columns.Count - 1; i++) // write column names.
                csvBuilder.Append(Columns[i].ColumnName + ",");
            csvBuilder.AppendLine(Columns[Columns.Count - 1].ColumnName);

            for (int i = 0; i < Table.Columns.Count; i++) // write TypesRow
            {
                var format = i == Table.Columns.Count - 1 ? "{0}\r\n" : "{0},"; // check if last in array
                var dataType = Table.Columns[i].DataType;
                if (dataType == typeof(int))
                {
                    csvBuilder.AppendFormat(format, "int");
                    continue;
                }
                csvBuilder.AppendFormat(format, dataType.Name);
            }

            for (int i = 0; i < Table.Rows.Count; i++) // writes all rows
                csvBuilder.AppendLine(string.Join(",", Table.Rows[i].ItemArray));

            if (compressed) // checks compression
            {
                var bytes = LzmaUtils.Compress(Encoding.UTF8.GetBytes(csvBuilder.ToString()));
                using (var mem = new MemoryStream(bytes))
                {
                    mem.Write(bytes, 0, 9);
                    mem.Write(bytes, 12, bytes.Length - 13);
                    File.WriteAllBytes(path, mem.ToArray());
                }
            }
            else File.WriteAllText(path, csvBuilder.ToString());
        }
    }
}
