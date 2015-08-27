using System.Net;
using System.Net.Sockets;

namespace CoCSharp.Proxy
{
    public class CoCProxyClient
    {
        public CoCProxyClient(Socket clientConnection)
        {
            Client = new CoCRemoteClient(clientConnection);
        }

        public CoCRemoteClient Client { get; set; }
        public CoCRemoteClient Server { get; set; }

        public void Start(Socket serverConnection)
        {
            Server = new CoCRemoteClient(serverConnection);
        }
    }
}

