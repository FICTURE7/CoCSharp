using System.IO;
namespace CoCSharp.Networking.Packets
{
    public class ChatMessageServerPacket : IPacket
    {
        public ushort ID { get { return 0x608B; } }

        public string Message;
        public string Username;

        //public int Unknown1;
        //public int Unknown2;

        public long UserID;
        //public long UserID2;
        public bool HasClan;
        public long ClanID;
        public string ClanName;

        //public int Unknown3;

        public void ReadPacket(PacketReader reader)
        {
            Message = reader.ReadString();
            Username = reader.ReadString();

            //Unknown1 = reader.ReadInt();
            //Unknown2 = reader.ReadInt();

            reader.Seek(8, SeekOrigin.Current);
            UserID = reader.ReadLong();

            //UserID2 = reader.ReadLong();
            reader.Seek(8, SeekOrigin.Current);

            HasClan = reader.ReadBool();
            if (HasClan)
            {
                ClanID = reader.ReadLong();
                ClanName = reader.ReadString();
                //Unknown3 = reader.ReadInt();
            }
        }

        public void WritePacket(PacketWriter writer)
        {

        }
    }
}
