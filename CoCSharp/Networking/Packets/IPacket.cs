namespace CoCSharp.Networking.Packets
{
    /// <summary>
    /// Represents a Clash of Clans message or packet.
    /// </summary>
    public interface IPacket // IMessage
    {
        /// <summary>
        /// Gets the ID of the packet.
        /// </summary>
        ushort ID { get; }

        /// <summary>
        /// Reads this <see cref="IPacket"/> from a <see cref="PacketReader"/>.
        /// </summary>
        /// <param name="reader">The <see cref="PacketReader"/> to read this <see cref="IPacket"/> from.</param>
        void ReadPacket(PacketReader reader);

        /// <summary>
        /// Writes this <see cref="IPacket"/> to a <see cref="PacketWriter"/>.
        /// </summary>
        /// <param name="writer">The <see cref="PacketWriter"/> to write this <see cref="IPacket"/> to.</param>
        void WritePacket(PacketWriter writer);
    }
}
