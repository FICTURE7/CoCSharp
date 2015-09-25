using System;
using System.Net.Sockets;

namespace CoCSharp.Networking
{
    public class DisconnectedEventArgs : EventArgs
    {
        public DisconnectedEventArgs(SocketAsyncEventArgs args)
        {
            Connection = args.ConnectSocket;
            SocketAsyncEventArgs = args;
        }

        public Socket Connection { get; private set; }
        public SocketAsyncEventArgs SocketAsyncEventArgs { get; private set; }
    }
}
