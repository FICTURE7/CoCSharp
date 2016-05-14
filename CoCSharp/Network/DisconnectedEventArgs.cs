using System;
using System.Net.Sockets;

namespace CoCSharp.Network
{
    /// <summary>
    /// Event argument when the <see cref="NetworkManagerAsync"/> got disconnected.
    /// </summary>
    public class DisconnectedEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DisconnectedEventArgs"/> class.
        /// </summary>
        public DisconnectedEventArgs()
        {
            // Space
        }

        /// <summary>
        /// Gets or sets the <see cref="SocketError"/> that caused the disconnection.
        /// </summary>
        public SocketError Error { get; set; }
    }
}
