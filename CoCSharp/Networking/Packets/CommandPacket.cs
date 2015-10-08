using CoCSharp.Networking.Packets.Commands;

namespace CoCSharp.Networking.Packets
{
    public class CommandPacket : IPacket
    {
        public ushort ID { get { return 0x3716; } }

        public int Subtick;
        public int Checksum;
        public ICommand[] Commands;

        public void ReadPacket(PacketReader reader)
        {
            Subtick = reader.ReadInt32();
            Checksum = reader.ReadInt32();
            Commands = new ICommand[reader.ReadInt32()];

            for (int i = 0; i < Commands.Length; i++)
            {
                var command = (ICommand)null;
                var id = reader.ReadInt32();
                if (!CommandFactory.TryCreate(id, out command))
                    break; // break because we dont want to mess the stream and causes exceptions
                command.ReadCommand(reader);
                Commands[i] = command;
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
