using System;
using System.Collections.Generic;
using System.Text;

namespace CoCSharp.Logging
{
    /// <summary>
    /// Provides methods to build a <see cref="Log"/> object.
    /// </summary>
    public sealed class LogBuilder
    {
        //TODO: Use Environment.NewLine instead of "\r\n".
        /// <summary>
        /// Initializes a new instance of the <see cref="LogBuilder"/> class.
        /// </summary>
        public LogBuilder()
        {
            // Space
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogBuilder"/> class with
        /// the specified log <see cref="string"/>.
        /// </summary>
        /// <param name="log"></param>
        public LogBuilder(string log)
        {
            m_StringBuilder.Append(log);
        }

        /// <summary>
        /// Gets the name of the current block.
        /// Returns null if no blocks is opened.
        /// </summary>
        public string CurrentBlock
        {
            get { return m_CurrentBlockIndex == -1 ? null : m_Blocks[m_CurrentBlockIndex]; }
        }

        private string m_CurrentIndent = string.Empty;
        private int m_CurrentIndentDepth = 0;
        private int m_CurrentBlockIndex = -1;
        private StringBuilder m_StringBuilder = new StringBuilder();
        private List<string> m_Blocks = new List<string>();

        private const string Indent = "    ";
        private const LoggingFlags DefaultFlags = LoggingFlags.Fields | LoggingFlags.Unknowns;

        /// <summary>
        /// Opens a new block to this instance with the
        /// specified name.
        /// </summary>
        /// <param name="name">The name of the block.</param>
        /// <returns>A reference to this instance.</returns>
        public LogBuilder OpenBlock(string name)
        {
            m_CurrentBlockIndex++;
            m_Blocks.Add(name);

            var firstIndent = m_CurrentIndentDepth != 0 ? Indent : m_CurrentIndent;
            var format = firstIndent + "{0} {1}\r\n" + m_CurrentIndent + "{{\r\n" + m_CurrentIndent;
            m_StringBuilder.AppendFormat(format, DateTime.Now.ToString("[~HH:mm:ss.fff]"), name);
            IncrementIndentation();
            //var debug = m_StringBuilder.ToString();
            return this;
        }

        /// <summary>
        /// Closes the latest opened block in this instance.
        /// </summary>
        /// <returns>A reference to this instance.</returns>
        public LogBuilder CloseBlock()
        {
            if (m_CurrentBlockIndex < 0)
                throw new InvalidOperationException("No blocks to close, call 'OpenBlock' first.");

            m_Blocks.RemoveAt(m_CurrentBlockIndex); // remove block from list.
            m_CurrentBlockIndex--;
            DecrementIndentation();

            var stringFormat = m_CurrentBlockIndex == -1 ? "\r\n{0}}}\r\n\r\n" : "\r\n{0}}}"; // check if last block
            m_StringBuilder.AppendFormat(stringFormat, m_CurrentIndent);
            //var debug = m_StringBuilder.ToString();
            return this;
        }

        /// <summary>
        /// Opens and closes an empty block.
        /// </summary>
        /// <param name="name">The name of the block.</param>
        /// <returns>A reference to this instace.</returns>
        public LogBuilder EmptyBlock(string name)
        {
            m_StringBuilder.AppendFormat("{0} {1} {{ }}\r\n\r\n" + m_CurrentIndent, DateTime.Now.ToString("[~HH:mm:ss.fff]"), name);
            //var debug = m_StringBuilder.ToString();
            return this;
        }

        /// <summary>
        /// Appends an <see cref="object"/> to this instance in
        /// a formatted way.
        /// </summary>
        /// <param name="obj">Object to append.</param>
        /// <returns>A reference to this instance.</returns>
        public LogBuilder AppendObject(object obj)
        {
            m_StringBuilder.Append(DumpObject(obj, DefaultFlags));
            //var debug = m_StringBuilder.ToString();
            return this;
        }

        /// <summary>
        /// Appends an <see cref="object"/> to this instance in
        /// a formatted way.
        /// </summary>
        /// <param name="obj">Object to append.</param>
        /// <param name="flags"><see cref="LoggingFlags"/> to instruct the <see cref="LogBuilder"/>.</param>
        /// <returns>A reference to this instance.</returns>
        public LogBuilder AppendObject(object obj, LoggingFlags flags)
        {
            m_StringBuilder.Append(DumpObject(obj, flags));
            //var debug = m_StringBuilder.ToString();
            return this;
        }

        /// <summary>
        /// Converts this instance into a <see cref="string"/>
        /// </summary>
        /// <returns>A string whose value is the same as this instance.</returns>
        public override string ToString()
        {
            return m_StringBuilder.ToString();
        }

        private string DumpObject(object obj, LoggingFlags flags)
        {
            var objType = obj.GetType();
            var objFields = objType.GetFields();
            var objProperties = objType.GetProperties();
            var objLog = string.Empty;

            if (flags.HasFlag(LoggingFlags.Properties))
            {
                //TODO: Remove this dirty code repetition.
                for (int i = 0; i < objProperties.Length; i++)
                {
                    var property = objProperties[i];
                    var propertyName = property.Name;

                    if (flags.HasFlag(LoggingFlags.Unknowns)) // check if we loggin unknowns
                        if (propertyName.StartsWith("Unknown"))
                            continue;

                    var propertyValue = property.GetValue(obj);
                    propertyValue = DumpObjectValue(propertyValue);

                    objLog += m_CurrentIndent + propertyName + ": " + propertyValue;
                    if (i != objProperties.Length - 1)
                        objLog += "\r\n";
                }
            }

            if (flags.HasFlag(LoggingFlags.Fields))
            {
                for (int i = 0; i < objFields.Length; i++)
                {
                    var field = objFields[i];
                    var fieldName = field.Name;

                    if (flags.HasFlag(LoggingFlags.Unknowns)) // check if we loggin unknowns
                        if (fieldName.StartsWith("Unknown"))
                            continue;

                    var fieldValue = field.GetValue(obj);
                    fieldValue = DumpObjectValue(fieldValue);

                    objLog += m_CurrentIndent + fieldName + ": " + fieldValue;
                    if (i != objFields.Length - 1)
                        objLog += "\r\n";
                }
            }
            //var debug = m_StringBuilder.ToString();
            return objLog;
        }

        private string DumpArray(Array array)
        {
            if (array.Length == 0)
                return "[]";

            var strBuilder = new StringBuilder();
            var count = 0;
            strBuilder.Append("\r\n" + m_CurrentIndent + "[\r\n");
            IncrementIndentation();

            foreach (var obj in array)
            {
                var objValue = DumpObjectValue(obj);
                strBuilder.Append(m_CurrentIndent + objValue);
                if (count != array.Length - 1)
                    strBuilder.Append("\r\n");
                count++;
            }

            DecrementIndentation();
            strBuilder.Append("\r\n" + m_CurrentIndent + "]");
            var debug = strBuilder.ToString();
            return strBuilder.ToString();
        }

        private object DumpObjectValue(object obj)
        {
            var value = obj;
            if (obj == null)
                value = "null";
            else if (obj is string)
                value = "\"" + obj + "\"";
            else if (obj is byte[])
                value = DumpByteArray((byte[])obj);
            else if (obj is Array)
                value = DumpArray((Array)obj);
            return value;
        }

        private string DumpByteArray(byte[] bytes)
        {
            if (bytes.Length == 0)
                return "[]";

            var strBuilder = new StringBuilder();
            strBuilder.Append("\r\n" + m_CurrentIndent + "[");
            IncrementIndentation();

            for (int i = 0; i < bytes.Length; i++)
            {
                if (i % 32 == 0) // add new lines every 32 bytes
                    strBuilder.Append("\r\n" + m_CurrentIndent);
                strBuilder.Append(bytes[i].ToString("X2") + " ");
            }

            DecrementIndentation();
            strBuilder.Append("\r\n" + m_CurrentIndent + "]");
            return strBuilder.ToString();
        }

        private void IncrementIndentation()
        {
            m_CurrentIndent += Indent;
            m_CurrentIndentDepth++;
        }

        private void DecrementIndentation()
        {
            if (m_CurrentIndentDepth == 0)
                return;

            m_CurrentIndent = m_CurrentIndent.Remove(m_CurrentIndent.Length - 4);
            m_CurrentIndentDepth--;
        }
    }
}
