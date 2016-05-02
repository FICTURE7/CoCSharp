using CoCSharp.Logic;
using System;

namespace CoCSharp.Network.Messages.Commands
{
    /// <summary>
    /// Command that is sent by the client to the server to tell
    /// it that a trap was rearmed.
    /// </summary>
    public class RearmTrapCommand : Command
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RearmTrapCommand"/> class.
        /// </summary>
        public RearmTrapCommand()
        {
            // Space
        }

        /// <summary>
        /// Gets the ID of the <see cref="RearmTrapCommand"/>.
        /// </summary>
        public override int ID { get { return 545; } }

        /// <summary>
        /// Unknown integer 1.
        /// </summary>
        public int Unknown1;

        /// <summary>
        /// Game ID of the <see cref="Trap"/> that was upgraded.
        /// </summary>
        public int TrapGameID;

        /// <summary>
        /// Unknown integer 2.
        /// </summary>
        public int Unknown2;

        /// <summary>
        /// Reads the <see cref="RearmTrapCommand"/> from the specified <see cref="MessageReader"/>.
        /// </summary>
        /// <param name="reader">
        /// <see cref="MessageReader"/> that will be used to read the <see cref="BuyBuildingCommand"/>.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="reader"/> is null.</exception>
        public override void ReadCommand(MessageReader reader)
        {
            ThrowIfReaderNull(reader);

            Unknown1 = reader.ReadInt32();

            TrapGameID = reader.ReadInt32();

            Unknown2 = reader.ReadInt32(); // Subtick it executed?
        }

        /// <summary>
        /// Writes the <see cref="RearmTrapCommand"/> to the specified <see cref="MessageWriter"/>.
        /// </summary>
        /// <param name="writer">
        /// <see cref="MessageWriter"/> that will be used to write the <see cref="BuyBuildingCommand"/>.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="writer"/> is null.</exception>
        public override void WriteCommand(MessageWriter writer)
        {
            ThrowIfWriterNull(writer);

            writer.Write(Unknown1);

            writer.Write(TrapGameID);

            writer.Write(Unknown2);
        }
    }
}
