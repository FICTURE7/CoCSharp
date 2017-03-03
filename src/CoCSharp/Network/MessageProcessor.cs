using CoCSharp.Network.Cryptography;

namespace CoCSharp.Network
{
    /// <summary>
    /// Represents a processor to process incoming and outgoing messages.
    /// </summary>
    public abstract class MessageProcessor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MessageProcessor"/> class.
        /// </summary>
        protected MessageProcessor()
        {
            // Space
        }

        /// <summary>
        /// Gets the <see cref="CoCCrypto"/> that is going to decrypt incoming and encrypt outgoing
        /// messages.
        /// </summary>
        public abstract CoCCrypto Crypto { get; }

        /// <summary>
        /// Processes the specified chippered array of bytes and returns
        /// the resulting <see cref="Message"/>.
        /// </summary>
        /// <param name="header">Header of the message.</param>
        /// <param name="stream">Chippered array of bytes representing a message to process.</param>
        /// <param name="raw">Raw array of bytes representing the message.</param>
        /// <param name="plaintext">Plaintext representation of the data read.</param>
        /// <returns>Resulting <see cref="Message"/>.</returns>
        public abstract Message ProcessIncoming(MessageHeader header, BufferStream stream, ref byte[] raw, ref byte[] plaintext);

        /// <summary>
        /// Processes the specified <see cref="Message"/> and returns
        /// the resulting chippered array of bytes.
        /// </summary>
        /// <param name="message"><see cref="Message"/> to process.</param>
        /// <returns>Resulting chippered array of bytes.</returns>
        public abstract byte[] ProcessOutgoing(Message message);
    }
}
