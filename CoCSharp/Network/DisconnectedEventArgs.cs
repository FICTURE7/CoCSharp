using System;
using System.Net.Sockets;

namespace CoCSharp.Network
{
    /// <summary>
    /// Event argument when <see cref="NetworkManagerAsync.Socket"/> gets disconnected.
    /// </summary>
    public class DisconnectedEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DisconnectedEventArgs"/> class.
        /// </summary>
        public DisconnectedEventArgs(SocketError error)
        {
            Error = error;
        }

        /// <summary>
        /// Gets or sets the <see cref="SocketError"/> that caused the disconnection.
        /// </summary>
        public SocketError Error { get; private set; }
    }
}
