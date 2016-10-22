using CoCSharp.Data;
using CoCSharp.Network;
using CoCSharp.Server.API;
using CoCSharp.Server.API.Core;
using CoCSharp.Server.API.Logging;
using CoCSharp.Server.Core;
using System;
using System.Collections.Generic;
using System.Net.Sockets;

namespace CoCSharp.Server
{
    public partial class Server : IServer
    {
        #region Constants
        public const string ContentPath = "contents";
        #endregion

        #region Constructors
        public Server()
        {
            _clients = new List<IClient>();
            _assets = new AssetManager(ContentPath);

            _db = new LiteDbManager(this);

            // TODO: Figure out a better logger.
            _logger = new ConsoleLogger();
            _logger.Next(new FileLogger());

            _listener = new Socket(SocketType.Stream, ProtocolType.Tcp);
            _acceptPool = new SocketAsyncEventArgsPool(8, AcceptCompleted);
            _settings = new NetworkManagerAsyncSettings(64, 64);
        }
        #endregion

        #region Fields & Properties
        public event EventHandler<EventArgs> Started;

        private bool _disposed;
        private readonly Logger _logger;
        private readonly List<IClient> _clients;
        private readonly AssetManager _assets;
        private readonly LiteDbManager _db;
        private readonly NetworkManagerAsyncSettings _settings;

        internal NetworkManagerAsyncSettings Settings => _settings;

        public AssetManager Assets => _assets;
        public Logger Logger => _logger;
        public IDbManager Db => _db;
        public ICollection<IClient> Clients => _clients;
        #endregion

        #region Methods
        public void Start()
        {
            StartListener();

            OnStarted(EventArgs.Empty);
        }

        public void Close()
        {
            Dispose();
        }

        public void ProcessMessage(Message message)
        {
            if (message == null)
                throw new ArgumentNullException(nameof(message));

            // TODO: Handle
        }

        public void Dispose()
        {
            if (_disposed)
                return;

            // TODO: Might want to lock this operation.
            for (int i = 0; i < _clients.Count; i++)
                _clients[i].Disconnect();

            _listener.Close();
            _acceptPool.Dispose();
            _db.Dispose();
            _disposed = true;
        }

        protected virtual void OnStarted(EventArgs args)
        {
            Started?.Invoke(this, args);
        }
        #endregion
    }
}
