using System;

namespace CoCSharp.Network.Messages
{
    /// <summary>
    /// Message that is sent by the server to the client to update the ciphers.
    /// </summary>
    [Obsolete("EncryptionMessage is not used in the latest 8.x.x protocol.")]
    public class EncryptionMessage : Message
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EncryptionMessage"/> class.
        /// </summary>
        public EncryptionMessage()
        {
            // Space
        }

        /// <summary>
        /// Gets the ID of the <see cref="EncryptionMessage"/>.
        /// </summary>
        public override ushort ID { get { return 20000; } }

        /// <summary>
        /// Byte array of the server random.
        /// </summary>
        public byte[] ServerRandom;        
        /// <summary>
        /// Version number of the scrambler. Only version 1 is supported.
        /// </summary>
        public int ScramblerVersion;

        /// <summary>
        /// Reads the <see cref="EncryptionMessage"/> from the specified <see cref="MessageReader"/>.
        /// </summary>
        /// <param name="reader">
        /// <see cref="MessageReader"/> that will be used to read the <see cref="EncryptionMessage"/>.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="reader"/> is null.</exception>
        public override void ReadMessage(MessageReader reader)
        {
            ThrowIfReaderNull(reader);

            ServerRandom = reader.ReadBytes();
            ScramblerVersion = reader.ReadInt32();
        }

        /// <summary>
        /// Writes the <see cref="EncryptionMessage"/> to the specified <see cref="MessageWriter"/>.
        /// </summary>
        /// <param name="writer">
        /// <see cref="MessageWriter"/> that will be used to write the <see cref="EncryptionMessage"/>.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="writer"/> is null.</exception>
        public override void WriteMessage(MessageWriter writer)
        {
            ThrowIfWriterNull(writer);

            writer.Write(ServerRandom, true);
            writer.Write(ScramblerVersion);
        }
    }
}
