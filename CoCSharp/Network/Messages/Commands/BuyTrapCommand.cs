using CoCSharp.Logic;
using System;

namespace CoCSharp.Network.Messages.Commands
{
    /// <summary>
    /// Command that is sent by the client to the server to tell
    /// it that a trap was bought.
    /// </summary>
    public class BuyTrapCommand : Command
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BuyTrapCommand"/> class.
        /// </summary>
        public BuyTrapCommand()
        {
            // Space
        }

        /// <summary>
        /// Gets the ID of the <see cref="BuyTrapCommand"/>.
        /// </summary>
        public override int ID { get { return 510; } }

        /// <summary>
        /// X coordinates of the <see cref="Trap"/> where it was placed.
        /// </summary>
        public int X;
        /// <summary>
        /// Y coordinates of the <see cref="Trap"/> where it was placed.
        /// </summary>
        public int Y;
        /// <summary>
        /// Data ID of the <see cref="Trap"/> that was bought.
        /// </summary>
        public int TrapDataID;

        /// <summary>
        /// Unknown integer 1.
        /// </summary>
        public int Unknown1;

        /// <summary>
        /// Reads the <see cref="BuyTrapCommand"/> from the specified <see cref="MessageReader"/>.
        /// </summary>
        /// <param name="reader">
        /// <see cref="MessageReader"/> that will be used to read the <see cref="BuyTrapCommand"/>.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="reader"/> is null.</exception>
        public override void ReadCommand(MessageReader reader)
        {
            ThrowIfReaderNull(reader);

            X = reader.ReadInt32();
            Y = reader.ReadInt32();
            TrapDataID = reader.ReadInt32();

            Unknown1 = reader.ReadInt32();
        }

        /// <summary>
        /// Writes the <see cref="BuyTrapCommand"/> to the specified <see cref="MessageWriter"/>.
        /// </summary>
        /// <param name="writer">
        /// <see cref="MessageWriter"/> that will be used to write the <see cref="BuyTrapCommand"/>.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="writer"/> is null.</exception>
        public override void WriteCommand(MessageWriter writer)
        {
            ThrowIfWriterNull(writer);

            writer.Write(X);
            writer.Write(Y);
            writer.Write(TrapDataID);

            writer.Write(Unknown1);
        }
    }
}
