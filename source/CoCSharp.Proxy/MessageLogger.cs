using CoCSharp.Network;
using CoCSharp.Proxy;
using System;
using System.Reflection;
using System.Text;

namespace CoCSharp.Server
{
    public class MessageLogger
    {
        public MessageLogger()
        {
            // Space
        }

        public void Log(Message message)
        {
            var builder = new StringBuilder();
            var type = message.GetType();
            var fields = type.GetFields();

            builder.AppendLine("&(gray)" + type.Name + "&(default) - " + message.ID + ":");

            for (int i = 0; i < fields.Length; i++)
            {
                var fieldName = GetFieldName(fields[i]);
                var fieldValue = GetFieldValue(fields[i], message);

                builder.AppendFormat("    {0}&(darkgray):&(default) {1}", fieldName, fieldValue);
                builder.AppendLine();
            }

            FancyConsole.WriteLine(builder);
        }

        private string GetFieldName(FieldInfo field)
        {
            var color = "&(blue)";
            var fieldName = field.Name;

            if (field.Name.Length > "Unknown".Length && fieldName.Substring(0, "Unknown".Length) == "Unknown")
                color = "&(darkblue)";

            return color + field.Name + "&(default)";
        }

        private string GetFieldValue(FieldInfo field, Message message)
        {
            var value = string.Empty;
            var fieldType = field.FieldType;
            var fieldValue = field.GetValue(message);

            if (fieldValue == null)
                return "&(cyan)null&(default)";
            else if (fieldType == typeof(byte) ||
                    fieldType == typeof(sbyte) ||
                    fieldType == typeof(short) ||
                    fieldType == typeof(ushort) ||
                    fieldType == typeof(int) ||
                    fieldType == typeof(uint) ||
                    fieldType == typeof(long) ||
                    fieldType == typeof(ulong) ||
                    fieldType == typeof(float) ||
                    fieldType == typeof(double) ||
                    fieldType == typeof(decimal) ||
                    fieldType == typeof(TimeSpan) ||
                    fieldType == typeof(DateTime))
                return "&(green)" + fieldValue + "&(default)";
            else if (fieldType == typeof(string))
                return "&(yellow)\"" + fieldValue + "\"&(default)";

            return fieldValue.ToString();
        }
    }
}
