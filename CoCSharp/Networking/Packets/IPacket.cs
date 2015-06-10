namespace CoCSharp.Networking.Packets
{
    public interface IPacket
    {
        ushort ID { get; }

        void ReadPacket(PacketReader reader);

        void WritePacket(CoCStream stream); //TODO: replace CoCStream by PacketWriter instead
    }
}
