using System.Text;

namespace CoCSharp.Server.Core
{
    public class SaveWriter
    {
        public SaveWriter()
        {
            _builder = new StringBuilder();
        }

        private StringBuilder _builder;

        public void WriteRaw(string fieldName, string value)
        {
            _builder.AppendLine(fieldName + "=" + value);
        }

        public void Write(string fieldName, string value)
        {
            WriteRaw(fieldName, "\"" + value + "\"");
        }

        public void Write(string fieldName, int value)
        {
            WriteRaw(fieldName, value.ToString());
        }

        public void Write(string fieldName, long value)
        {
            WriteRaw(fieldName, value.ToString());
        }

        public void Write(string fieldName, bool value)
        {
            WriteRaw(fieldName, value.ToString());
        }

        public override string ToString()
        {
            return _builder.ToString();
        }
    }
}
