using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace CoCSharp
{
    public class CoCServer
    {
        public const int DefaultPort = 9339;

        public CoCServer()
        {
            this.Clients = new List<CoCRemoteClient>();

            throw new NotImplementedException();
        }

        public List<CoCRemoteClient> Clients { get; set; }
        public TcpListener Listener { get; set; }

        private Thread NetworkHandler { get; set; }

        public void Start(IPEndPoint endPoint)
        {
            NetworkHandler = new Thread(HandleNetwork);
            Listener = new TcpListener(endPoint);

            Listener.Start(10);
            Listener.BeginAcceptTcpClient(new AsyncCallback(AcceptClientAysnc), Listener);
            NetworkHandler.Start();
        }

        public void Stop()
        {
            NetworkHandler.Abort();
            Listener.Stop();
        }

        private void AcceptClientAysnc(IAsyncResult result)
        {
            var client = Listener.EndAcceptTcpClient(result);
            var cocClient = new CoCRemoteClient(client);
            Clients.Add(cocClient);
        }

        private void HandleNetwork()
        {
            while (true)
            {
                for (int i = 0; i < Clients.Count; i++)
                {
                    var client = Clients[i];

                    // throw new NotImplementedException();
                }
            }
        }
    }
}
