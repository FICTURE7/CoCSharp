using System.Net.Sockets;

namespace CoCSharp.Networking
{
    internal class MessageToken
    {
        private static int _tokenID = 0;
        public MessageToken()
        {
            Header = new byte[Message.HeaderSize];
            TokenID = _tokenID;
            _tokenID++;
        }

        public int TokenID { get; set; }
        public ushort ID { get; set; }
        public int Length { get; set; }
        public ushort Version { get; set; }

        public int Offset { get; set; }
        public int HeaderRemaining { get { return Message.HeaderSize - HeaderOffset; } }
        public int HeaderOffset { get; set; }
        public byte[] Header { get; set; }

        public int BodyRemaining { get { return Length - BodyOffset; } }
        public int BodyOffset { get; set; }
        public byte[] Body { get; set; }

        public void Reset()
        {
            ID = 0;
            Length = 0;
            Version = 0;

            Header = new byte[Message.HeaderSize];
            HeaderOffset = 0;

            Body = null;
            BodyOffset = 0;
        }

        public static void Create(SocketAsyncEventArgs args)
        {
            args.UserToken = new MessageToken();
        }
    }
}
