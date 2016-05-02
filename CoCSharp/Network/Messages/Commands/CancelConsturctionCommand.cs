using CoCSharp.Logic;
using System;

namespace CoCSharp.Network.Messages.Commands
{
    /// <summary>
    /// Command that is sent by the client to the server to tell
    /// it that a construction was cancelled.
    /// </summary>
    public class CancelConsturctionCommand : Command
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CancelConsturctionCommand"/> class.
        /// </summary>
        public CancelConsturctionCommand()
        {
            // Space
        }

        /// <summary>
        /// Gets the ID of the <see cref="BuyDecorationCommand"/>.
        /// </summary>
        public override int ID { get { return 505; } }

        /// <summary>
        /// Game ID of the <see cref="VillageObject"/> whose
        /// construction was cancelled.
        /// </summary>
        public int VillageObjectID;

        /// <summary>
        /// Unknown integer 1.
        /// </summary>
        public int Unknown1;

        /// <summary>
        /// Reads the <see cref="CancelConsturctionCommand"/> from the specified <see cref="MessageReader"/>.
        /// </summary>
        /// <param name="reader">
        /// <see cref="MessageReader"/> that will be used to read the <see cref="CancelConsturctionCommand"/>.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="reader"/> is null.</exception>
        public override void ReadCommand(MessageReader reader)
        {
            ThrowIfReaderNull(reader);

            VillageObjectID = reader.ReadInt32();

            Unknown1 = reader.ReadInt32();
        }

        /// <summary>
        /// Writes the <see cref="CancelConsturctionCommand"/> to the specified <see cref="MessageWriter"/>.
        /// </summary>
        /// <param name="writer">
        /// <see cref="MessageWriter"/> that will be used to write the <see cref="CancelConsturctionCommand"/>.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="writer"/> is null.</exception>
        public override void WriteCommand(MessageWriter writer)
        {
            ThrowIfWriterNull(writer);

            writer.Write(VillageObjectID);

            writer.Write(Unknown1);
        }
    }
}
