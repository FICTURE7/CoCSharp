using System;

namespace CoCSharp.Network.Messages
{
    /// <summary>
    /// Message that is sent by the client to the server to send a message to the lobby chat. The
    /// server then replies with a <see cref="ChatMessageServerMessage"/>.
    /// </summary>
    public class ChatMessageClientMessage : Message
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChatMessageClientMessage"/> class.
        /// </summary>
        public ChatMessageClientMessage()
        {
            // Space
        }

        /// <summary>
        /// Gets the ID of the <see cref="ChatMessageClientMessage"/>.
        /// </summary>
        public override ushort ID { get { return 14715; } }

        /// <summary>
        /// Message that will be sent to the lobby chat.
        /// </summary>
        public string Message;

        /// <summary>
        /// Reads the <see cref="ChatMessageClientMessage"/> from the specified <see cref="MessageReader"/>.
        /// </summary>
        /// <param name="reader">
        /// <see cref="MessageReader"/> that will be used to read the <see cref="ChatMessageClientMessage"/>.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="reader"/> is null.</exception>
        public override void ReadMessage(MessageReader reader)
        {
            ThrowIfReaderNull(reader);

            Message = reader.ReadString();
        }

        /// <summary>
        /// Writes the <see cref="ChatMessageClientMessage"/> to the specified <see cref="MessageWriter"/>.
        /// </summary>
        /// <param name="writer">
        /// <see cref="MessageWriter"/> that will be used to write the <see cref="ChatMessageClientMessage"/>.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="writer"/> is null.</exception>
        public override void WriteMessage(MessageWriter writer)
        {
            ThrowIfWriterNull(writer);

            writer.Write(Message);
        }
    }
}
