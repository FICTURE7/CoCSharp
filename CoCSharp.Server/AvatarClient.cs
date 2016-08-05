using CoCSharp.Logic;
using CoCSharp.Network;
using LiteDB;
using System;
using System.Net.Sockets;
using System.Security.Cryptography;

namespace CoCSharp.Server
{
    // Represents a Avatar which has the ability to send 
    // and received Messages.
    public partial class AvatarClient : Avatar
    {
        public AvatarClient() : base()
        {
            // Space
        }

        public AvatarClient(CoCServer server, long id) : base()
        {
            Server = server;
            ID = id;
        }

        public AvatarClient(CoCServer server, Socket connection, NetworkManagerAsyncSettings settings) : base()
        {
            Server = server;

            Connection = connection;
            SessionKey = null;
            _networkManager = new NetworkManagerAsync(connection, settings);
            _networkManager.MessageReceived += OnMessageReceived;
            _networkManager.Disconnected += OnDisconnected;

            UpdateKeepAlive();
        }

        [BsonIgnore]
        public ClientFlags Status { get; internal set; }
        [BsonIgnore]
        public CoCServer Server { get; internal set; }
        [BsonIgnore]
        public Socket Connection { get; private set; }
        [BsonIgnore]
        public byte[] SessionKey { get; set; }

        [BsonIgnore]
        // Date when the last KeepAliveMessage was received.
        public DateTime LastKeepAlive { get; set; }
        [BsonIgnore]
        // Date when the next KeepAliveMessage should received.
        public DateTime ExpirationKeepAlive { get; set; }

        private NetworkManagerAsync _networkManager;

        public void SendMessage(Message message)
        {
            _networkManager.SendMessage(message);
        }

        public void Disconnect()
        {
            InternalDisconnect(false);
        }

        private void OnMessageReceived(object sender, MessageReceivedEventArgs e)
        {
            if (e.Exception != null)
            {
                Console.WriteLine("Exception occurred while receiving: {0}", e.Exception.ToString());
            }
            if (e.Exception is CryptographicException)
            {
                Console.WriteLine("\tCryptographicException occurred while decrypting a message.");
                Disconnect();
                return;
            }
            Server.HandleMessage(this, e.Message);
        }

        private void OnDisconnected(object sender, DisconnectedEventArgs e)
        {
            Disconnect();
        }

        internal void InternalDisconnect(bool dueToKeepAlive)
        {
            // Close connection to client.
            // Push the VillageObjects to the VillageObjectPool.
            if (Home != null)
                Home.Dispose();

            var remoteEndPoint = _networkManager.Socket.RemoteEndPoint;
            Save();
            _networkManager.Dispose();
            Server.Clients.Remove(this);

            var extraInfo = dueToKeepAlive ? "; keepalive expired" : string.Empty;
            Log.Info("listener", string.Format("disconnected {0}{1}", remoteEndPoint, extraInfo));
        }

        internal void UpdateKeepAlive()
        {
            LastKeepAlive = DateTime.UtcNow;
            ExpirationKeepAlive = LastKeepAlive.AddSeconds(30);
        }
    }
}
