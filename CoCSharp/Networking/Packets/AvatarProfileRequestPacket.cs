namespace CoCSharp.Networking.Packets
{
    public class AvatarProfileRequestPacket : IPacket
    {
        public ushort ID { get { return 0x37F5; } }

        public long UserID;
        public long UserID2;
        public byte Unknown1;

        public void ReadPacket(PacketReader reader)
        {
            UserID = reader.ReadInt64();
            UserID2 = reader.ReadInt64();
            Unknown1 = (byte)reader.ReadByte();
        }

        public void WritePacket(PacketWriter writer)
        {
            writer.WriteInt64(UserID);
            writer.WriteInt64(UserID2);
            writer.WriteByte(Unknown1);
        }
    }
}
