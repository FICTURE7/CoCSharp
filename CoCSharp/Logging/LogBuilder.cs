using System;
using System.Collections.Generic;
using System.Text;

namespace CoCSharp.Logging
{
    /// <summary>
    /// 
    /// </summary>
    public class LogBuilder
    {
        /// <summary>
        /// 
        /// </summary>
        public LogBuilder()
        {
            // Space
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="log"></param>
        public LogBuilder(string log)
        {
            m_StringBuilder.Append(log);
        }

        /// <summary>
        /// 
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
        /// 
        /// </summary>
        /// <returns></returns>
        public LogBuilder OpenBlock(string name)
        {
            m_CurrentBlockIndex++;
            m_Blocks.Add(name);

            m_StringBuilder.AppendFormat("{0} {1}\r\n" + m_Indent, DateTime.Now.ToString("[~HH:mm:ss.fff]"), name);
            IncrementIndentation();
            m_StringBuilder.Append("{\r\n" + m_Indent);

            return new LogBuilder(m_StringBuilder.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public LogBuilder CloseBlock()
        {
            m_Blocks.RemoveAt(m_CurrentBlockIndex);
            m_CurrentBlockIndex--;
            DecrementIndentation();

            var stringFormat = m_CurrentBlockIndex == -1 ? "\r\n{0}}}\r\n\r\n" : "\r\n{0}}}"; // check if last block
            m_StringBuilder.AppendFormat(stringFormat, m_Indent);

            return new LogBuilder(m_StringBuilder.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        public void AppendObject(object obj)
        {
            var objType = obj.GetType();
            var objName = objType.Name;
            var objFields = objType.GetFields();
            var objProperties = objType.GetProperties();

            for (int i = 0; i < objProperties.Length; i++)
            {
                var property = objProperties[i];
                var propertyName = property.Name;
                var propertyValue = property.GetValue(obj);
                var propertyString = propertyName + ": ";

                if (propertyValue == null)
                {
                    propertyString += "null";
                }
                else if (propertyValue is byte[])
                {
                    var dumpByteArray = DumpByteArray((byte[])propertyValue);
                    propertyString += dumpByteArray;
                }
                else
                {
                    propertyString += propertyValue.ToString();
                }

                if (i == objProperties.Length - 1)
                    m_StringBuilder.Append(propertyString + "\r\n");
                else
                    m_StringBuilder.Append(propertyString + "\r\n" + m_Indent);

                var output = m_StringBuilder.ToString();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return m_StringBuilder.ToString();
        }

        private string DumpByteArray(byte[] bytes)
        {
            var builder = new StringBuilder();

            if (bytes.Length == 0)
            {
                builder.Indent("[]");
                return builder.ToString();
            }

            builder.Append("\r\n" + m_Indent + "[");
            IncrementIndentation();
            for (int i = 0; i < bytes.Length; i++)
            {
                if (i % 32 == 0) // add new lines every 32 bytes
                    builder.Append("\r\n" + m_Indent);
                builder.Append(bytes[i].ToString("X2") + " ");
            }
            DecrementIndentation();
            builder.Append("\r\n" + m_Indent + "]");
            return builder.ToString();
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
