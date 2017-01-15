using CoCSharp.Logic;
using CoCSharp.Network;
using System;
using System.Collections;
using System.IO;
using System.Text;

namespace CoCSharp.Proxy
{
    public class MessageLogger
    {
        public MessageLogger()
        {
            _sync = new object();
            _cmdMapper = new CommandMapper();
            _msgMapper = new MessageMapper();
        }

        private object _sync;
        private readonly CommandMapper _cmdMapper;
        private readonly MessageMapper _msgMapper;

        public void Log(Message message)
        {
            // Get the field mapping of the message.
            var map = _msgMapper.Map(message);
            var header = $"{message.GetType().Name} - {message.Id}: ";

            // Start crafting the log with the map.
            var log = LogRecursive(map, message, 0);

            // No need to add the logs after a new line
            // if its body is empty.
            if (map.Length == 0)
            {
                log = header + log;
            }
            else
            {
                log = header + Environment.NewLine + log;
            }

            lock (_sync)
            {
                Console.WriteLine(log);

                var file = new FileStream("message-log.log", FileMode.Append);
                using (var writer = new StreamWriter(file))
                    writer.WriteLine(log);
            }
        }

        private string LogRecursive(FieldMap[] map, object obj, int indent)
        {
            var builder = new StringBuilder();

            // If the log does not have a body,
            // no need to add a new line.
            if (map.Length == 0)
            {
                builder.Append("{ }");
            }
            else
            {
                builder.Append(GetIndent(indent)).AppendLine("{");
                indent++;

                for (int i = 0; i < map.Length; i++)
                {
                    var field = map[i];
                    // Get the value of the field on obj.
                    var value = field.Field.GetValue(obj);
                    // Get it string representation.
                    var valueTxt = GetValue(value, indent);

                    builder.Append(GetIndent(indent)).Append(field.Name).Append(": ");
                    if (value is Command || field.Field.FieldType.BaseType == typeof(MessageComponent))
                        builder.AppendLine().Append(valueTxt);
                    else
                        builder.Append(valueTxt);

                    builder.AppendLine();
                }

                indent--;
                builder.Append(GetIndent(indent)).Append("}");
            }

            return builder.ToString();
        }

        private string GetValue(object value, int indent)
        {
            var valueTxt = (string)null;

            if (value == null)
            {
                valueTxt = "null";
            }
            else if (value is string)
            {
                valueTxt = "\"" + value + "\"";
            }
            else if (value is MessageComponent)
            {
                var component = (MessageComponent)value;
                var map = _msgMapper.Map(component);

                valueTxt = LogRecursive(map, component, indent);
            }
            else if (value is Command)
            {
                var cmd = (Command)value;
                var cmdMap = _cmdMapper.Map(cmd);
                var header = $"{cmd.GetType().Name} - {cmd.Id}: {Environment.NewLine}";
                var log = header + LogRecursive(cmdMap, cmd, indent);

                valueTxt = log;
            }
            else if (value is ICollection)
            {
                var collectionBuilder = new StringBuilder();

                var valueCollection = (ICollection)value;
                if (valueCollection.Count == 0)
                {
                    collectionBuilder.Append("[ ]");
                }
                else
                {
                    collectionBuilder.AppendLine().Append(GetIndent(indent)).AppendLine("[");
                    indent++;

                    foreach (var k in valueCollection)
                    {
                        var vvalue = GetValue(k, indent);
                        collectionBuilder.Append(GetIndent(indent)).AppendLine(vvalue);
                    }

                    indent--;
                    collectionBuilder.Append(GetIndent(indent)).Append("]");
                }

                valueTxt = collectionBuilder.ToString();
            }
            else
            {
                valueTxt = value.ToString();
            }

            return valueTxt;
        }

        private string GetIndent(int indent)
        {
            return new string(' ', 4 * indent);
        }
    }
}
