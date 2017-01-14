namespace CoCSharp.Networking.Packets
{
    public class EndClientTurnPacket : IPacket
    {
        public ushort ID { get { return 0x3716; } }

        public int Subtick;
        public int Checksum;
        public int nCommands;

        public void ReadPacket(PacketReader reader)
        {
            Subtick = reader.ReadInt32();
            Checksum = reader.ReadInt32();
            nCommands = reader.ReadInt32();
        }

        public void WritePacket(PacketWriter writer)
        {
            writer.WriteInt32(Subtick);
            writer.WriteInt32(Checksum);
            writer.WriteInt32(nCommands);
        }
    }
}
