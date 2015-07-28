using CoCSharp.Logic;
using CoCSharp.Networking;
using CoCSharp.Networking.Packets;
using System.Net.Sockets;

namespace CoCSharp.Proxy
{
    public class CoCProxyClient
    {
        public CoCProxyClient(ICoCServer server, Socket connection, PacketDirection direction)
        {
            this.Proxy = server;
            this.Connection = connection;
            this.ClientNetworkManager = new NetworkManager(server, connection, HandleNetwork, direction);
        }

        public string Username { get; set; }
        public string UserToken { get; set; }
        public string FingerprintHash { get; set; }
        public long UserID { get; set; }
        public int Seed { get; set; }
        public bool LoggedIn { get; set; }
        public Village Home { get; set; }
        public Socket Connection { get; set; }
        public NetworkManager ClientNetworkManager { get; set; }
        public NetworkManager ServerNetworkManager { get; set; }

        private ICoCServer Proxy { get; set; }

        private void HandleNetwork(SocketAsyncEventArgs args, IPacket packet)
        {
            
        }
    }
}
