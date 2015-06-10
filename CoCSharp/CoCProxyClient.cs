using System.Net.Sockets;

namespace CoCSharp
{
    public class CoCProxyClient
    {
        public CoCProxyClient(TcpClient client, TcpClient server)
        {
            this.Client = new CoCRemoteClient(client);
            this.Server = new CoCRemoteClient(server);
        }

        public CoCRemoteClient Client { get; set; }
        public CoCRemoteClient Server { get; set; }
    }
}
