using CoCSharp.Networking;
using System.Net.Sockets;

namespace CoCSharp
{
    public class CoCRemoteClient // will contain every info about client
    {
        public CoCRemoteClient(TcpClient client)
        {
            this.Client = client;
            this.NetworkManager = new NetworkManager(client);
        }

        public TcpClient Client { get; set; }
        public NetworkManager NetworkManager { get; set; }
        public uint Seed { get; set; }
    }
}
