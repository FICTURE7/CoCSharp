using SevenZip;
using System;
using System.Data;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace CoCSharp.Csv
{
    /// <summary>
    /// Represents a Comma Seperated Values(CSV) file as a <see cref="DataTable"/>. 
    /// Mainly designed for the Clash of Clans CSV file format.
    /// </summary>
    public class CsvTable : IDisposable
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
        /// <exception cref="ArgumentNullException"><paramref name="path"/> is null.</exception>
        /// <exception cref="CsvException">Unexpected data type in CSV table.</exception>
        public CsvTable(string path) : this(path, false)
        {
            // Space
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CsvTable"/> class and decompresses
        /// the specified .csv file if compressed, then reads file and parses it.
        /// </summary>
        /// <param name="path">Path to the .csv file.</param>
        /// <param name="compressed">Whether the .csv file is compressed or not.</param>
        /// <exception cref="ArgumentNullException"><paramref name="path"/> is null.</exception>
        /// <exception cref="CsvException">Unexpected data type in CSV table.</exception>
        public CsvTable(string path, bool compressed)
        {
            if (path == null)
                throw new ArgumentNullException("path");

            Table = new DataTable();
            Load(path, compressed);
        }

        private static readonly Type s_stringType = typeof(string);
        private static readonly Type s_boolType = typeof(bool);
        private static readonly Type s_intType = typeof(int);

        private bool _disposed = false;

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
        /// Gets the row of the CSV file which defines the data types of the columns.
        /// </summary>
        public DataRow TypesRow { get { return Table.Rows[0]; } }

        /// <summary>
        /// Reads the specified .csv file from disk without compression.
        /// </summary>
        /// <param name="path">Path to the .csv file.</param>
        /// <exception cref="ArgumentNullException"><paramref name="path"/> is null.</exception>
        /// <exception cref="CsvException">Unexpected data type in CSV table.</exception>
        public void Load(string path)
        {
            if (path == null)
                throw new ArgumentNullException("path");

            var bytes = File.ReadAllBytes(path);
            Load(bytes);
        }

        /// <summary>
        /// Reads the specified .csv file from disk and decompresses it if specifed.
        /// </summary>
        /// <param name="path">Path to the .csv file.</param>
        /// <param name="compressed">Whether the .csv file is compressed or not.</param>
        /// <exception cref="ArgumentNullException"><paramref name="path"/> is null.</exception>
        /// <exception cref="CsvException">Unexpected data type in CSV table.</exception>
        /// <exception cref="ObjectDisposedException">CsvTable object was disposed.</exception>
        public void Load(string path, bool compressed)
        {
            if (path == null)
                throw new ArgumentNullException("path");

            var bytes = File.ReadAllBytes(path);
            Load(bytes, compressed);
        }

        /// <summary>
        /// Reads the specified .csv file from a <see cref="Byte"/> array without compression.
        /// </summary>
        /// <param name="bytes"><see cref="Byte"/> array of the .csv file.</param>
        /// <exception cref="ArgumentNullException"><paramref name="bytes"/> is null.</exception>
        /// <exception cref="CsvException">Unexpected data type in CSV table.</exception>
        /// <exception cref="ObjectDisposedException">CsvTable object was disposed.</exception>
        public void Load(byte[] bytes)
        {
            if (_disposed)
                throw new ObjectDisposedException(null, "Cannot access CsvTable object because it was disposed.");
            if (bytes == null)
                throw new ArgumentNullException("bytes");

            var rawCsv = Encoding.UTF8.GetString(bytes); // kinda silly

            var rows = Regex.Split(rawCsv.Replace("\"", string.Empty), "\r\n");
            var columnNames = Regex.Split(rows[0], ",");
            var columnTypes = Regex.Split(rows[1], ",");

            if (columnNames.Length != columnTypes.Length)
                throw new CsvException("Number of column and number of ");

            // manipulating directly Table is probably a bad idea
            // might wanna create a new table

            for (int i = 0; i < columnNames.Length; i++)  // populates datatable's columns loop
            {
                switch (columnTypes[i])
                {
                    case "String":
                    case "string":
                        Table.Columns.Add(columnNames[i], s_stringType);
                        break;

                    case "int":
                        Table.Columns.Add(columnNames[i], s_intType);
                        break;

                    case "Boolean":
                    case "boolean":
                        Table.Columns.Add(columnNames[i], s_boolType);
                        break;

                    default:
                        throw new CsvException(string.Format("Unexpected data type '{0}' at Row: {1}, Column: {2}.", columnTypes[i], 2, i + 1));
                }
            }

            for (int i = 2; i < rows.Length; i++) // turn empty("") fields to DBNull.Value and add them to table loop
            {
                var rowsValues = (object[])Regex.Split(rows[i], ",");
                var newRowsValues = new object[rowsValues.Length];

                for (int j = 0; j < rowsValues.Length; j++)
                    newRowsValues[j] = (string)rowsValues[j] == string.Empty ? DBNull.Value : rowsValues[j];

                Table.Rows.Add(newRowsValues);
            }
        }

        /// <summary>
        /// Reads the specified .csv file from a byte array and decompresses it if specifed.
        /// </summary>
        /// <param name="bytes">Bytes of the .csv file.</param>
        /// <param name="compressed">Whether the .csv file is compressed or not.</param>
        /// <exception cref="ArgumentNullException"><paramref name="bytes"/> is null.</exception>
        /// <exception cref="CsvException">Unexpected data type in CSV table.</exception>
        /// <exception cref="ObjectDisposedException">CsvTable object was disposed.</exception>
        public void Load(byte[] bytes, bool compressed)
        {
            if (bytes == null)
                throw new ArgumentNullException("bytes");

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
        /// <exception cref="ArgumentNullException"><paramref name="path"/> is null.</exception>
        /// <exception cref="ObjectDisposedException">CsvTable object was disposed.</exception>
        public void Save(string path)
        {
            if (path == null)
                throw new ArgumentNullException("path");

            Save(path, false);
        }

        /// <summary>
        /// Saves this <see cref="CsvTable"/> on disk with the specified path and with compression
        /// if specified.
        /// </summary>
        /// <param name="path">Path of the .csv to save to.</param>
        /// <param name="compressed">Whether to compress the file or not.</param>
        /// <exception cref="ArgumentNullException"><paramref name="path"/> is null.</exception>
        /// <exception cref="ObjectDisposedException">CsvTable object was disposed.</exception>
        public void Save(string path, bool compressed)
        {
            if (_disposed)
                throw new ObjectDisposedException(null, "Cannot access CsvTable object because it was disposed.");
            if (path == null)
                throw new ArgumentNullException("path");

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

        /// <summary>
        /// Releases all resources used by the current instance of the <see cref="CsvTable"/> class.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }

        /// <summary>
        /// Releases all unmanged resources and optionally releases managed resources
        /// used by the current instance of the <see cref="CsvTable"/> class.
        /// </summary>
        /// <param name="disposing">Releases managed resources if set to <c>true</c>.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                if (Table != null)
                    Table.Dispose();
            }
            _disposed = true;
        }
    }
}
