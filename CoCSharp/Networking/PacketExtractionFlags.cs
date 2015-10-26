using System;

namespace CoCSharp.Networking
{
    /// <summary>
    /// Controls the <see cref="PacketExtractor"/> on how it extracts packets bytes.
    /// </summary>
    [Flags]
    public enum PacketExtractionFlags : byte
    {
        /// <summary>
        /// Instructs the <see cref="PacketExtractor"/> to extract the header.
        /// </summary>
        Header = 2,

        /// <summary>
        /// Instructs the <see cref="PacketExtractor"/> to extract the body.
        /// </summary>
        Body = 4,

        /// <summary>
        /// Instructs the <see cref="PacketExtractor"/> to remove 
        /// the specified PacketExtractionFlags from <see cref="PacketExtractor.Buffer"/> 
        /// byte array.
        /// </summary>
        Remove = 8
    };
}
