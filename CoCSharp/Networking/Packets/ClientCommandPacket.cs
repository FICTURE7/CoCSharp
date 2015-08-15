using CoCSharp.Networking.Commands;
using System;
using System.Collections.Generic;

namespace CoCSharp.Networking.Packets
{
    public class ClientCommandPacket : IPacket
    {
        public ushort ID { get { return 0x3716; } }

        public int Subtick;
        public int Checksum;
        public ICommand[] Commands;

        private static Dictionary<int, ICommand> CommandDictionary { get; set; }

        public void ReadPacket(PacketReader reader)
        {
            Subtick = reader.ReadInt32();
            Checksum = reader.ReadInt32();
            Commands = new ICommand[reader.ReadInt32()];
            throw new NotImplementedException();

            for (int i = 0; i < Commands.Length; i++)
            {
                var commandID = reader.ReadInt32();
            }
        }

        public void WritePacket(PacketWriter writer)
        {
            writer.WriteInt32(Subtick);
            writer.WriteInt32(Checksum);
            writer.WriteInt32(Commands.Length);
            for (int i = 0; i < Commands.Length; i++)
            {
                writer.WriteInt32(Commands[i].ID);
                Commands[i].WriteCommand(writer);
            }
        }
    }
}
