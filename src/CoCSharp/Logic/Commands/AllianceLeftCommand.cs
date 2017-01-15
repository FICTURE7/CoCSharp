using CoCSharp.Network;
using System;

namespace CoCSharp.Logic.Commands
{
    /// <summary>
    ///  Command that is sent by the server to the client
    /// to tell it that it left its alliance.
    /// </summary>
    public class AllianceLeftCommand : Command
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AllianceLeftCommand"/> class.
        /// </summary>
        public AllianceLeftCommand()
        {
            // Space
        }

        /// <summary>
        /// Gets the ID of the <see cref="AllianceLeftCommand"/>.
        /// </summary>
        public override int Id { get { return 2; } }

        /// <summary>
        /// Clan ID which the client left.
        /// </summary>
        public long ClanId;
        /// <summary>
        /// Reason why the client left.
        /// </summary>
        public int Reason; // TODO: Implement enum for it.

        /// <summary>
        /// Reads the <see cref="AllianceLeftCommand"/> from the specified <see cref="MessageReader"/>.
        /// </summary>
        /// <param name="reader">
        /// <see cref="MessageReader"/> that will be used to read the <see cref="AllianceLeftCommand"/>.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="reader"/> is null.</exception>
        public override void ReadCommand(MessageReader reader)
        {
            ThrowIfReaderNull(reader);

            ClanId = reader.ReadInt64();
            Reason = reader.ReadInt32();
            Tick = reader.ReadInt32();
        }

        /// <summary>
        /// Writes the <see cref="AllianceLeftCommand"/> to the specified <see cref="MessageWriter"/>.
        /// </summary>
        /// <param name="writer">
        /// <see cref="MessageWriter"/> that will be used to write the <see cref="AllianceLeftCommand"/>.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="writer"/> is null.</exception>
        public override void WriteCommand(MessageWriter writer)
        {
            ThrowIfWriterNull(writer);

            writer.Write(ClanId);
            writer.Write(Reason);
            writer.Write(Tick);
        }
    }
}
