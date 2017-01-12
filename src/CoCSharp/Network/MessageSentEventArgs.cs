using System;

namespace CoCSharp.Network
{
    /// <summary>
    /// Provides arguments data for message sent event.
    /// </summary>
    public class MessageSentEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MessageSentEventArgs"/> class.
        /// </summary>
        public MessageSentEventArgs()
        {
            // Space
        }

        /// <summary>
        /// Gets or sets the <see cref="Network.Message"/> that has been
        /// sent.
        /// </summary>
        public Message Message { get; set; }
    }
}
