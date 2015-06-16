namespace CoCSharp.Networking.Packets
{
    public interface IPacket // IMessage
    {
        ushort ID { get; }

        void ReadPacket(PacketReader reader);

        void WritePacket(PacketWriter writer);
    }
}
