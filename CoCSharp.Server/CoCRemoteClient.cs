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
            Server = server;

            Connection = connection;
            SessionKey = null;
            NetworkManager = new NetworkManagerAsync(connection, settings);
            NetworkManager.MessageReceived += OnMessageReceived;
            NetworkManager.Disconnected += OnDisconnected;
        }

        public Avatar Avatar { get; set; }
        public CoCServer Server { get; private set; }
        public Socket Connection { get; private set; }
        public NetworkManagerAsync NetworkManager { get; private set; }
        public byte[] SessionKey { get; set; }

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

            Server.HandleMessage(this, e.Message);
        }

        private void OnDisconnected(object sender, DisconnectedEventArgs e)
        {
            // Push the VillageObjects to the VillageObjectPool.
            if (Avatar != null && Avatar.Home != null)
            {
                Avatar.Home.Dispose();
            }

            // Dereference the client object so that it gets picked up
            // by the GarbageCollector.
            Server.Clients.Remove(this);
            FancyConsole.WriteLine("[&(darkyellow)Listener&(default)] -> Avatar &(darkcyan){0}&(default) disconnected.",
                                   Avatar == null ? Connection.RemoteEndPoint.ToString() : Avatar.Token);
        }
    }
}
