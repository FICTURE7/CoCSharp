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
            Proxy = server;
            Connection = connection;
            ClientNetworkManager = new NetworkManager(server, connection, HandleNetworkClient, direction);
            ServerNetworkManager = new NetworkManager(server, connection, HandleNetworkServer, direction);
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

        private void HandleNetworkClient(SocketAsyncEventArgs args, IPacket packet)
        {
            ServerNetworkManager.Connection.Send(args.Buffer);
        }

        private void HandleNetworkServer(SocketAsyncEventArgs args, IPacket packet)
        {
            ClientNetworkManager.Connection.Send(args.Buffer);
        }
    }
}
