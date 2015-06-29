using System.IO;
namespace CoCSharp.Networking.Packets
{
    public class ChatMessageServerPacket : IPacket
    {
        public ushort ID { get { return 0x608B; } }

        public string Message;
        public string Username;
        private int Unknown1;
        private int Unknown2;
        public long UserID;
        private long UserID2;
        public bool HasClan;
        public long ClanID;
        public string ClanName;
        private int Unknown3;

        public void ReadPacket(PacketReader reader)
        {
            Message = reader.ReadString();
            Username = reader.ReadString();

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

        public void WritePacket(PacketWriter writer)
        {
            writer.WriteString(Message);
            writer.WriteString(Username);

            writer.WriteInt(Unknown1);
            writer.WriteInt(Unknown2);

            writer.WriteLong(UserID);
            writer.WriteLong(UserID2);
            writer.WriteBool(HasClan);
            if (HasClan)
            {
                writer.WriteLong(ClanID);
                writer.WriteString(ClanName);
                writer.WriteInt(Unknown3);
            }
        }
    }
}
