using CoCSharp.Logic;
using CoCSharp.Network;
using CoCSharp.Network.Cryptography;
using CoCSharp.Server.Api;
using CoCSharp.Server.Api.Core.Factories;
using CoCSharp.Server.Api.Db;
using CoCSharp.Server.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

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

            // Give client a 30 second window to send its first keep alive.
            // TODO: Use value from Assets to figure out how much seconds before timeout.
            LastKeepAliveTime = DateTime.UtcNow;
            KeepAliveExpireTime = DateTime.UtcNow.AddSeconds(30);

            _server = server;
            _socket = socket;
            _localEndPoint = socket.LocalEndPoint;
            _remoteEndPoint = socket.RemoteEndPoint;
            _session = new RemoteSession(server);
            _save = Server.Factories.GetFactory<LevelSaveFactory>().Create();

            _networkManager = new NetworkManagerAsync(_socket, server.Settings, new MessageProcessorNaCl(Crypto8.StandardKeyPair));
            _networkManager.MessageReceived += OnMessageReceived;
            _networkManager.MessageSent += OnMessageSent;
            _networkManager.Disconnected += OnDisconnected;
        }
        #endregion

        #region Fields & Properties
        private bool _disposed;
        private readonly IServer _server;
        private readonly LevelSave _save;
        private readonly Socket _socket;
        private readonly Session _session;
        private readonly EndPoint _localEndPoint;
        private readonly EndPoint _remoteEndPoint;
        private readonly NetworkManagerAsync _networkManager;

        internal bool _canChat;

        public Socket Connection => _socket;
        public EndPoint LocalEndPoint => _remoteEndPoint;
        public EndPoint RemoteEndPoint => _remoteEndPoint;
        public Session Session => _session;

        public IServer Server => _server;
        [Obsolete]
        public LevelSave Save
        {
            get
            {
                if (!_session.State.HasFlag(SessionState.LoggedIn))
                    return null;

                _save.FromLevel(_session.Level);
                return _save;
            }
        }

        public DateTime LastKeepAliveTime { get; set; }
        public DateTime KeepAliveExpireTime { get; set; }
        #endregion

        #region Methods
        public void SendMessage(Message message)
        {
            if (message == null)
                throw new ArgumentNullException(nameof(message));

            _networkManager.SendMessage(message);
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                // Make sure we don't handle incoming messages when disconnecting.
                _networkManager.MessageReceived -= OnMessageReceived;

                var now = DateTime.UtcNow;
                var level = _session.Level;
                // Save the client when it disconnects.
                if (level != null)
                {
                    // Calculates at the tick at which the client disconnected
                    // and do Tick on that calculated tick.
                    const double TickDuration = (1d / 60d) * 1000d;
                    var diffTime = now - level.LastTickTime;
                    var expectedTick = (int)(diffTime.TotalMilliseconds / TickDuration) + level.LastTickValue;

                    level.Tick(expectedTick);

                    // Now that all the VillageObject has been ticked and updated
                    // we can push the Level back to the database.
                    var save = Save;
                    Server.Db.SaveLevelAsync(save, new CancellationToken()); // TODO: Wait for save when disposing IDbManager.

                    // Push back VillageObjects to pool.
                    //level.Dispose();
                }

                // Disconnect socket.
                _networkManager.Dispose();

                // Remove the client reference in the client list.
                _server.Clients.Remove(this);
            }
            _disposed = true;
        }

        private readonly List<Task> _tasks = new List<Task>();

        private async void OnMessageReceived(object sender, MessageReceivedEventArgs e)
        {
            if (e.Message == null)
                return;

            Server.Logs.Info($"Received & Processing {e.Message.GetType().Name}...");
            try
            {
                // Ask the server to handle the message received on this client connection.
                await Server.Handler.HandleAsync(this, e.Message);
            }
            catch (Exception ex)
            {
                Server.Logs.Error($"Failed to handle {e.Message}: {ex}");

                Server.Logs.GetLogger<CleanErrorLogger>().Log(ex.GetType().Name + ":" + ex.Message);
            }
            Server.Logs.Info($"Processing {e.Message.GetType().Name}...done!");
        }

        private void OnMessageSent(object sender, MessageSentEventArgs e)
        {
            Server.Logs.Info($"Sent {e.Message.GetType().Name}");
        }

        private void OnDisconnected(object sender, DisconnectedEventArgs e)
        {
            try
            {
                var remoteEndPoint = RemoteEndPoint;

                Dispose();
                Server.Logs.Info($"Client at {remoteEndPoint} disconnected.");
            }
            catch (Exception ex)
            {
                Server.Logs.Error("Failed to disconnect client: " + ex);

                Server.Logs.GetLogger<CleanErrorLogger>().Log(ex.GetType().Name + ":" + ex.Message);
            }
        }
        #endregion
    }
}
