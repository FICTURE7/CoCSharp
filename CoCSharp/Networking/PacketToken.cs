using System;
using System.Net.Sockets;

namespace CoCSharp.Networking
{
    internal class PacketToken
    {
        private static int m_NextTokenId = 0;
        public PacketToken(SocketAsyncEventArgs args)
        {
            args.UserToken = this;
            PacketBuffer = new PacketBuffer(args);
            Header = new byte[PacketBuffer.HeaderSize];
            TokenID = m_NextTokenId;
            m_NextTokenId++;
        }

        public ushort ID { get; set; }
        public int Length { get; set; }
        public ushort Version { get; set; }

        public byte[] Header { get; set; }
        public int HeaderReceiveOffset { get; set; }
        public int ReceiveOffset { get; set; }

        public byte[] Body { get; set; }
        public int BodyReceiveOffset { get; set; }




        public bool HasReadHeader { get; set; }
        public int BytesReceivedCount { get; set; }
        public int TokenID { get; set; }
        public PacketBuffer PacketBuffer { get; set; }

        public void Reset()
        {
            ID = 0;
            Length = 0;
            Version = 0;

            Header = new byte[PacketBuffer.HeaderSize];
            HeaderReceiveOffset = 0;

            Body = null;
            BodyReceiveOffset = 0;

            HasReadHeader = false;
        }
    }
}
