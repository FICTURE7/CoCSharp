using CoCSharp.Network.Cryptography;

namespace CoCSharp.Network
{
    /// <summary>
    /// Represents a processor to process incoming and outgoing messages.
    /// </summary>
    public abstract class MessageProcessor
    {
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
        /// <param name="chiper">Ciphered array of bytes representing a message to process.</param>
        /// <param name="plaintext">Plaintext representation of <paramref name="chiper"/>.</param>
        /// <returns>Resulting <see cref="Message"/>.</returns>
        public abstract Message ProcessIncoming(MessageHeader header, byte[] chiper, ref byte[] plaintext);

        /// <summary>
        /// Processes the specified <see cref="Message"/> and returns
        /// the resulting chippered array of bytes.
        /// </summary>
        /// <param name="message"><see cref="Message"/> to process.</param>
        /// <returns>Resulting chippered array of bytes.</returns>
        public abstract byte[] ProcessOutgoing(Message message);
    }
}
