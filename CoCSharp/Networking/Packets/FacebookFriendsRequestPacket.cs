using System.Collections.Generic;

namespace CoCSharp.Networking.Packets
{
    public class FacebookFriendsRequestPacket : IPacket
    {
        public List<string> FacebookIDs;

        public ushort ID { get { return 0x2911; } }

        public void ReadPacket(PacketReader reader)
        {
            FacebookIDs = new List<string>();
            var count = reader.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                string facebookID = reader.ReadString();
                FacebookIDs.Add(facebookID);
            }
        }

        public void WritePacket(PacketWriter writer)
        {
            writer.Write(FacebookIDs.Count);
            for (int i = 0; i < FacebookIDs.Count; i++)
            {
                writer.WriteString(FacebookIDs[i]);
            }
        }
    }
}