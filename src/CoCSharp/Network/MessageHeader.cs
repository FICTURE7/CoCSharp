using System;

namespace CoCSharp.Network
{
    /// <summary>
    /// Represents the header of a <see cref="Message"/>.
    /// </summary>
    public struct MessageHeader
    {
        /// <summary>
        /// Size of a <see cref="MessageHeader"/>. This field is constant.
        /// </summary>
        public const int Size = 7;

        /// <summary>
        /// Constructs a new instance of the <see cref="MessageHeader"/> with the specified
        /// message ID, length and version.
        /// </summary>
        /// <param name="id">ID of the <see cref="Message"/>.</param>
        /// <param name="length">Length of the <see cref="Message"/>.</param>
        /// <param name="version">Version of the <see cref="Message"/>.</param>
        public MessageHeader(ushort id, int length, ushort version)
        {
            if (length < 0 || length > Message.MaxSize)
                throw new ArgumentOutOfRangeException(nameof(length), "Length must be non-negative and less or equal to the maximum size of a message.");

            Id = id;
            Length = length;
            Version = version;
        }

        /// <summary>
        /// ID of the <see cref="Message"/>.
        /// </summary>
        public readonly ushort Id;
        /// <summary>
        /// Length of the <see cref="Message"/>.
        /// </summary>
        public readonly int Length;
        /// <summary>
        /// Version of the <see cref="Message"/>.
        /// </summary>
        public readonly ushort Version;
    }
}
