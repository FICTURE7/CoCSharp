using CoCSharp.Networking;
using CoCSharp.Networking.Packets;
using System;
using System.IO;
using System.Text;

namespace CoCSharp.Logger
{
    public class PacketLogger
    {
        public PacketLogger()
        {
            LogConsole = true;
            LogWriter = new StreamWriter("packets.log");
            LogWriter.AutoFlush = true;
        }

        public PacketLogger(string logName)
        {
            LogConsole = true;
            LogWriter = new StreamWriter(logName);
            LogWriter.AutoFlush = true;
        }

        public bool LogConsole { get; set; }

        private StreamWriter LogWriter { get; set; }

        public void LogPacket(IPacket packet, PacketDirection direction)
        {
            //TODO: Log raw packet data
            var builder = new StringBuilder();
            var prefix = direction == PacketDirection.Server ? "[CLIENT > SERVER] " : "[CLIENT < SERVER] ";

            var packetType = packet.GetType();
            var packetName = packetType.Name;
            var packetFields = packetType.GetFields();

            builder.Append(DateTime.Now.ToString("[~HH:mm:ss.fff] "));
            builder.Append(prefix);
            builder.Append(FormatPacketName(packetName));
            builder.AppendFormat(" 0x{0:X2}", packet.ID);

            if (packet is UnknownPacket)
            {
                var unknownPacket = packet as UnknownPacket;
                builder.AppendFormat(" Length {0} Version {1}", unknownPacket.Length, unknownPacket.Version);
            }

            if (packetFields.Length > 0)
            {
                builder.AppendLine();
                builder.AppendLine("{");

                for (int i = 0; i < packetFields.Length; i++)
                {
                    var field = packetFields[i];
                    var fieldName = field.Name;
                    var fieldValue = field.GetValue(packet);

                    builder.Indent();
                    builder.AppendFormat("{0}: ", fieldName);

                    if (fieldValue == null) builder.Append("null");
                    else if (fieldValue is string) builder.AppendFormat("{0}{1}{2}", "\"", fieldValue, "\"");
                    else if (fieldValue is byte[])
                    {
                        var byteArray = fieldValue as byte[];
                        builder.AppendLine();
                        builder.Append(DumpByteArray(byteArray));
                    }
                    else builder.Append(fieldValue);
                    builder.AppendLine();
                }

                builder.AppendLine("}");
            }
            else builder.AppendLine(" { }"); // no fields
            
            var builderString = builder.ToString();

            LogWriter.WriteLine(builderString);
            if (LogConsole) Console.WriteLine(builderString);
        }

        private static string FormatPacketName(string name)
        {
            var formattedName = name;
            if (name.EndsWith("Packet")) formattedName = name.Remove(name.Length - "Packet".Length);

            for (int i = 1; i < formattedName.Length; i++)
            {
                var chr = formattedName[i];
                if (char.IsUpper(chr))
                {
                    formattedName = formattedName.Insert(i, " ");
                    i++;
                }
            }
            return formattedName;
        }

        private static string DumpByteArray(byte[] bytes)
        {
            var builder = new StringBuilder();

            if (bytes.Length == 0) return "[]"; // empty array

            builder.Indent("[");
            for (int i = 0; i < bytes.Length; i++)
            {
                if (i % 32 == 0) // add new lines every 32 bytes
                {
                    builder.AppendLine();
                    builder.Indent(2);
                }
                builder.Append(bytes[i].ToString("X2") + " ");
            }
            builder.AppendLine();
            builder.Indent("]");
            return builder.ToString();
        }
    }
}
