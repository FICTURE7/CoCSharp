namespace CoCSharp.Networking.Packets
{
    public class ChatMessageServerPacket : IPacket
    {
        public ushort ID { get { return 0x608B; } }

        public string Message;
        public string UserName;

        public int Unknown1; // keeping unknowns cause we liek data
        public int Unknown2;

        public long UserID;
        public long UserID2;
        public bool HasClan;
        public long ClanID;
        public string ClanName;

        public int Unknown3;

        public void ReadPacket(PacketReader reader)
        {
            Message = reader.ReadString();
            UserName = reader.ReadString();
            Unknown1 = reader.ReadInt();
            Unknown2 = reader.ReadInt();
            UserID = reader.ReadLong();
            UserID2 = reader.ReadLong();
            HasClan = reader.ReadBool();
            if (HasClan)
            {
                ClanID = reader.ReadLong();
                ClanName = reader.ReadString();
                Unknown3 = reader.ReadInt();
            }
        }

        public void WritePacket(CoCStream stream)
        {

        }
    }
}
