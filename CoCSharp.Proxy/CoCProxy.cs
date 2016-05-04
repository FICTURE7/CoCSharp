using CoCSharp.Network;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace CoCSharp.Server
{
    public class CoCProxy
    {
        public CoCProxy()
        {
            Connections = new List<CoCProxyClient>();
            Settings = new NetworkManagerAsyncSettings();
            _listener = new Socket(SocketType.Stream, ProtocolType.Tcp);
        }

        public NetworkManagerAsyncSettings Settings { get; private set; }
        public List<CoCProxyClient> Connections { get; private set; }

        private Socket _listener;

        public void Start(IPEndPoint endPoint)
        {
            _listener.Bind(endPoint);
            _listener.Listen(10);
            _listener.BeginAccept(AcceptClient, _listener);
        }

        private void AcceptClient(IAsyncResult ar)
        {
            var client = _listener.EndAccept(ar);
            Console.WriteLine("Accepted new client: {0}", client.RemoteEndPoint);

            // Starts accepting as soon as possible.
            _listener.BeginAccept(AcceptClient, _listener);

            // Creates a new connection to the official server.
            var server = new Socket(SocketType.Stream, ProtocolType.Tcp);
            server.Connect("gamea.clashofclans.com", 9339);
            Console.WriteLine("Created new connection to gamea.clashofclans.com");

            var connection = new CoCProxyClient(client, server, Settings);
            Connections.Add(connection);
        }
    }
}
