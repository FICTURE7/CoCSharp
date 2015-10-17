using System;
using System.Collections.Generic;
using System.Text;

namespace CoCSharp.Logging
{
    /// <summary>
    /// Provides methods to create <see cref="Log"/> object.
    /// </summary>
    public sealed class LogBuilder
    {
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
        /// </summary>
        public string CurrentBlock
        {
            get
            {
                return m_CurrentBlockIndex == -1 ? null : m_Blocks[m_CurrentBlockIndex];
            }
        }

        private int m_IndentDepth = 0;
        private string m_Indent = string.Empty;
        private int m_CurrentBlockIndex = -1;
        private StringBuilder m_StringBuilder = new StringBuilder();
        private List<string> m_Blocks = new List<string>();

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

            m_StringBuilder.AppendFormat("{0} {1}\r\n" + m_Indent, DateTime.Now.ToString("[~HH:mm:ss.fff]"), name);
            IncrementIndentation();
            m_StringBuilder.Append("{\r\n" + m_Indent);

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

            m_Blocks.RemoveAt(m_CurrentBlockIndex);
            m_CurrentBlockIndex--;
            DecrementIndentation();

            var stringFormat = m_CurrentBlockIndex == -1 ? "\r\n{0}}}\r\n\r\n" : "\r\n{0}}}"; // check if last block
            m_StringBuilder.AppendFormat(stringFormat, m_Indent);

            return this;
        }

        /// <summary>
        /// Opens and closes an empty block. Just for formatting sake.
        /// </summary>
        /// <param name="name">The name of the block.</param>
        /// <returns>A reference to this instace.</returns>
        public LogBuilder EmptyBlock(string name)
        {
            m_StringBuilder.AppendFormat("{0} {1} {{ }}\r\n\r\n" + m_Indent, DateTime.Now.ToString("[~HH:mm:ss.fff]"), name);
            return this;
        }

        /// <summary>
        /// Appends an <see cref="object"/> to this instance in
        /// a formatted way.
        /// </summary>
        /// <returns>A reference to this instance.</returns>
        public LogBuilder AppendObject(object obj)
        {
            m_StringBuilder.Append(DumpObject(obj));
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

        private string DumpObject(object obj)
        {
            var strBuilder = new StringBuilder();
            var objType = obj.GetType();
            var objName = objType.Name;
            var objFields = objType.GetFields();
            var objProperties = objType.GetProperties();

            for (int i = 0; i < objProperties.Length; i++) // dump each property
            {
                var property = objProperties[i];
                var propertyName = property.Name;
                var propertyValue = property.GetValue(obj);
                var propertyString = propertyName + ": "; // using this to reduce number of 'Append's

                // dump properties according to there Type (could use a delegate for this)
                if (propertyValue == null)
                {
                    propertyString += "null";
                }
                else if (propertyValue is string)
                {
                    var str = propertyValue as string;
                    propertyString += "\"" + str + "\"";
                }
                else if (propertyValue is int ||
                         propertyValue is uint ||
                         propertyValue is long ||
                         propertyValue is ulong ||
                         propertyValue is float)
                {
                    propertyString += propertyValue.ToString();
                }
                else if (propertyValue is byte[])
                {
                    var dumpByteArray = DumpByteArray((byte[])propertyValue);
                    propertyString += dumpByteArray;
                }
                else if (propertyValue is Array)
                {
                    var dumpArray = DumpArray((Array)propertyValue);
                    propertyString += dumpArray;
                }
                else // if no dumping method was found then
                {
                    try // tries to dump 'propertyValue' properties and fields which was inside 'obj'
                    {
                        IncrementIndentation();

                        strBuilder.Append("[\r\n" + m_Indent);
                        propertyString += DumpObject(propertyValue);

                        DecrementIndentation();
                        strBuilder.Append(propertyString + m_Indent + "]");
                        //var debug = strBuilder.ToString();
                        continue; // for indentation sake
                    }
                    catch // failed, then dump 'propertyValue' itself
                    {
                        propertyString += propertyValue.ToString();
                    }
                }

                if (i == objProperties.Length - 1) // dont add an unnecessary indent at the end
                    strBuilder.Append(propertyString + "\r\n");
                else
                    strBuilder.Append(propertyString + "\r\n" + m_Indent);

                //var debug = strBuilder.ToString();
            }
            return strBuilder.ToString();
        }

        private string DumpArray(Array array)
        {
            var strBuilder = new StringBuilder();
            var count = 0; // number of iteration
            if (array.Length == 0)
            {
                strBuilder.Append("[]");
                return strBuilder.ToString();
            }

            strBuilder.Append("\r\n" + m_Indent + "[\r\n");
            IncrementIndentation();
            strBuilder.Append(m_Indent);
            foreach (var obj in array)
            {
                try // tries to dump object properties and fields inside of array
                {
                    var dump = DumpObject(obj); 
                    strBuilder.Append(dump);
                }
                catch // failed, then dump 'obj' itself
                {
                    var str = string.Empty;

                    // dump properties according to there Type (could use a delegate for this)
                    if (obj == null)
                    {
                        str += "null";
                    }
                    else if (obj is string)
                    {
                        var strValue = obj as string;
                        str += "\"" + strValue + "\"";
                    }
                    else if (obj is byte[])
                    {
                        var dumpByteArray = DumpByteArray((byte[])obj);
                        str += dumpByteArray;
                    }
                    else if (obj is Array)
                    {
                        var dumpArray = DumpArray((Array)obj);
                        str += dumpArray;
                    }
                    else // if no dumping method was found then dump 'obj' itself
                    {
                        str += obj.ToString();
                    }

                    if (count == array.Length - 1) // dont add an unnecessary indent at the end
                        strBuilder.Append(str + "\r\n"); 
                    else
                        strBuilder.Append(str + "\r\n" + m_Indent);
                }
                count++;
            }
            DecrementIndentation();
            strBuilder.Append(m_Indent + "]");

            //var debug = strBuilder.ToString();
            return strBuilder.ToString();
        }

        private string DumpByteArray(byte[] bytes)
        {
            var strBuilder = new StringBuilder();
            if (bytes.Length == 0)
            {
                strBuilder.Append("[]");
                return strBuilder.ToString();
            }

            strBuilder.Append("\r\n" + m_Indent + "[");
            IncrementIndentation();
            for (int i = 0; i < bytes.Length; i++)
            {
                if (i % 32 == 0) // add new lines every 32 bytes
                    strBuilder.Append("\r\n" + m_Indent);
                strBuilder.Append(bytes[i].ToString("X2") + " ");
            }
            DecrementIndentation();
            strBuilder.Append("\r\n" + m_Indent + "]");
            return strBuilder.ToString();
        }

        private void IncrementIndentation()
        {
            m_Indent += "    ";
            m_IndentDepth++;
        }

        private void DecrementIndentation()
        {
            if (m_IndentDepth == 0)
                return;

            m_Indent = m_Indent.Remove(m_Indent.Length - 4);
            m_IndentDepth--;
        }
    }
}
