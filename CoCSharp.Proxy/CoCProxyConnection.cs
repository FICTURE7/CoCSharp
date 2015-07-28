using CoCSharp.Networking;
using System.Net;
using System.Net.Sockets;

namespace CoCSharp.Proxy
{
    public class CoCProxyConnection
    {
        public CoCProxyConnection(ICoCServer server, Socket clientConnection)
        {
            this.Client = new CoCProxyClient(server, clientConnection, PacketDirection.Client);
        }

        public CoCProxyClient Client { get; set; }
        public CoCProxyClient Server { get; set; }

        public void Start(ICoCServer server, Socket serverConnection)
        {
            Server = new CoCProxyClient(server, serverConnection, PacketDirection.Server);
        }
    }
}
