using CoCSharp.Logic;
using System.IO;

namespace CoCSharp.Database.Csv
{
    public class CsvRow
    {
        public CsvRow(string[] value)
        {
            this.Records = value;
        }

        public string this[int index]
        {
            get { return Records[index]; }
            set { Records[index] = value; }
        }
       
        private string[] Records { get; set; }

        public string GetRecord(int column)
        {
            var str = Records[column].Replace("\"", "");
            return str;
        }

        public bool GetRecordAsBool(int column)
        {
            var value = GetRecord(column);
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

        public int GetRecordAsInt(int column)
        {
            var value = GetRecord(column);
            return value == string.Empty ? 0 : int.Parse(value);
        }

        public Resource GetRecordAsResource(int column)
        {
            var value = GetRecord(column);
            switch (value)
            {
                case "":
                    return 0;

                case "Gold":
                    return Resource.Gold;

                case "Elixir":
                    return Resource.Elixir;

                case "DarkElixir":
                    return Resource.DarkElixir;

                case "Diamonds":
                    return Resource.Gem;

                default:
                    throw new InvalidDataException(value + " is not a valid resource string.");
            }
        }
    }
}
