using CoCSharp.Networking;
using System.Net.Sockets;
using CoCSharp.Server.Handlers;
using CoCSharp.Logic;
using System;
using CoCSharp.Networking.Cryptography;
using System.Security.Cryptography;

namespace CoCSharp.Server
{
    public class CoCRemoteClient
    {
        public CoCRemoteClient(CoCServer server, Socket connection, NetworkManagerAsyncSettings settings)
        {
            _server = server;
            Connection = connection;
            SessionKey = Crypto8.GenerateNonce();
            Avatar = new Avatar();
            NetworkManager = new NetworkManagerAsync(connection, settings);
            NetworkManager.MessageReceived += OnMessageReceived;
        }

        public Avatar Avatar { get; set; }
        public Socket Connection { get; private set; }
        public NetworkManagerAsync NetworkManager { get; private set; }
        public byte[] SessionKey { get; private set; }

        private readonly CoCServer _server;

        private void OnMessageReceived(object sender, MessageReceivedEventArgs e)
        {
            if (e.Exception != null)
                Console.WriteLine("Exception occurred while receiving: {0}", e.Exception.ToString());
            if (e.Exception is CryptographicException)
                Console.WriteLine("\tCryptographicException occurred while decrypting a message.");

            try
            {
                var handler = (MessageHandler)null;
                if (_server.MessageHandlerDictionary.TryGetValue(e.Message.ID, out handler))
                    handler(_server, this, e.Message);
            }
            catch(Exception ex)
            {
                Console.WriteLine("Exception occurred while handling: {0}", ex.ToString());
            }
        }
    }
}
