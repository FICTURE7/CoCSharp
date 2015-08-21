using System;

namespace CoCSharp.Networking
{
    /// <summary>
    /// Controls the <see cref="PacketBuffer"/> on how it extracts packets bytes.
    /// </summary>
    [Flags]
    public enum PacketExtractionFlags : byte
    {
        /// <summary>
        /// Will extract the header.
        /// </summary>
        Header = 2,

        /// <summary>
        /// Will extract the body.
        /// </summary>
        Body = 4,

        /// <summary>
        /// Will remove the specified PacketExtractionFlags from <see cref="PacketBuffer"/>.
        /// </summary>
        Remove = 8
    };
}
