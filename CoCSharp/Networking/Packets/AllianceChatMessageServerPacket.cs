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
        public MemberStatusId MemberStatus;
        public TimeSpan MessageTime;
        public string Message;

        public enum MemberStatusId
        {
            Elder = 1,
            CoLeader = 4
        };

        public void ReadPacket(PacketReader reader)
        {
            MessageType = reader.ReadInt32();
            Unknown1 = reader.ReadInt32();
            ServerTick = reader.ReadUInt32();
            Unknown2 = (byte)reader.ReadByte();

            switch (MessageType) // not sure about this
            {
                case 1:
                    break;

                case 2:
                    UserID = reader.ReadInt64();
                    UserID2 = reader.ReadInt64();
                    Username = reader.ReadString();
                    Unknown3 = reader.ReadInt32();
                    Unknown4 = reader.ReadInt32();
                    MemberStatus = (MemberStatusId)reader.ReadInt32();
                    MessageTime = TimeSpan.FromSeconds(reader.ReadInt32());
                    Message = reader.ReadString();
                    break;

                default:
                    break;
            }
        }

        public void WritePacket(PacketWriter writer)
        {
            writer.WriteInt32(MessageType);
            writer.WriteInt32(Unknown1);
            writer.WriteUInt32(ServerTick);
            writer.WriteByte(Unknown2);

            switch (MessageType)
            {
                case 1:
                    break;

                case 2:
                    writer.WriteInt64(UserID);
                    writer.WriteInt64(UserID2);
                    writer.WriteString(Username);
                    writer.WriteInt32(Unknown3);
                    writer.WriteInt32(Unknown4);
                    writer.WriteInt32((int)MemberStatus);
                    writer.WriteInt32((int)MessageTime.TotalSeconds);
                    writer.WriteString(Message);
                    break;

                default:
                    break;
            }
        }
    }
}
