using System;

namespace CoCSharp.Network.Messages
{
    /// <summary>
    /// Message that is sent by the client to the server to
    /// to tell it has a visited a village.
    /// </summary>
    public class VisitHomeMessage : Message
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VisitHomeMessage"/> class.
        /// </summary>
        public VisitHomeMessage()
        {
            // Space
        }

        /// <summary>
        /// Gets the ID of the <see cref="VisitHomeMessage"/>.
        /// </summary>
        public override ushort Id { get { return 14113; } }

        /// <summary>
        /// ID of home the avatar has visited.
        /// </summary>
        public long HomeId;

        /// <summary>
        /// Unknown integer 1.
        /// </summary>
        public int Unknown1;

        /// <summary>
        /// Reads the <see cref="VisitHomeMessage"/> from the specified <see cref="MessageReader"/>.
        /// </summary>
        /// <param name="reader">
        /// <see cref="MessageReader"/> that will be used to read the <see cref="VisitHomeMessage"/>.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="reader"/> is null.</exception>
        public override void ReadMessage(MessageReader reader)
        {
            ThrowIfReaderNull(reader);

            HomeId = reader.ReadInt64();

            Unknown1 = reader.ReadInt32();
        }

        /// <summary>
        /// Writes the <see cref="VisitHomeMessage"/> to the specified <see cref="MessageWriter"/>.
        /// </summary>
        /// <param name="writer">
        /// <see cref="MessageWriter"/> that will be used to write the <see cref="VisitHomeMessage"/>.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="writer"/> is null.</exception>
        public override void WriteMessage(MessageWriter writer)
        {
            ThrowIfWriterNull(writer);

            writer.Write(HomeId);

            writer.Write(Unknown1);
        }
    }
}
