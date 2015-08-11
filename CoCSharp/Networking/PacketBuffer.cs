using System;
using System.IO;

namespace CoCSharp.Networking
{
    //TODO: NEED TO FIX DIS BED BOI!!

    /// <summary>
    /// Provides methods to extract packets from a <see cref="Byte"/> array.
    /// </summary>
    public class PacketBuffer
    {
        /// <summary>
        /// Header size in bytes of the Clash of Clans packet structure.
        /// </summary>
        public const int HeaderSize = 7;

        /// <summary>
        /// Initializes a new instance of the <see cref="PacketBuffer"/> class with the specified
        /// <see cref="Byte"/> array buffer.
        /// </summary>
        /// <param name="packetBuffer">The packet bytes.</param>
        public PacketBuffer(byte[] packetBuffer)
        {
            Buffer = packetBuffer;
            OriginalBufferSize = packetBuffer.Length;
        }

        /// <summary>
        /// Gets the packet buffer bytes.
        /// </summary>
        public byte[] Buffer { get; private set; }

        private int OriginalBufferSize { get; set; }

        /// <summary>
        /// Clears the buffer.
        /// </summary>
        public void Clear()
        {
            Array.Clear(Buffer, 0, Buffer.Length);
        }

        /// <summary>
        /// Resets the buffer.
        /// </summary>
        public void Reset()
        {
            Buffer = new byte[OriginalBufferSize];
        }

        /// <summary>
        /// Resize to the buffer to its original size.
        /// </summary>
        public void ResizeToOriginalSize()
        {
            var buff = Buffer;
            Array.Resize(ref buff, OriginalBufferSize);
            Buffer = buff;
        }

        /// <summary>
        /// Extracts the packet with the specified bitmask of <see cref="PacketExtractionFlags"/> enum.
        /// </summary>
        /// <param name="flags">Bitmask of <see cref="PacketExtractionFlags"/>.</param>
        /// <param name="packetLength">Length of the packet in bytes.</param>
        /// <returns>Packet bytes.</returns>
        public byte[] ExtractPacket(PacketExtractionFlags flags, int packetLength)
        {
            var packetStream = new MemoryStream(packetLength + HeaderSize);
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
                Buffer = buffer;
                ResizeToOriginalSize();
            }
            return packetStream.ToArray();
        }
    }
}
