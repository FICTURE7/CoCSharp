using SevenZip;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;

using LZMA = SevenZip.Compression.LZMA;

namespace CoCSharp.Csv
{
    /// <summary>
    /// Represents a Comma Separated Values(CSV) file as a <see cref="DataTable"/>. 
    /// Mainly designed for the Supercell's CSV file format.
    /// </summary>
    public class CsvTable : IDisposable
    {
        #region Constants
        private static int s_dictionarySize = 262144;
        private static bool s_endOfStream = false;

        private static CoderPropID[] s_propertiesIds =
        {
            CoderPropID.DictionarySize,
            CoderPropID.PosStateBits,
            CoderPropID.LitContextBits,
            CoderPropID.LitPosBits,
            CoderPropID.Algorithm,
            CoderPropID.NumFastBytes,
            CoderPropID.MatchFinder,
            CoderPropID.EndMarker,
        };

        private static object[] s_properties =
        {
            s_dictionarySize,
            2,
            3,
            0,
            2,
            32,
            "bt4",
            s_endOfStream
        };

        private static readonly byte[] s_signature =
        {
            // Properties.
            0x5D,

            // Dictionary size.
            0x00,
            0x00,
            0x04,
            0x00
        };
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="CsvTable"/> class.
        /// </summary>
        public CsvTable()
        {
            _table = new DataTable();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CsvTable"/> class and 
        /// reads the specified .csv file and parses it without decompressing it.
        /// </summary>
        /// <param name="path">Path to the .csv file.</param>
        /// <exception cref="ArgumentNullException"><paramref name="path"/> is null.</exception>
        /// <exception cref="CsvException">Unexpected data type in CSV table.</exception>
        public CsvTable(string path) : this(path, CsvTableCompression.Auto)
        {
            // Space
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CsvTable"/> class and decompresses
        /// the specified .csv file if compressed, then reads file and parses it.
        /// </summary>
        /// <param name="path">Path to the .csv file.</param>
        /// <param name="compression">Compression kind of the table.</param>
        /// <exception cref="ArgumentNullException"><paramref name="path"/> is null.</exception>
        /// <exception cref="CsvException">Unexpected data type in CSV table.</exception>
        public CsvTable(string path, CsvTableCompression compression)
        {
            if (path == null)
                throw new ArgumentNullException("path");

            _disposed = false;

            // Set file name without the extension as the table name.
            var tableName = Path.GetFileNameWithoutExtension(path);
            _table = new DataTable(tableName);

            var fileStream = new FileStream(path, FileMode.Open);
            LoadInternal(fileStream, compression);
        }
        #endregion

        #region Fields & Properties
        private bool _disposed;

        private DataTable _table;
        /// <summary>
        /// Gets the <see cref="DataTable"/> of the CSV file.
        /// </summary>
        public DataTable Table
        {
            get
            {
                CheckDisposed();

                return _table;
            }
        }

        /// <summary>
        /// Gets the rows of the CSV file.
        /// </summary>
        public DataRowCollection Rows
        {
            get
            {
                CheckDisposed();

                return Table.Rows;
            }
        }

        /// <summary>
        /// Gets the columns of the CSV file.
        /// </summary>
        public DataColumnCollection Columns
        {
            get
            {
                CheckDisposed();

                return Table.Columns;
            }
        }
        #endregion

        #region Methods
        #region Loading
        /// <summary>
        /// Reads the specified .csv file from disk without compression.
        /// </summary>
        /// <param name="path">Path to the .csv file.</param>
        /// <exception cref="ArgumentNullException"><paramref name="path"/> is null.</exception>
        /// <exception cref="CsvException">Unexpected data type in CSV table.</exception>
        /// <exception cref="ObjectDisposedException"><see cref="CsvTable"/> object was disposed.</exception>
        public void Load(string path)
        {
            Load(path, CsvTableCompression.Auto);
        }

        /// <summary>
        /// Reads the specified .csv file from disk and decompresses it if specified.
        /// </summary>
        /// <param name="path">Path to the .csv file.</param>
        /// <param name="compression">Compression kind of the table.</param>
        /// <exception cref="ArgumentNullException"><paramref name="path"/> is null.</exception>
        /// <exception cref="CsvException">Unexpected data type in CSV table.</exception>
        /// <exception cref="ObjectDisposedException"><see cref="CsvTable"/> object was disposed.</exception>
        public void Load(string path, CsvTableCompression compression)
        {
            CheckDisposed();

            if (path == null)
                throw new ArgumentNullException("path");

            var fileStream = new FileStream(path, FileMode.Open);
            LoadInternal(fileStream, compression);
        }

        /// <summary>
        /// Reads the specified .csv file from a byte array without compression.
        /// </summary>
        /// <param name="bytes">Bytes array of the .csv file.</param>
        /// <exception cref="ArgumentNullException"><paramref name="bytes"/> is null.</exception>
        /// <exception cref="CsvException">Unexpected data type in CSV table.</exception>
        /// <exception cref="ObjectDisposedException"><see cref="CsvTable"/> object was disposed.</exception>
        public void Load(byte[] bytes)
        {
            Load(bytes, CsvTableCompression.Auto);
        }

        /// <summary>
        /// Reads the specified .csv file from a byte array and decompresses it if specified.
        /// </summary>
        /// <param name="bytes">Bytes of the .csv file.</param>
        /// <param name="compression">Whether the .csv file is compressed or not.</param>
        /// <exception cref="ArgumentNullException"><paramref name="bytes"/> is null.</exception>
        /// <exception cref="CsvException">Unexpected data type in CSV table.</exception>
        /// <exception cref="ObjectDisposedException"><see cref="CsvTable"/> object was disposed.</exception>
        public void Load(byte[] bytes, CsvTableCompression compression)
        {
            CheckDisposed();

            if (bytes == null)
                throw new ArgumentNullException("bytes");

            var stream = new MemoryStream(bytes);
            LoadInternal(stream, compression);
        }
        #endregion

        #region Saving
        /// <summary>
        /// Saves this <see cref="CsvTable"/> on disk with the specified path and with compression
        /// if specified.
        /// </summary>
        /// <param name="path">Path of the .csv to save to.</param>
        /// <param name="compression">Whether to compress the file or not.</param>
        /// 
        /// <exception cref="ArgumentNullException"><paramref name="path"/> is null.</exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="compression"/> is not either <see cref="CsvTableCompression.Compressed"/> or <see cref="CsvTableCompression.Uncompressed"/>
        /// </exception>
        /// <exception cref="ObjectDisposedException"><see cref="CsvTable"/> object was disposed.</exception>
        public void Save(string path, CsvTableCompression compression)
        {
            if (_disposed)
                throw new ObjectDisposedException(null, "Cannot access CsvTable object because it was disposed.");
            if (path == null)
                throw new ArgumentNullException("path");
            if (compression < CsvTableCompression.Compressed || compression > CsvTableCompression.Uncompressed)
                throw new ArgumentException("Unexpected compression type.", "compression");

            // Write column names.
            var builder = new StringBuilder();
            for (int i = 0; i < Table.Columns.Count - 1; i++)
                builder.Append(Columns[i].ColumnName + ",");
            builder.AppendLine(Columns[Columns.Count - 1].ColumnName);

            // Write column types.
            for (int i = 0; i < Table.Columns.Count; i++)
            {
                // Append "\r\n"(new line) if last in array else append ",".
                var format = i == Table.Columns.Count - 1 ? "{0}\r\n" : "{0},";
                var dataType = Table.Columns[i].DataType;
                if (dataType == typeof(int))
                {
                    builder.AppendFormat(format, "int");
                }
                else
                {
                    builder.AppendFormat(format, dataType.Name);
                }
            }

            // Write all rows.
            for (int i = 0; i < Table.Rows.Count; i++)
                builder.AppendLine(string.Join(",", Table.Rows[i].ItemArray));

            // Compress it if specified.
            if (compression == CsvTableCompression.Compressed)
            {
                using (var inStream = new MemoryStream(Encoding.UTF8.GetBytes(builder.ToString())))
                using (var outStream = (MemoryStream)CompressInternal(inStream))
                {
                    File.WriteAllBytes(path, outStream.ToArray());
                }
            }
            else if (compression == CsvTableCompression.Uncompressed)
            {
                File.WriteAllText(path, builder.ToString());
            }
        }
        #endregion

        #region Dispose 
        /// <summary>
        /// Releases all resources used by the current instance of the <see cref="CsvTable"/> class.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }

        /// <summary>
        /// Releases all unmanaged resources and optionally releases managed resources
        /// used by the current instance of the <see cref="CsvTable"/> class.
        /// </summary>
        /// <param name="disposing">Releases managed resources if set to <c>true</c>.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                _table.Dispose();
                _table = null;
            }
            _disposed = true;
        }
        #endregion

