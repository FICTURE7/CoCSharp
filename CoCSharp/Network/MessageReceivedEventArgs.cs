using System;

namespace CoCSharp.Network
{
    /// <summary>
    /// Provides arguments data for message received event.
    /// </summary>
    public class MessageReceivedEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MessageReceivedEventArgs"/> class.
        /// </summary>
        public MessageReceivedEventArgs()
        {
            // Space
        }

        /// <summary>
        /// Gets or sets the <see cref="Network.Message"/> that has been
        /// received.
        /// </summary>
        public Message Message { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="System.Exception"/> that occurred during
        /// the reading of the <see cref="Network.Message"/>. Returns null if no
        /// error occurred during reading.
        /// </summary>
        public Exception Exception { get; set; }

        /// <summary>
        /// Gets or sets whether the <see cref="Message"/> received was fully read.
        /// </summary>
        public bool MessageFullyRead { get; set; } // TODO: Consider renaming to FullyRead.

        /// <summary>
        /// Gets or sets the <see cref="Network.Message"/> bytes body that was decrypted
        /// from the network.
        /// </summary>
        public byte[] MessageBody { get; set; } //TODO: Improve structure.

        /// <summary>
        /// Gets or sets the <see cref="Network.Message"/> raw bytes that was received from
        /// the network. This includes the header and the body encrypted.
        /// </summary>
        public byte[] MessageData { get; set; }
    }
}
