using System;
using CoCSharp.Networking.Packets;
using CoCSharp.Networking;

namespace CoCSharp.Logging
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class PacketLog : Log
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PacketLog"/> class
        /// with the specified log name.
        /// </summary>
        /// <param name="name">Name of log. The log name will be used as the <see cref="Log.Path"/>.</param>
        public PacketLog(string name) 
            : base(name)
        {
            // Space        
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PacketLog"/> class
        /// with the specified log name and path.
        /// </summary>
        /// <param name="name">Name of log.</param>
        /// <param name="path">Path of log.</param>
        public PacketLog(string name, string path)
            : base(name, path)
        {
            // Space
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameters"></param>
        public override void LogData(params object[] parameters)
        {
            if (!(parameters[0] is IPacket))
                throw new ArgumentException("parameters[0] must an IPacket type.");
            if (!(parameters[1] is PacketDirection))
                throw new ArgumentException("parameters[1] must a PacketDirection type.");

            var packet = (IPacket)parameters[0];
            var direction = (PacketDirection)parameters[1];

            var blockName = string.Empty;
            var prefix = direction == PacketDirection.Server ? "[CLIENT > SERVER]" :
                                                               "[CLIENT < SERVER]";
            var packetName = packet.GetType().Name;
            blockName = string.Format("{0} {1} 0x{2:X2} ({2})", prefix, FormatPacketName(packetName), packet.ID);

            if (packet is KeepAliveRequestPacket || packet is KeepAliveResponsePacket)
            {
                LogBuilder.EmptyBlock(blockName);
            }
            else
            {
                LogBuilder.OpenBlock(blockName);
                LogBuilder.AppendObject(packet);
                LogBuilder.CloseBlock();
            }
            if (AutoSave)
                Save();
        }

        private static string FormatPacketName(string name)
        {
            var formattedName = name;
            if (name.EndsWith("Packet"))
                formattedName = name.Remove(name.Length - "Packet".Length);

            for (int i = 1; i < formattedName.Length; i++)
            {
                if (char.IsUpper(formattedName[i]))
                {
                    formattedName = formattedName.Insert(i, " ");
                    i++;
                }
            }
            return formattedName;
        }
    }
}