        #region Compression
        /// <summary>
        /// Decompresses LZMA compressed data from the specified byte array and returns a new array containing
        /// the decompressed data.
        /// </summary>
        /// <param name="bytes">Byte array containing the LZMA data to decompress.</param>
        /// <returns>Decompressed byte array.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="bytes"/> is null.</exception>
        public static byte[] Decompress(byte[] bytes)
        {
            if (bytes == null)
                throw new ArgumentNullException("bytes");

            using (var inStream = new MemoryStream(bytes))
            using (var outStream = (MemoryStream)DecompressInternal(inStream))
                return outStream.ToArray();
        }

        /// <summary>
        /// Decompresses LZMA compressed data from the specified stream and returns a new <see cref="MemoryStream"/> containing
        /// the decompressed data.
        /// </summary>
        /// <param name="stream"><see cref="Stream"/> from which to decompress the LZMA data.</param>
        /// <exception cref="ArgumentNullException"><paramref name="stream"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="stream.CanRead"/> is <c>false</c>.</exception>
        public static Stream Decompress(Stream stream)
        {
            if (stream == null)
                throw new ArgumentNullException("stream");
            if (!stream.CanRead)
                throw new ArgumentException("stream must be readable.", "stream");

            return DecompressInternal(stream);
        }

        /// <summary>
        /// Compresses the specified byte array into LZMA.
        /// </summary>
        /// <param name="bytes">Byte array to compress into LZMA.</param>
        /// <returns>Compressed byte array.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="bytes"/> is null.</exception>
        public static byte[] Compress(byte[] bytes)
        {
            if (bytes == null)
                throw new ArgumentNullException("bytes");

            using (var inStream = new MemoryStream(bytes))
            using (var outStream = (MemoryStream)CompressInternal(inStream))
                return outStream.ToArray();
        }

