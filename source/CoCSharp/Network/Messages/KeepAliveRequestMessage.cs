namespace CoCSharp.Network.Messages
{
    /// <summary>
    /// Empty message that is sent by the client to the server. The server
    /// must reply with a <see cref="KeepAliveResponseMessage"/>.
    /// </summary>
    public class KeepAliveRequestMessage : Message
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="KeepAliveRequestMessage"/> class.
        /// </summary>
        public KeepAliveRequestMessage()
        {
            // Space
        }

        /// <summary>
        /// Gets the ID of the <see cref="KeepAliveRequestMessage"/>.
        /// </summary>
        public override ushort ID { get { return 10108; } }

        /// <summary>
        /// Reads the <see cref="KeepAliveRequestMessage"/> from the specified <see cref="MessageReader"/>.
        /// </summary>
        /// <param name="reader">
        /// <see cref="MessageReader"/> that will be used to read the <see cref="KeepAliveRequestMessage"/>.
        /// </param>
        public override void ReadMessage(MessageReader reader)
        {
            // Space
        }

        /// <summary>
        /// Writes the <see cref="KeepAliveRequestMessage"/> to the specified <see cref="MessageWriter"/>.
        /// </summary>
        /// <param name="writer">
        /// <see cref="MessageWriter"/> that will be used to write the <see cref="KeepAliveRequestMessage"/>.
        /// </param>
        public override void WriteMessage(MessageWriter writer)
        {
            // Space
        }
    }
}
