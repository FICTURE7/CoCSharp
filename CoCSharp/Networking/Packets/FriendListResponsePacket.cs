using System.Collections.Generic;

namespace CoCSharp.Networking.Packets
{
    public class FriendListResponsePacket : IPacket
    {
        public ushort ID { get { return 0x4E89; } }
        public int Unknown1;
        public List<Friend> Friends;

        public void ReadPacket(PacketReader reader)
        {
            Unknown1 = reader.ReadInt32();
            var count = reader.ReadInt32();
            Friends = new List<Friend>();
            for (int i = 0; i < count; i++)
            {
                Friend friend = new Friend();
                friend.UserID1 = reader.ReadInt64();
                friend.UserID2 = reader.ReadInt64();
                friend.Username = reader.ReadString();
                friend.FacebookID = reader.ReadString();
                friend.Unknown3 = reader.ReadString();
                friend.Unknown4 = reader.ReadInt32();
                friend.Level = reader.ReadInt32();
                friend.Unknown6 = reader.ReadInt32();
                friend.Trophies = reader.ReadInt32();
                friend.HasClan = reader.ReadBoolean();
                if (friend.HasClan)
                {
                    friend.ClanID = reader.ReadInt64();
                    friend.ClanUnknown1 = reader.ReadInt32();
                    friend.ClanName = reader.ReadString();
                    friend.ClanRole = reader.ReadInt32();
                    friend.ClanLevel = reader.ReadInt32();
                }
                Friends.Add(friend);
            }
        }

        public void WritePacket(PacketWriter writer)
        {
            writer.WriteInt32(Unknown1);
            writer.WriteInt32(Friends.Count);
            for (int i = 0; i < Friends.Count; i++)
            {
                var friend = Friends[i];
                writer.WriteInt64(friend.UserID1);
                writer.WriteInt64(friend.UserID2);
                writer.WriteString(friend.Username);
                writer.WriteString(friend.FacebookID);
                writer.WriteString(friend.Unknown3);
                writer.WriteInt32(friend.Unknown4);
                writer.WriteInt32(friend.Level);
                writer.WriteInt32(friend.Unknown6);
                writer.WriteInt32(friend.Trophies);
                writer.WriteBoolean(friend.HasClan);
                if (friend.HasClan)
                {
                    writer.WriteInt64(friend.ClanID);
                    writer.WriteInt32(friend.ClanUnknown1);
                    writer.WriteString(friend.ClanName); 
                    writer.WriteInt32(friend.ClanRole);
                    writer.WriteInt32(friend.ClanLevel);
                }
            }
        }

        public class Friend
        {
            public long UserID1 { get; set; } 
            public long UserID2 { get; set; } //seems to be the same like UserID1
            public string Username { get; set; }
            public string FacebookID { get; set; }
            public string Unknown3 { get; set; }
            public int Unknown4 { get; set; }
            public int Level { get; set; }
            public int Unknown6 { get; set; }
            public int Trophies { get; set; }
            public bool HasClan { get; set; }
            public long ClanID { get; set; }
            public int ClanUnknown1 { get; set; } //same as Unknown1 from Alliance Info Response
            public string ClanName { get; set; }
            public int ClanRole { get; set; } //1-> Member, 2-> Leader, 3-> Elder, 4 ->Co-Leader
            public int ClanLevel { get; set; }
        }
    }
}