using System;
using System.IO;
using System.Net.Sockets;

namespace CoCSharp.Networking
{
    /// <summary>
    /// Provides methods to extract packets from a <see cref="SocketAsyncEventArgs"/> object or <see cref="Byte"/>
    /// array.
    /// </summary>
    public class PacketBuffer
    {
        //TODO: Improve this bad boi

        /// <summary>
        /// Header size in bytes of the Clash of Clans packet structure.
        /// </summary>
        public const int HeaderSize = 7;

        /// <summary>
        /// Initializes a new instance of the <see cref="PacketBuffer"/> class with the specified
        /// <see cref="SocketAsyncEventArgs"/>.
        /// </summary>
        /// <param name="args"></param>
        /// <exception cref="System.ArgumentNullException"/>
        public PacketBuffer(SocketAsyncEventArgs args)
        {
            if (args == null)
                throw new ArgumentNullException("args");

            var buffer = new byte[65535];
            m_InitializedFromByteArray = false;
            m_OriginalBufferSize = buffer.Length;
            m_SocketAsyncEventArgs = args;

            args.UserToken = this;
            args.SetBuffer(buffer, 0, buffer.Length);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PacketBuffer"/> class with the specified
        /// <see cref="Byte"/> array.
        /// </summary>
        /// <param name="args"></param>
        /// <exception cref="System.ArgumentNullException"/>
        public PacketBuffer(byte[] buffer)
        {
            if (buffer == null)
                throw new ArgumentNullException("buffer");

            m_InitializedFromByteArray = false;
            m_OriginalBufferSize = buffer.Length;
        }

        /// <summary>
        /// Gets the packet buffer bytes.
        /// </summary>
        public byte[] Buffer
        {
            get
            {
                return m_InitializedFromByteArray == true ? m_Buffer : m_SocketAsyncEventArgs.Buffer;
            }
        }

        private SocketAsyncEventArgs m_SocketAsyncEventArgs = null;
        private bool m_InitializedFromByteArray = false;
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
                    if (!m_InitializedFromByteArray)
                        m_SocketAsyncEventArgs.SetBuffer(buffer, 0, m_OriginalBufferSize);
                    else
                        m_Buffer = buffer;
                }
                return packetStream.ToArray();
            }
        }
    }
}
