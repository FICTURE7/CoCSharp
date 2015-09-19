using System.Net.Sockets;

namespace CoCSharp.Networking
{
    internal sealed class PacketToken
    {
        private static int m_NextTokenId = 1;
        private PacketToken(SocketAsyncEventArgs args)
        {
            args.UserToken = this;
            Header = new byte[PacketExtractor.HeaderSize];
            ReceiveOffset = args.Offset;
            TokenID = m_NextTokenId;
            m_NextTokenId++;
        }

        public ushort ID { get; set; }
        public int Length { get; set; }
        public ushort Version { get; set; }

        public int ReceiveOffset { get; set; }

        public byte[] Header { get; set; }
        public int HeaderReceiveOffset { get; set; }

        public byte[] Body { get; set; }
        public int BodyReceiveOffset { get; set; }

        public int TokenID { get; private set; } // for testing

        public void Reset()
        {
            ID = 0;
            Length = 0;
            Version = 0;

            Header = new byte[PacketExtractor.HeaderSize];
            HeaderReceiveOffset = 0;

            Body = null;
            BodyReceiveOffset = 0;
        }

        public static void Create(SocketAsyncEventArgs args)
        {
            args.UserToken = new PacketToken(args);
        }
    }
}
