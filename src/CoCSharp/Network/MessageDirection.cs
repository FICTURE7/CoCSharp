using System;

namespace CoCSharp.Network
{
    /// <summary>
    /// Defines the direction of a <see cref="Message"/>.
    /// </summary>
    [Flags]
    public enum MessageDirection : byte
    {
        /// <summary>
        /// <see cref="Message"/> is going to the client.
        /// </summary>
        Client = 2,

        /// <summary>
        /// <see cref="Message"/> is going to the server.
        /// </summary>
        Server = 3
    }
}
