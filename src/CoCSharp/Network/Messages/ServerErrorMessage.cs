namespace CoCSharp.Network.Messages
{
    /// <summary>
    /// Message that is sent by the server to the client to tell
    /// it an error has occurred.
    /// </summary>
    public class ServerErrorMessage : Message
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ServerErrorMessage"/> class.
        /// </summary>
        public ServerErrorMessage()
        {
            // Space
        }

        /// <summary>
        /// Gets the ID of the <see cref="ServerErrorMessage"/>.
        /// </summary>
        public override ushort Id { get { return 24115; } }

        /// <summary>
        /// Error message.
        /// </summary>
        public string ErrorMessage;

        /// <summary>
        /// Reads the <see cref="ServerErrorMessage"/> from the specified <see cref="MessageReader"/>.
        /// </summary>
        /// <param name="reader">
        /// <see cref="MessageReader"/> that will be used to read the <see cref="ServerErrorMessage"/>.
        /// </param>
        public override void ReadMessage(MessageReader reader)
        {
            ThrowIfReaderNull(reader);

            ErrorMessage = reader.ReadString();
        }

        /// <summary>
        /// Writes the <see cref="ServerErrorMessage"/> to the specified <see cref="MessageWriter"/>.
        /// </summary>
        /// <param name="writer">
        /// <see cref="MessageWriter"/> that will be used to write the <see cref="ServerErrorMessage"/>.
        /// </param>
        public override void WriteMessage(MessageWriter writer)
        {
            ThrowIfWriterNull(writer);

            writer.Write(ErrorMessage);
        }
    }
}
