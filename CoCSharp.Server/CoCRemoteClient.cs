using CoCSharp.Logic;
using CoCSharp.Networking;
using CoCSharp.Networking.Packets;
using System;
using System.Collections.Generic;
using System.Net.Sockets;

namespace CoCSharp.Server
{
    public class CoCRemoteClient
    {
        public CoCRemoteClient(CoCServer server, Socket connection)
        {
            this.Connection = connection;
            // this.NetworkManager = new NetworkManager(connection);
            this.SendAsyncEventPool = new SocketAsyncEventArgsPool(50);
            this.ReceiveAsyncEventPool = new SocketAsyncEventArgsPool(50);

            StartReceive();
        }

        public string Username { get; set; }
        public string UserToken { get; set; }
        public string FingerprintHash { get; set; }
        public long UserID { get; set; }
        public int Seed { get; set; }
        public bool LoggedIn { get; set; }
        public Village Home { get; set; }
        public Socket Connection { get; set; }
        public NetworkManager NetworkManager { get; set; }
        
        private SocketAsyncEventArgsPool SendAsyncEventPool { get; set; }
        private SocketAsyncEventArgsPool ReceiveAsyncEventPool { get; set; }
        private Dictionary<ushort, Type> PacketHandlers { get; set; }

        public void QueuePacket(IPacket packet)
        {
            var args = SendAsyncEventPool.Pop();
            args.Completed += OperationCompleted;
        }

        private void StartReceive()
        {
            var args = ReceiveAsyncEventPool.Pop();
            args.Completed += OperationCompleted;

            if (!Connection.ReceiveAsync(args))
                OperationCompleted(Connection, args);
        }

        private void OperationCompleted(object sender, SocketAsyncEventArgs e)
        {
            e.Completed -= OperationCompleted;

            switch (e.LastOperation)
            {
                case SocketAsyncOperation.Receive:
                    // handle shit here

                    // clear the buffer
                    SendAsyncEventPool.Push(e);
                    break;

                case SocketAsyncOperation.Send:
                    // check if disconnection
                    SendAsyncEventPool.Push(e);
                    break;
            }
        }

        private void HandleReceived(SocketAsyncEventArgs args)
        {
            var buffer = args.Buffer;
        }
    }
}
