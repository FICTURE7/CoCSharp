using System;

namespace CoCSharp.Network.Messages
{

    /// <summary>
    /// Message that is sent to the client after client login
    /// Used In RC4
    /// </summary>
    public class UpdateKeyMessage : Message
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateKeyMessage"/> class.
        /// </summary>
        public UpdateKeyMessage()
        {
            // Space
        }

        /// <summary>
        /// Random Key with lenght of 20.
        /// </summary>
        public byte[] Key;
        /// <summary>
        /// Scrambler Version That Want To Be Used.
        /// </summary>
        public int ScramblerVersion; //1

        /// <summary>
        /// Gets the ID of the <see cref="UpdateKeyMessage"/>.
        /// </summary>
        public override ushort ID { get { return 20000; } }

        /// <summary>
        /// Reads the <see cref="UpdateKeyMessage"/> from the specified <see cref="MessageReader"/>.
        /// </summary>
        /// <param name="reader">
        /// <see cref="MessageReader"/> that will be used to read the <see cref="UpdateKeyMessage"/>.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="reader"/> is null.</exception>
        public override void ReadMessage(MessageReader reader)
        {
            ThrowIfReaderNull(reader);

            Key = reader.ReadBytes(20);
            ScramblerVersion = reader.ReadInt32();
        }

        /// <summary>
        /// Writes the <see cref="UpdateKeyMessage"/> to the specified <see cref="MessageWriter"/>.
        /// </summary>
        /// <param name="writer">
        /// <see cref="MessageWriter"/> that will be used to write the <see cref="UpdateKeyMessage"/>.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="writer"/> is null.</exception>
        public override void WriteMessage(MessageWriter writer)
        {
            ThrowIfWriterNull(writer);
            if (Key == null)
                throw new InvalidOperationException("Key cannot be null.");
            writer.Write(Key);
            writer.Write(ScramblerVersion);
        }
    }
}