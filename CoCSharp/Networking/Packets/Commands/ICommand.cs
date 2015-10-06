namespace CoCSharp.Networking.Packets.Commands
{
    /// <summary>
    /// Represents a command in <see cref="CommandPacket"/>.
    /// </summary>
    public interface ICommand
    {
        /// <summary>
        /// Gets the ID of this command.
        /// </summary>
        int ID { get; }

        /// <summary>
        /// Reads this <see cref="ICommand"/> from a <see cref="PacketReader"/>.
        /// </summary>
        /// <param name="reader">The <see cref="PacketReader"/> to read this <see cref="ICommand"/> from.</param>
        void ReadCommand(PacketReader reader);

        /// <summary>
        /// Writes this <see cref="ICommand"/> to a <see cref="PacketWriter"/>.
        /// </summary>
        /// <param name="writer">The <see cref="PacketWriter"/> to write this <see cref="ICommand"/> to.</param>
        void WriteCommand(PacketWriter writer);
    }
}
