using CoCSharp.Logic;
using CoCSharp.Network;
using CoCSharp.Server.Core;
using System;
using System.Net.Sockets;
using System.Security.Cryptography;

namespace CoCSharp.Server
{
    public class CoCRemoteClient
    {
        public CoCRemoteClient(CoCServer server, Socket connection, NetworkManagerAsyncSettings settings)
        {
            _server = server;

            Connection = connection;
            SessionKey = null;
            NetworkManager = new NetworkManagerAsync(connection, settings);
            NetworkManager.MessageReceived += OnMessageReceived;
            NetworkManager.Disconnected += OnDisconnected;
        }

        public Avatar Avatar { get; set; }

        public Socket Connection { get; private set; }

        public NetworkManagerAsync NetworkManager { get; private set; }

        public byte[] SessionKey { get; set; }

        private readonly CoCServer _server;

        private void OnMessageReceived(object sender, MessageReceivedEventArgs e)
        {
            if (e.Exception != null)
            {
                Console.WriteLine("Exception occurred while receiving: {0}", e.Exception.ToString());
            }
            if (e.Exception is CryptographicException)
            {
                Console.WriteLine("\tCryptographicException occurred while decrypting a message.");
                //TODO: Disconnect the client.
                return;
            }

            _server.HandleMessage(this, e.Message);
        }

        private void OnDisconnected(object sender, DisconnectedEventArgs e)
        {
            // Dereference the client object so that it gets picked up
            // by the GarbageCollector.
            _server.Clients.Remove(this);
            FancyConsole.WriteLine("[&(darkyellow)Listener&(default)] -> Avatar &(darkcyan){0}&(default) disconnected.",
                                   Avatar == null ? Connection.RemoteEndPoint.ToString() : Avatar.Token);
        }
    }
}
