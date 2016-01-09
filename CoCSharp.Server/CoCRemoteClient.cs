using CoCSharp.Networking;
using System.Net.Sockets;
using CoCSharp.Server.Handlers;
using CoCSharp.Logic;
using System;

namespace CoCSharp.Server
{
    public class CoCRemoteClient
    {
        public CoCRemoteClient(CoCServer server, Socket connection, NetworkManagerAsyncSettings settings)
        {
            _server = server;
            Connection = connection;
            Avatar = new Avatar();
            NetworkManager = new NetworkManagerAsync(connection, settings);
            NetworkManager.MessageReceived += OnMessageReceived;
        }

        public Avatar Avatar { get; set; }
        public Socket Connection { get; private set; }
        public NetworkManagerAsync NetworkManager { get; private set; }

        private readonly CoCServer _server;

        private void OnMessageReceived(object sender, MessageReceivedEventArgs e)
        {
            if (e.Exception != null)
                Console.WriteLine("Exception occured: {0}", e.Exception.ToString());

            var handler = (MessageHandler)null;
            if (_server.MessageHandlers.TryGetValue(e.Message.ID, out handler))
                handler(_server, this, e.Message);
        }
    }
}
