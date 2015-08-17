using CoCSharp.Networking;
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
                args.Completed += OperationCompleted;

                if (!Listener.AcceptAsync(args))
                    OperationCompleted(Listener, args);
            }
        }

        private void OperationCompleted(object sender, SocketAsyncEventArgs e)
        {
            e.Completed -= OperationCompleted;
            switch (e.LastOperation)
            {
                case SocketAsyncOperation.Accept:
                    Console.WriteLine("Something connected.");
                    var client = new CoCRemoteClient(this, e.AcceptSocket);
                    Clients.Add(client);
                    InGamePacketHandler.RegisterInGamePacketHandlers(client);
                    e.AcceptSocket = null;
                    AcceptAsyncEventPool.Push(e);
                    break;
            }
            AcceptSocketAsync();
        }
    }
}
