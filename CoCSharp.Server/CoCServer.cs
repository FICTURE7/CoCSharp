using CoCSharp.Logic;
using CoCSharp.Server.Handlers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace CoCSharp.Server
{
    public class CoCServer
    {
        public CoCServer()
        {
            Clients = new List<CoCRemoteClient>();
            AvatarManager = new AvatarManager(this);
            DefaultVillage = new Village();
            DefaultVillage.FromJson(File.ReadAllText("default_village.json"));
        }

        public Village DefaultVillage { get; set; }
        public AvatarManager AvatarManager { get; set; }
        public Socket Listener { get; set; }
        public IPEndPoint EndPoint { get; set; }
        public List<CoCRemoteClient> Clients { get; set; }

        public void Start(IPEndPoint endPoint)
        {
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
            Console.WriteLine("Accepted new connection: {0}", socket.RemoteEndPoint);

            // should use a single dictionary of handlers instead.
            LoginPacketHandlers.RegisterLoginPacketHandlers(client);
            InGamePacketHandler.RegisterInGamePacketHandlers(client);
            Clients.Add(client);
            Listener.BeginAccept(AcceptClient, Listener);
        }
    }
}