        /// <summary>
        /// Compresses the specified string into LZMA.
        /// </summary>
        /// <param name="csv">UTF-8 string to compress into LZMA.</param>
        /// <returns>Compressed byte array.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="csv"/> is null.</exception>
        public static byte[] Compress(string csv)
        {
            if (csv == null)
                throw new ArgumentNullException("csv");

            return Compress(Encoding.UTF8.GetBytes(csv));
        }

        /// <summary>
        /// Compresses the specified <see cref="Stream"/> into LZMA.
        /// </summary>
        /// <param name="stream"><see cref="Stream"/> to compress into LZMA.</param>
        /// <returns>Compressed byte array.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="stream"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="stream.CanRead"/> is <c>false</c>.</exception>
        public static Stream Compress(Stream stream)
        {
            if (stream == null)
                throw new ArgumentNullException("stream");
            if (!stream.CanRead)
                throw new ArgumentException("stream should be readable.", "stream");

            return CompressInternal(stream);
        }
        #endregion

        // Loads the specified CSV file stream and compression kind, into the DataTable.
        private void LoadInternal(Stream stream, CsvTableCompression compression)
        {
            // Detect compression if it compression is to Auto.
            if (compression == CsvTableCompression.Auto)
                compression = DetectCompression(stream);

            // If we detected that the CSV file is compressed or it was already specified,
            // we decompress it.
            if (compression == CsvTableCompression.Compressed)
            {
                var newStream = DecompressInternal(stream);

                stream.Seek(0, SeekOrigin.Begin);
                // Just in case uncompressed data is smaller than compressed data.
                stream.SetLength(newStream.Length);
                newStream.CopyTo(stream);
            }

            // stream will be disposed as well when reader is disposed.
            using (var reader = (TextReader)new StreamReader(stream))
            {
                stream.Seek(0, SeekOrigin.Begin);

                var strTable = ParseTable(reader);
                // First row of CSV files are the property names.
                var rowNames = strTable[0];
                if (rowNames == null)
                    throw new CsvException("CSV file does not contain the names row, that is, the first row.");

                // Second row of the CSV files are the property types.
                var rowTypes = strTable[1];
                if (rowTypes == null)
                    throw new CsvException("CSV file does not contain the types row, that is, the second row.");

                // Width of table is determined by the first row.
                var tableWidth = rowNames.Length;
                // Set the columns of the DataTable according the first and second row
                SetColumns(rowNames, rowTypes);

                // i = 2, because we want to skip the first 2 rows.
                for (int i = 2; i < strTable.Length; i++)
                {
                    // Raw string row from the file.
                    var row = strTable[i];
                    // DataRow that we'll add to the Table.
                    var tableRow = Table.NewRow();
                    Table.Rows.Add(tableRow);

                    for (int j = 0; j < row.Length; j++)
                    {
                        var column = row[j];
                        // If the string is null (should not happen) or empty,
                        // we set its table value to be DBNull.Value.
                        if (string.IsNullOrEmpty(column))
                        {
                            tableRow[j] = DBNull.Value;
                        }
                        else
                        {
                            tableRow[j] = column;
                        }
                    }
                }
            }
        }

