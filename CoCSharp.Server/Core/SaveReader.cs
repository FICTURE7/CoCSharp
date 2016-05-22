using System;
using System.Collections;
using System.IO;

namespace CoCSharp.Server.Core
{
    // Provides method to read default save files.
    public class SaveReader
    {
        private const string NullString = "null";

        public SaveReader(string saveStr)
        {
            if (string.IsNullOrWhiteSpace(saveStr))
                throw new ArgumentNullException("saveStr", "saveStr cannot be null or empty.");

            Table = new Hashtable();
            var lineNum = 0;
            var sperator = new char[] { '=' };
            using (var reader = new StringReader(saveStr))
            {
                while (reader.Peek() != -1)
                {
                    var line = reader.ReadLine();
                    lineNum++;

                    // Make sure the line contains a '=' character.
                    if (!line.Contains("="))
                        throw new FormatException("Missing '=' operator on line " + lineNum + ".");

                    var split = line.Split(sperator, count: 2);
                    //if (split.Length < 2)
                    //    throw new FormatException("Expected a field and a value on line " + lineNum + ".");

                    Table.Add(split[0], split[1]);
                }
            }
        }

        public Hashtable Table { get; private set; }

        // Returns the raw value from _table as a string.
        public string ReadRaw(string fieldName)
        {
            return (string)Table[fieldName];
        }

        public string ReadAsString(string fieldName)
        {
            var value = ReadRaw(fieldName);
            if (value == NullString)
                return null;

            var opIndex = value.IndexOf('"');
            if (opIndex == -1)
                return null;

            var clIndex = value.LastIndexOf('"');
            if (clIndex == -1)
                return null;

            var str = value.Substring(opIndex + 1, clIndex - 1);
            return str;
        }

        public int ReadAsInt(string fieldName)
        {
            var value = ReadRaw(fieldName);
            var outVal = default(int);
            //if (int.TryParse(value, out outVal))
            //    return outVal;
            int.TryParse(value, out outVal);
            return outVal;

            //throw new FormatException("Not a valid integer '" + value + "'.");
        }

        public long ReadAsLong(string fieldName)
        {
            var value = ReadRaw(fieldName);
            var outVal = default(long);
            //if (long.TryParse(value, out outVal))
            //    return outVal;
            long.TryParse(value, out outVal);
            return outVal;

            //throw new FormatException("Not a valid integer '" + value + "'.");
        }

        public bool ReadAsBoolean(string fieldName)
        {
            var value = ReadRaw(fieldName);
            var outVal = default(bool);
            //if (bool.TryParse(value, out outVal))
            //    return outVal;
            bool.TryParse(value, out outVal);
            return outVal;
            //throw new FormatException("Not a valid boolean'" + value + "'.");
        }
    }
}
