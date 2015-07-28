using System;
using System.IO;

namespace CoCSharp.Networking
{
    /// <summary>
    /// 
    /// </summary>
    public class PacketBuffer
    {
        /// <summary>
        /// Header size in bytes of the Clash of Clans protocol.
        /// </summary>
        public const int HeaderSize = 7;

        /// <summary>
        /// Initializes a new instance of the <see cref="PacketBuffer"/> class with the specified
        /// byte[] buffer.
        /// </summary>
        /// <param name="packetBuffer">The packet bytes.</param>
        public PacketBuffer(byte[] packetBuffer)
        {
            Buffer = packetBuffer;
            OriginalBufferSize = packetBuffer.Length;
        }

        /// <summary>
        /// 
        /// </summary>
        public byte[] Buffer { get; private set; }

        private int OriginalBufferSize { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public void Clear()
        {
            Array.Clear(Buffer, 0, Buffer.Length);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Reset()
        {
            Buffer = new byte[OriginalBufferSize];
        }

        /// <summary>
        /// 
        /// </summary>
        public void ResizeToOriginalSize()
        {
            var buff = Buffer;
            Array.Resize(ref buff, OriginalBufferSize);
            Buffer = buff;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="flags"></param>
        /// <param name="packetLength"></param>
        /// <returns></returns>
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
            }
            return packetStream.ToArray();
        }
    }
}
