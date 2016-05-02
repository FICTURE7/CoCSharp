namespace CoCSharp.Network.Messages
{
    /// <summary>
    /// Empty message that is sent by the server to the client
    /// after every <see cref="KeepAliveRequestMessage"/> sent by the client.
    /// </summary>
    public class KeepAliveResponseMessage : Message
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="KeepAliveResponseMessage"/> class.
        /// </summary>
        public KeepAliveResponseMessage()
        {
            // Space
        }

        /// <summary>
        /// Gets the ID of the <see cref="KeepAliveResponseMessage"/>.
        /// </summary>
        public override ushort ID { get { return 20108; } }

        /// <summary>
        /// Reads the <see cref="KeepAliveResponseMessage"/> from the specified <see cref="MessageReader"/>.
        /// </summary>
        /// <param name="reader">
        /// <see cref="MessageReader"/> that will be used to read the <see cref="KeepAliveResponseMessage"/>.
        /// </param>
        public override void ReadMessage(MessageReader reader)
        {
            // Space
        }

        /// <summary>
        /// Writes the <see cref="KeepAliveResponseMessage"/> to the specified <see cref="MessageWriter"/>.
        /// </summary>
        /// <param name="writer">
        /// <see cref="MessageWriter"/> that will be used to write the <see cref="KeepAliveResponseMessage"/>.
        /// </param>
        public override void WriteMessage(MessageWriter writer)
        {
            // Space
        }
    }
}
