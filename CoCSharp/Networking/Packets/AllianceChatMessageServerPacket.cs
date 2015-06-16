using System;

namespace CoCSharp.Networking.Packets
{
    public class AllianceChatMessageServerPacket : IPacket
    {
        public ushort ID { get { return 0x5EF8; } }

        public int MessageType; // seems = 1 when asking for troops donation, 2 when chat, 4 when promoted
        private int Unknown1; // = 00 00 00 00
        public uint ServerTick;
        private byte Unknown2; // = 3
        public long UserID;
        private long UserID2;
        public string Username;
        private int Unknown3; // = 10 when member, = 11 when elder
        private int Unknown4;
        public int MemberStatus;
        public TimeSpan MessageTime;
        public string Message;

        public void ReadPacket(PacketReader reader)
        {
            MessageType = reader.ReadInt();
            Unknown1 = reader.ReadInt();
            ServerTick = reader.ReadUInt();
            Unknown2 = (byte)reader.ReadByte();

            switch (MessageType) // not sure about this
            {
                case 1:
                    break;

                case 2:
                    UserID = reader.ReadLong();
                    UserID2 = reader.ReadLong();
                    Username = reader.ReadString();
                    Unknown3 = reader.ReadInt();
                    Unknown4 = reader.ReadInt();
                    MemberStatus = reader.ReadInt();
                    MessageTime = TimeSpan.FromSeconds(reader.ReadInt());
                    Message = reader.ReadString();
                    break;

                default:
                    break;
            }
        }

        public void WritePacket(PacketWriter writer)
        {

        }
    }
}
