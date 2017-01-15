using CoCSharp.Network;
using System;

namespace CoCSharp.Logic.Commands
{
    /// <summary>
    /// Command that is sent by the client to server to tell it
    /// that some shield was removed.
    /// </summary>
    public class RemoveShieldCommand : Command
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RemoveShieldCommand"/> class.
        /// </summary>
        public RemoveShieldCommand()
        {
            // Space
        }

        /// <summary>
        /// Gets the ID of the <see cref="RemoveShieldCommand"/>.
        /// </summary>
        public override int Id { get { return 573; } }

        /// <summary>
        /// Unknown byte 1.
        /// </summary>
        public byte Unknown1;
        /// <summary>
        /// Unknown integer 2.
        /// </summary>
        public int Unknown2;

        /// <summary>
        /// Current tick.
        /// </summary>
        public int Tick;

        /// <summary>
        /// Reads the <see cref="RemoveShieldCommand"/> from the specified <see cref="MessageReader"/>.
        /// </summary>
        /// <param name="reader">
        /// <see cref="MessageReader"/> that will be used to read the <see cref="RemoveShieldCommand"/>.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="reader"/> is null.</exception>
        public override void ReadCommand(MessageReader reader)
        {
            ThrowIfReaderNull(reader);

            Unknown1 = reader.ReadByte();
            Unknown2 = reader.ReadInt32();

            Tick = reader.ReadInt32();
        }

        /// <summary>
        /// Writes the <see cref="RemoveShieldCommand"/> to the specified <see cref="MessageWriter"/>.
        /// </summary>
        /// <param name="writer">
        /// <see cref="MessageWriter"/> that will be used to write the <see cref="RemoveShieldCommand"/>.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="writer"/> is null.</exception>
        public override void WriteCommand(MessageWriter writer)
        {
            ThrowIfWriterNull(writer);

            writer.Write(Unknown1);
            writer.Write(Unknown2);

            writer.Write(Tick);
        }
    }
}
