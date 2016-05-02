using CoCSharp.Logic;
using System;

namespace CoCSharp.Network.Messages.Commands
{
    /// <summary>
    /// Command that is sent by the client to the server to tell
    /// it that a decoration was bought.
    /// </summary>
    public class BuyDecorationCommand : Command
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BuyDecorationCommand"/> class.
        /// </summary>
        public BuyDecorationCommand()
        {
            // Space
        }

        /// <summary>
        /// Gets the ID of the <see cref="BuyDecorationCommand"/>.
        /// </summary>
        public override int ID { get { return 512; } }

        /// <summary>
        /// X coordinates of the <see cref="Decoration"/> where it was placed.
        /// </summary>
        public int X;
        /// <summary>
        /// Y coordinates of the <see cref="Decoration"/> where it was placed.
        /// </summary>
        public int Y;
        /// <summary>
        /// Data ID of the <see cref="Decoration"/> that was bought.
        /// </summary>
        public int DecorationDataID;

        /// <summary>
        /// Unknown integer 1.
        /// </summary>
        public int Unknown1;

        /// <summary>
        /// Reads the <see cref="BuyDecorationCommand"/> from the specified <see cref="MessageReader"/>.
        /// </summary>
        /// <param name="reader">
        /// <see cref="MessageReader"/> that will be used to read the <see cref="BuyDecorationCommand"/>.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="reader"/> is null.</exception>
        public override void ReadCommand(MessageReader reader)
        {
            ThrowIfReaderNull(reader);

            X = reader.ReadInt32();
            Y = reader.ReadInt32();
            DecorationDataID = reader.ReadInt32();

            Unknown1 = reader.ReadInt32();
        }

        /// <summary>
        /// Writes the <see cref="BuyDecorationCommand"/> to the specified <see cref="MessageWriter"/>.
        /// </summary>
        /// <param name="writer">
        /// <see cref="MessageWriter"/> that will be used to write the <see cref="BuyDecorationCommand"/>.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="writer"/> is null.</exception>
        public override void WriteCommand(MessageWriter writer)
        {
            ThrowIfWriterNull(writer);

            writer.Write(X);
            writer.Write(Y);
            writer.Write(DecorationDataID);

            writer.Write(Unknown1);
        }
    }
}
