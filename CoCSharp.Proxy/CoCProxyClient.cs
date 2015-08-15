using CoCSharp.Logic;
using CoCSharp.Networking;
using CoCSharp.Networking.Packets;
using System.Net.Sockets;

namespace CoCSharp.Proxy
{
    public class CoCProxyClient
    {
        public CoCProxyClient(CoCProxy server, Socket connection)
        {
            Proxy = server;
            Connection = connection;
            ClientNetworkManager = new NetworkManagerAsync(connection, HandleNetworkClient, null);
        }

        public string Username { get; set; }
        public string UserToken { get; set; }
        public string FingerprintHash { get; set; }
        public long UserID { get; set; }
        public int Seed { get; set; }
        public bool LoggedIn { get; set; }
        public Village Home { get; set; }
        public Socket Connection { get; set; }
        public NetworkManagerAsync ClientNetworkManager { get; set; }
        public NetworkManagerAsync ServerNetworkManager { get; set; }

        private CoCProxy Proxy { get; set; }

        public void Start(Socket connection)
        {
            ServerNetworkManager = new NetworkManagerAsync(connection, HandleNetworkServer, null);
        }

        private void HandleNetworkClient(SocketAsyncEventArgs args, IPacket packet)
        {
            ServerNetworkManager.WritePacket(packet);
            Proxy.PacketLogger.LogPacket(packet, PacketDirection.Server);
        }

        private void HandleNetworkServer(SocketAsyncEventArgs args, IPacket packet)
        {
            ClientNetworkManager.WritePacket(packet);
            Proxy.PacketLogger.LogPacket(packet, PacketDirection.Client);
        }
    }
}
