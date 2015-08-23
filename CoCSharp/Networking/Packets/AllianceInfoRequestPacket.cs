namespace CoCSharp.Networking.Packets
{
    public class AllianceInfoRequestPacket : IPacket
    {
        public ushort ID { get { return 0x37DE; } }

        public long ClanID;

        public void ReadPacket(PacketReader reader)
        {
            ClanID = reader.ReadInt64();
        }

        public void WritePacket(PacketWriter writer)
        {
            writer.WriteInt64(ClanID);
        }
    }
}
