using System;

namespace CoCSharp.Network.Messages
{
    /// <summary>
    /// Message that is sent by the server to the client after 
    /// the client sends a <see cref="SessionRequestMessage"/>.
    /// </summary>
    public class SessionSuccessMessage : Message
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SessionSuccessMessage"/> class.
        /// </summary>
        public SessionSuccessMessage()
        {
            // Space
        }

        /// <summary>
        /// Session key.
        /// </summary>
        public byte[] SessionKey;

        /// <summary>
        /// Gets the ID of the <see cref="SessionSuccessMessage"/>.
        /// </summary>
        public override ushort ID { get { return 20100; } }

        /// <summary>
        /// Reads the <see cref="SessionSuccessMessage"/> from the specified <see cref="MessageReader"/>.
        /// </summary>
        /// <param name="reader">
        /// <see cref="MessageReader"/> that will be used to read the <see cref="SessionSuccessMessage"/>.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="reader"/> is null.</exception>
        public override void ReadMessage(MessageReader reader)
        {
            ThrowIfReaderNull(reader);

            SessionKey = reader.ReadBytes();
        }

        /// <summary>
        /// Writes the <see cref="SessionSuccessMessage"/> to the specified <see cref="MessageWriter"/>.
        /// </summary>
        /// <param name="writer">
        /// <see cref="MessageWriter"/> that will be used to write the <see cref="SessionSuccessMessage"/>.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="writer"/> is null.</exception>
        public override void WriteMessage(MessageWriter writer)
        {
            ThrowIfWriterNull(writer);

            writer.Write(SessionKey, true); // the byte array is prefixed
        }
    }
}
