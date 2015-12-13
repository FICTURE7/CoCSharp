using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace CoCSharp.Proxy
{
    public class CoCProxy
    {
        public CoCProxy()
        {
            Connections = new List<CoCProxyClient>();
            _listener = new Socket(SocketType.Stream, ProtocolType.Tcp);
        }

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

            var server = new Socket(SocketType.Stream, ProtocolType.Tcp);
            server.Connect("gamea.clashofclans.com", 9339);
            Console.WriteLine("Created new connection to gamea.clashofclans.com");

            var connection = new CoCProxyClient(client, server);
            Connections.Add(connection);

            _listener.BeginAccept(AcceptClient, _listener);
        }
    }
}