        // Set the Table columns names and type.
        private void SetColumns(string[] rowNames, string[] rowTypes)
        {
            var tableWidth = rowNames.Length;
            for (int i = 0; i < tableWidth; i++)
            {
                var name = rowNames[i];
                var type = (Type)null;

                var columnType = rowTypes[i].ToLower();

                switch (columnType)
                {
                    case "string":
                        type = typeof(string);
                        break;

                    case "boolean":
                        type = typeof(bool);
                        break;

                    case "int":
                        type = typeof(int);
                        break;

                    default:
                        throw new CsvException(string.Format("Unexpected data type '{0}' at Row: {1}, Column: {2}.", columnType, 2, i + 1));
                }
                Columns.Add(name, type);
            }
        }

        private void CheckDisposed()
        {
            if (_disposed)
                throw new ObjectDisposedException(null, "Cannot access CsvTable object because it was disposed.");
        }

        // Reads a row from the TextReader and removes the first and the last inverted commas as well.
        private static string[] ReadRow(TextReader reader)
        {
            var row = reader.ReadLine();
            if (row == null)
                return null;

            var inCommas = false;
            var columns = new List<string>(64);
            var columnValue = string.Empty;
            for (int i = 0; i < row.Length; i++)
            {
                var c = row[i];
                if (c == '"')
                {
                    if (inCommas)
                        inCommas = false;
                    else
                        inCommas = true;
                }
                else if (c == ',' && !inCommas)
                {
                    columns.Add(columnValue);
                    columnValue = string.Empty;
                }
                else
                {
                    columnValue += c;
                }
            }
            columns.Add(columnValue);

            return columns.ToArray();
        }

        // Parses the table into a table of strings.
        private static string[][] ParseTable(TextReader reader)
        {
            var rows = new List<string[]>();
            var row = ReadRow(reader);
            var rowLn = 1;
            var tableWidth = row.Length;

            do
            {
                if (row.Length != tableWidth)
                    throw new CsvException(string.Format("CSV file has an inconsistent table width. Expected row at line {0} to have a width of {1} but was {2}",
                                                         rowLn, tableWidth, row.Length));

                rows.Add(row);
                rowLn++;
            }
            while ((row = ReadRow(reader)) != null);

            return rows.ToArray();
        }

        private static Stream DecompressInternal(Stream inStream)
        {
            var outStream = new MemoryStream();
            var decoder = new LZMA.Decoder();

            var properties = new byte[5];
            if (inStream.Read(properties, 0, 5) != 5)
            {
                throw new InvalidDataException("Stream has an invalid header.");
            }
            else
            {
                // Make sure we have the 4 bytes representing the uncompressed size.
                // The Clash of Clans LZMA header's uncompressed size is 4 bytes instead of 8.
                var outSizeBytes = new byte[4];
                if (inStream.Read(outSizeBytes, 0, 4) != 4)
                    throw new InvalidDataException("Stream has an invalid header.");

                var inSize = inStream.Length - inStream.Position;
                var outSize = BitConverter.ToInt32(outSizeBytes, 0);
                decoder.SetDecoderProperties(properties);
                decoder.Code(inStream, outStream, inSize, outSize, null);
            }

            outStream.Seek(0, SeekOrigin.Begin);
            return outStream;
        }

        private static Stream CompressInternal(Stream inStream)
        {
            var outStream = new MemoryStream();
            var encoder = new LZMA.Encoder();
            encoder.SetCoderProperties(s_propertiesIds, s_properties);
            encoder.WriteCoderProperties(outStream);

            // Uncompressed size.
            var outSize = inStream.Length;
            var outSizeBytes = BitConverter.GetBytes(outSize);
            outStream.Write(outSizeBytes, 0, 4);

            encoder.Code(inStream, outStream, -1, -1, null);

            outStream.Seek(0, SeekOrigin.Begin);
            return outStream;
        }

        private static CsvTableCompression DetectCompression(Stream stream)
        {
            var initialPos = stream.Position;
            // Look for the LZMA header.
            var properties = new byte[5];

            // If we don't have that 5 bytes of header, its not compressed.
            if (stream.Read(properties, 0, 5) != 5)
            {
                goto ReturnUncompressed;
            }
            else
            {
                for (int i = 0; i < s_signature.Length; i++)
                {
                    if (s_signature[i] != properties[i])
                        goto ReturnUncompressed;
                }

                // Make sure we have the 4 bytes representing the uncompressed size.
                // The Clash of Clans LZMA header's uncompressed size is 4 bytes instead of 8.
                var outSize = new byte[4];
                if (stream.Read(outSize, 0, 4) != 4)
                    goto ReturnUncompressed;

                goto ReturnCompressed;
            }

            ReturnUncompressed:
            stream.Seek(initialPos, SeekOrigin.Begin);
            return CsvTableCompression.Uncompressed;

            ReturnCompressed:
            stream.Seek(initialPos, SeekOrigin.Begin);
            return CsvTableCompression.Compressed;
        }
        #endregion
    }
}
