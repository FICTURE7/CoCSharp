using System;
using System.IO;
using System.Linq;

namespace CoCSharp.Networking
{
    public class PacketBuffer
    {
        public const int HeaderSize = 7;

        public PacketBuffer(byte[] packetBuffer)
        {
            this.Buffer = packetBuffer;
            this.OriginalBufferSize = packetBuffer.Length;
        }

        public byte[] Buffer { get; private set; }

        private int OriginalBufferSize { get; set; }

        public void Clear()
        {
            Array.Clear(Buffer, 0, Buffer.Length);
        }

        public void Reset()
        {
            Buffer = new byte[OriginalBufferSize];
        }

        public void ResizeToOriginalSize()
        {
            var buff = Buffer;
            Array.Resize(ref buff, OriginalBufferSize);
            Buffer = buff;
        }

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
