namespace CoCSharp.Networking.Packets
{
    public class BindFacebookAccountPacket : IPacket
    {
        public ushort ID { get { return 0x3779; } }

        public bool Unknown1;
        public string NumericFacebookID;
        public string Base64FacebookToken;

        public void ReadPacket(PacketReader reader)
        {
            Unknown1 = reader.ReadBoolean();
            NumericFacebookID = reader.ReadString();
            Base64FacebookToken = reader.ReadString();
        }

        public void WritePacket(PacketWriter writer)
        {
            writer.WriteBoolean(Unknown1);
            writer.WriteString(NumericFacebookID);
            writer.WriteString(Base64FacebookToken);
        }
    }
}
