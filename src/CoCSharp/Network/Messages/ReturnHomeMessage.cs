namespace CoCSharp.Network.Messages
{
    /// <summary>
    /// Message that is sent by the client to server to tell
    /// it that it wants to return home; the server then should
    /// send an <see cref="OwnHomeDataMessage"/>.
    /// </summary>
    public class ReturnHomeMessage : Message
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReturnHomeMessage"/> class.
        /// </summary>
        public ReturnHomeMessage()
        {
            // Space
        }

        /// <summary>
        /// Unknown long 1.
        /// </summary>
        public long Unknown1;

        /// <summary>
        /// Gets the ID of the <see cref="ReturnHomeMessage"/>.
        /// </summary>
        public override ushort Id { get { return 14101; } }

        /// <summary>
        /// Reads the <see cref="ReturnHomeMessage"/> from the specified <see cref="MessageReader"/>.
        /// </summary>
        /// <param name="reader">
        /// <see cref="MessageReader"/> that will be used to read the <see cref="ReturnHomeMessage"/>.
        /// </param>
        public override void ReadMessage(MessageReader reader)
        {
            Unknown1 = reader.ReadInt64();
        }

        /// <summary>
        /// Writes the <see cref="ReturnHomeMessage"/> to the specified <see cref="MessageWriter"/>.
        /// </summary>
        /// <param name="writer">
        /// <see cref="MessageWriter"/> that will be used to write the <see cref="ReturnHomeMessage"/>.
        /// </param>
        public override void WriteMessage(MessageWriter writer)
        {
            writer.Write(Unknown1);
        }
    }
}
