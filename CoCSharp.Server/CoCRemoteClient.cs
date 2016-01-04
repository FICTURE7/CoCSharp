using CoCSharp.Networking;
using System.Net.Sockets;
using CoCSharp.Server.Handlers;

namespace CoCSharp.Server
{
    public class CoCRemoteClient
    {
        public CoCRemoteClient(CoCServer server, Socket connection)
        {
            _server = server;
            Connection = connection;
            NetworkManager = new NetworkManagerAsync(connection);
            NetworkManager.MessageReceived += OnMessageReceived;
        }

        public Socket Connection { get; private set; }
        public NetworkManagerAsync NetworkManager { get; private set; }

        private readonly CoCServer _server;

        private void OnMessageReceived(object sender, MessageReceivedEventArgs e)
        {
            var handler = (MessageHandler)null;
            if (_server.MessageHandlers.TryGetValue(e.Message.ID, out handler))
                handler(_server, this, e.Message);
        }
    }
}
