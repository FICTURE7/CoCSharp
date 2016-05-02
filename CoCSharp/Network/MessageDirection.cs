using System;

namespace CoCSharp.Network
{
    /// <summary>
    /// Defines the direction of a <see cref="Message"/>.
    /// </summary>
    [Flags]
    public enum MessageDirection
    {
        /// <summary>
        /// <see cref="Message"/> is going to the server.
        /// </summary>
        Server = 0,

        /// <summary>
        /// <see cref="Message"/> is going to the client.
        /// </summary>
        Client = 2
    }
}
