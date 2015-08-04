using System;
using System.IO;

namespace CoCSharp.Data.Csv
{
    [Obsolete("Use CsvTable.Rows instead.")]
    public class CsvRow
    {
        public CsvRow(string[] value)
        {
            Records = value;
        }

        public string this[int index]
        {
            get { return Records[index]; }
            set { Records[index] = value; }
        }
       
        private string[] Records { get; set; }

        public string ReadRecordAsString(int column)
        {
            var str = Records[column].Replace("\"", "");
            return str;
        }

        public bool ReadRecordAsBool(int column)
        {
            var value = ReadRecordAsString(column);
            switch (value)
            {
                case "TRUE":
                    return true;
                case "FALSE":
                    return false;
                default:
                    throw new InvalidDataException(value + " is not a valid bool string.");
            }
        }

        public int ReadRecordAsInt(int column)
        {
            var value = ReadRecordAsString(column);
            return value == string.Empty ? 0 : int.Parse(value);
        }
    }
}
