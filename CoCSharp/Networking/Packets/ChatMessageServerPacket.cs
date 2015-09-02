namespace CoCSharp.Networking.Packets
{
    public class ChatMessageServerPacket : IPacket
    {
        public ushort ID { get { return 0x608B; } }

        public string Message;
        public string Username;
        public int Level;
        public int League;
        public long UserID;
        public long UserID2;
        public bool HasClan;
        public long ClanID;
        public string ClanName;
        public int Unknown3;

        public void ReadPacket(PacketReader reader)
        {
            Message = reader.ReadString();
            Username = reader.ReadString();

            Level = reader.ReadInt32();
            League = reader.ReadInt32();

            UserID = reader.ReadInt64();
            UserID2 = reader.ReadInt64();
            HasClan = reader.ReadBoolean();
            if (HasClan)
            {
                ClanID = reader.ReadInt64();
                ClanName = reader.ReadString();
                Unknown3 = reader.ReadInt32();
            }
        }

        public void WritePacket(PacketWriter writer)
        {
            writer.WriteString(Message);
            writer.WriteString(Username);

            writer.WriteInt32(Level);
            writer.WriteInt32(League);

            writer.WriteInt64(UserID);
            writer.WriteInt64(UserID2);
            writer.WriteBoolean(HasClan);
            if (HasClan)
            {
                writer.WriteInt64(ClanID);
                writer.WriteString(ClanName);
                writer.WriteInt32(Unknown3);
            }
        }
    }
}
