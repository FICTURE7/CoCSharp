using CoCSharp.Network;
using CoCSharp.Proxy;
using System;
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

            builder.AppendLine("&(gray)" + type.Name + "&(default):");
            for (int i = 0; i < fields.Length; i++)
            {
                var fieldName = "&(darkmagenta)" + fields[i].Name + "&(darkgray)";
                var fieldValue = "&(yellow)" + fields[i].GetValue(message);

                if (fieldName.Contains("Unknown"))
                    fieldName = "&(blue)" + fields[i].Name + "&(darkgray)";

                builder.AppendFormat("    {0}: {1}", fieldName, fieldValue);
                builder.AppendLine();
            }

            FancyConsole.WriteLine(builder);
        }
    }
}
