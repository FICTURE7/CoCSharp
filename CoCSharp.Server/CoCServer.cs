using CoCSharp.Networking;
using CoCSharp.Networking.Packets;
using CoCSharp.Server.Handlers;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace CoCSharp.Server
{
    public class CoCServer
    {
        public CoCServer()
        {
            Clients = new List<CoCRemoteClient>();
            AcceptAsyncEventPool = new SocketAsyncEventArgsPool(100);
        }

        public Socket Listener { get; set; }
        public IPEndPoint EndPoint { get; set; }
        public List<CoCRemoteClient> Clients { get; set; }

        private bool ShuttingDown { get; set; }
        private SocketAsyncEventArgsPool AcceptAsyncEventPool { get; set; }

        public void Start(IPEndPoint endPoint)
        {
            ShuttingDown = false;
            EndPoint = endPoint;
            Listener = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            Listener.Bind(endPoint);
            Listener.Listen(100);

            AcceptSocketAsync();
        }

        public void Stop()
        {
            if (Listener != null)
                Listener.Close();
        }

        private void AcceptSocketAsync()
        {
            while (AcceptAsyncEventPool.Count > 1 && !ShuttingDown)
            {
                var args = AcceptAsyncEventPool.Pop();
                args.Completed += AcceptCompleted;

                if (!Listener.AcceptAsync(args))
                    AcceptCompleted(Listener, args);
            }
        }

        private void AcceptCompleted(object sender, SocketAsyncEventArgs e)
        {
            e.Completed -= AcceptCompleted;
            switch (e.LastOperation)
            {
                case SocketAsyncOperation.Accept:
                    var socket = e.AcceptSocket;
                    e.AcceptSocket = null;
                    Console.WriteLine("Accepted new socket: {0}", socket.RemoteEndPoint);
                    var client = new CoCRemoteClient(this, socket);
                    Clients.Add(client);
                    InGamePacketHandler.RegisterInGamePacketHandlers(client);
                    AcceptAsyncEventPool.Push(e);
                    break;
            }
            AcceptSocketAsync();
        }
    }
}
