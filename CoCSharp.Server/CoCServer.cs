using CoCSharp.Logic;
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
        }

        public Avatar NewAvatar { get { return new Avatar(); } }
        public Socket Listener { get; set; }
        public IPEndPoint EndPoint { get; set; }
        public List<CoCRemoteClient> Clients { get; set; }

        private bool ShuttingDown { get; set; }

        public void Start(IPEndPoint endPoint)
        {
            ShuttingDown = false;
            EndPoint = endPoint;
            Listener = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            Listener.Bind(endPoint);
            Listener.Listen(100);
            Listener.BeginAccept(AcceptClient, Listener);
        }

        public void Stop()
        {
            if (Listener != null)
                Listener.Close();
        }

        private void AcceptClient(IAsyncResult ar)
        {
            var socket = Listener.EndAccept(ar);
            var client = new CoCRemoteClient(this, socket);
            Clients.Add(client);

            LoginPacketHandlers.RegisterLoginPacketHandlers(client);
            InGamePacketHandler.RegisterInGamePacketHandlers(client);
            Listener.BeginAccept(AcceptClient, Listener);
        }
    }
}
