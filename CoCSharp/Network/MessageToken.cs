using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace CoCSharp.Network
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

    internal class MessageReceiveToken
    {
        public MessageReceiveToken()
        {
            Header = new byte[Message.HeaderSize];
            TokenID = NetworkManagerAsyncStatistics.NextTokenID;
        }

        public int TokenID { get; set; }
        public SocketAsyncEventArgs Args { get; set; }

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
            var token = new MessageReceiveToken();
            token.Args = args;
            token.Offset = args.Offset;
            args.UserToken = token;
        }
    }
}
