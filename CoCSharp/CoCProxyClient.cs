using System.Net;
using System.Net.Sockets;

namespace CoCSharp
{
    public class CoCProxyClient
    {
        public CoCProxyClient(TcpClient client)
        {
            this.Client = new CoCRemoteClient(client);
            //this.Server = new CoCRemoteClient(server);
        }

        public CoCRemoteClient Client { get; set; }
        public CoCRemoteClient Server { get; set; }

        public void Start(TcpClient server)
        {
            Server = new CoCRemoteClient(server);
        }
    }
}
