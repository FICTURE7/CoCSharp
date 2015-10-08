using System;
using System.IO;

namespace CoCSharp.Networking
{
    /// <summary>
    /// Provides methods to extract packets from <see cref="byte"/>
    /// array.
    /// </summary>
    public class PacketExtractor
    {
        /// <summary>
        /// Header size in bytes of the Clash of Clans packet structure.
        /// </summary>
        public const int HeaderSize = 7;

        /// <summary>
        /// Initializes a new instance of the <see cref="PacketExtractor"/> class with the specified
        /// <see cref="byte"/> array.
        /// </summary>
        /// <param name="buffer"></param>
        /// <exception cref="ArgumentNullException"/>
        public PacketExtractor(byte[] buffer)
        {
            if (buffer == null)
                throw new ArgumentNullException("buffer");

            m_Buffer = buffer;
            m_OriginalBufferSize = buffer.Length;
        }

        /// <summary>
        /// Gets the packet buffer bytes.
        /// </summary>
        public byte[] Buffer { get { return m_Buffer; } }

        private int m_OriginalBufferSize = -1;
        private byte[] m_Buffer = null;

        /// <summary>
        /// Extracts the packet with the specified bitmask of <see cref="PacketExtractionFlags"/> enum.
        /// </summary>
        /// <param name="flags">Bitmask of <see cref="PacketExtractionFlags"/>.</param>
        /// <param name="packetLength">Length of the packet in bytes.</param>
        /// <returns>Packet bytes.</returns>
        public byte[] ExtractPacket(PacketExtractionFlags flags, int packetLength)
        {
            using (var packetStream = new MemoryStream(packetLength + HeaderSize))
            {
                if (flags.HasFlag(PacketExtractionFlags.Header))
                {
                    packetStream.Write(Buffer, 0, HeaderSize);
                }
                if (flags.HasFlag(PacketExtractionFlags.Body))
                {
                    packetStream.Write(Buffer, HeaderSize, packetLength);
                }
                if (flags.HasFlag(PacketExtractionFlags.Remove))
                {
                    var buffer = new byte[Buffer.Length - (packetLength + HeaderSize)];
                    Array.Copy(Buffer, packetLength + HeaderSize, buffer, 0, buffer.Length);
                    Array.Resize(ref buffer, m_OriginalBufferSize);
                    m_Buffer = buffer;
                }
                return packetStream.ToArray();
            }
        }
    }
}
