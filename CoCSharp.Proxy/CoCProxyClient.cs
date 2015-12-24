using CoCSharp.Networking;
using System.Net.Sockets;

namespace CoCSharp.Proxy
{
    public class CoCProxyClient
    {
        public CoCProxyClient(Socket client, Socket server, NetworkManagerAsyncSettings settings)
        {
            Client = new NetworkManagerAsync(client, settings);
            Client.MessageReceived += ClientReceived;

            Server = new NetworkManagerAsync(server, settings);
            Server.MessageReceived += ServerReceived;
        }

        public NetworkManagerAsync Client { get; private set; }
        public NetworkManagerAsync Server { get; private set; }

        private void ServerReceived(object sender, MessageReceivedEventArgs e)
        {
            Client.Connection.Send(e.MessageData);
        }

        private void ClientReceived(object sender, MessageReceivedEventArgs e)
        {
            Server.Connection.Send(e.MessageData);
        }
    }
}
