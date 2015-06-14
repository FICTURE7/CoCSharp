using CoCSharp.Logic;
using CoCSharp.Networking;
using System.Net.Sockets;

namespace CoCSharp
{
    public class CoCRemoteClient
    {
        public CoCRemoteClient(TcpClient client)
        {
            this.Client = client;
            this.NetworkManager = new NetworkManager(client);
        }

        public string Username { get; set; }
        public string UserToken { get; set; }
        public long UserID { get; set; }
        public int Seed { get; set; }
        public Village Home { get; set; }
        public TcpClient Client { get; set; }
        public NetworkManager NetworkManager { get; set; }
    }
}
