using System.Net.Sockets;

namespace CoCSharp.Networking
{
    internal class MessageSendToken
    {
        public MessageSendToken()
        {
            TokenID = NetworkManagerAsyncStatistics.NextTokenID;
        }

        public int TokenID { get; set; }
        public SocketAsyncEventArgs Args { get; set; }

        public ushort ID { get; set; }
        public int Length { get; set; }
        public ushort Version { get; set; }

        public int SendRemaining { get { return Body.Length - SendOffset; } }
        public int SendOffset { get; set; }

        public byte[] Body { get; set; }

        public void Reset()
        {
            ID = 0;
            Length = 0;
            Version = 0;

            Body = null;
            SendOffset = 0;
        }

        public static void Create(SocketAsyncEventArgs args)
        {
            var token = new MessageSendToken();
            token.Args = args;
            args.UserToken = token;
        }
    }
}
