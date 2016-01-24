namespace CoCSharp.Networking.Messages
{
    /// <summary>
    /// New message introduced in the latest update. Its is 
    /// sent by the server after the client sends a NewClientEncryptionMessage.
    /// </summary>
    public class NewServerEncryptionMessage : Message
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NewServerEncryptionMessage"/> class.
        /// </summary>
        public NewServerEncryptionMessage()
        {
            // Space
        }

        /// <summary>
        /// Session key used for encryption.
        /// </summary>
        public byte[] SessionKey;

        /// <summary>
        /// Gets the ID of the <see cref="NewServerEncryptionMessage"/>.
        /// </summary>
        public override ushort ID { get { return 20100; } }

        /// <summary>
        /// Reads the <see cref="NewServerEncryptionMessage"/> from the specified <see cref="MessageReader"/>.
        /// </summary>
        /// <param name="reader">
        /// <see cref="MessageReader"/> that will be used to read the <see cref="NewServerEncryptionMessage"/>.
        /// </param>
        public override void ReadMessage(MessageReader reader)
        {
            SessionKey = reader.ReadBytes();
        }

        /// <summary>
        /// Writes the <see cref="NewServerEncryptionMessage"/> to the specified <see cref="MessageWriter"/>.
        /// </summary>
        /// <param name="writer">
        /// <see cref="MessageWriter"/> that will be used to write the <see cref="NewServerEncryptionMessage"/>.
        /// </param>
        public override void WriteMessage(MessageWriter writer)
        {
            writer.Write(SessionKey, true);
        }
    }
}
