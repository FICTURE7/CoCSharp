using CoCSharp.Network;
using System;
using System.Text;
using System.Threading;

namespace CoCSharp.Proxy
{
    public class MessageLogger
    {
        public MessageLogger()
        {
            _indent = 0;
            _mapper = new MessageMapper();
        }

        private volatile int _indent;
        private readonly MessageMapper _mapper;

        public void Log(Message message)
        {
            var map = _mapper.Map(message);
            var header = $"{message.GetType().Name} - {message.ID}: ";
            var log = LogRecursive(map, message);

            if (map.Length == 0)
                log = header + log;
            else
                log = header + Environment.NewLine + log;

            Console.WriteLine(log);
        }

        private string LogRecursive(FieldMap[] map, object message)
        {
            var builder = new StringBuilder();
            if (map.Length == 0)
            {
                builder.Append("{ }");
            }
            else
            {
                builder.Append(GetIndent()).AppendLine("{");

                Interlocked.Increment(ref _indent);

                for (int i = 0; i < map.Length; i++)
                {
                    var field = map[i];
                    var value = field.Field.GetValue(message);

                    var valueTxt = (string)null;
                    if (value == null)
                    {
                        valueTxt = "null";
                    }
                    else if (value is string)
                    {
                        valueTxt = "\"" + value + "\"";
                    }
                    else
                    {
                        valueTxt = value.ToString();
                    }

                    builder.Append(GetIndent()).Append(field.Name).Append(": ").Append(valueTxt).AppendLine();

                    if (field.Child != null && value != null)
                        builder.Append(LogRecursive(field.Child, value));
                }

                Interlocked.Decrement(ref _indent);

                builder.Append(GetIndent()).AppendLine("}");
            }

            return builder.ToString();
        }

        private string GetIndent()
        {
            return new string(' ', 4 * _indent);
        }
    }
}
