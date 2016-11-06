using CoCSharp.Logic;
using CoCSharp.Network;
using CoCSharp.Network.Cryptography;
using CoCSharp.Server.API;
using CoCSharp.Server.API.Core;
using CoCSharp.Server.Core;
using System;
using System.Net;
using System.Net.Sockets;

namespace CoCSharp.Server
{
    public class Client : IClient
    {
        #region Constructors
        public Client(Server server, Socket socket)
        {
            if (server == null)
                throw new ArgumentNullException(nameof(server));
            if (socket == null)
                throw new ArgumentNullException(nameof(socket));

            _server = server;
            _socket = socket;
            _localEndPoint = socket.LocalEndPoint;
            _remoteEndPoint = socket.RemoteEndPoint;
            _networkManager = new NetworkManagerAsync(_socket, server.Settings, new MessageProcessorNaCl(Crypto8.StandardKeyPair));
            _networkManager.MessageReceived += OnMessage;
            _networkManager.Disconnected += OnDisconnected;
        }
        #endregion

        #region Fields & Properties
        public event EventHandler<DisconnectedEventArgs> Left;

        private bool _disposed;
        private readonly IServer _server;
        private readonly Socket _socket;
        private readonly EndPoint _localEndPoint;
        private readonly EndPoint _remoteEndPoint;
        private readonly NetworkManagerAsync _networkManager;

        public Socket Connection => _socket;
        public EndPoint LocalEndPoint => _remoteEndPoint;
        public EndPoint RemoteEndPoint => _remoteEndPoint;
        public IServer Server => _server;
        public ILevelSave Save => new LevelSave(Level);

        public Level Level { get; set; }
        public byte[] SessionKey { get; set; }
        #endregion

        #region Methods
        public void Disconnect()
        {
            Dispose();
        }

        public void SendMessage(Message message)
        {
            if (message == null)
                throw new ArgumentNullException(nameof(message));

            _networkManager.SendMessage(message);
        }

        public void Dispose()
        {
            if (_disposed)
                return;

            var now = DateTime.UtcNow;
            // Save the client when it disconnects.
            if (Level != null)
            {
                // Calculates at the tick at which the client disconnected
                // and do Tick on that calculated tick.
                const double TickDuration = (1d / 60d) * 1000d;
                var diffTime = now - Level.LastTick;
                var expectedTick = (int)(diffTime.TotalMilliseconds / TickDuration) + Level.LastTickValue;

                Level.Tick(expectedTick);

                // Now that all the VillageObject has been ticked an updated
                // we can push the Level back to the database.
                var save = Save;
                Server.Db.SaveLevel(save);

                // Push back VillageObjects to pool.
                Level.Dispose();
            }

            // Disconnect socket.
            _networkManager.Dispose();
            _disposed = true;
        }

        private void OnMessage(object sender, MessageReceivedEventArgs e)
        {
            if (e.Message == null)
                return;

            try
            {
                Server.Handler.Handle(this, e.Message);
            }
            catch (Exception ex)
            {
                Server.Log.Error($"Failed to handle {e.Message}: {ex}");
            }
        }

        private void OnDisconnected(object sender, DisconnectedEventArgs e)
        {
            try
            {
                var remoteEndPoint = RemoteEndPoint;

                Dispose();
                Server.Clients.Remove(this);

                Server.Log.Info($"Client at {remoteEndPoint} disconnected.");
            }
            catch (Exception ex)
            {
                Server.Log.Error("Failed to disconnect client: " + ex);
            }
        }
        #endregion
    }
}
