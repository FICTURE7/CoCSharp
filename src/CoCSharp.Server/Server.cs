using CoCSharp.Csv;
using CoCSharp.Data;
using CoCSharp.Data.Models;
using CoCSharp.Logic;
using CoCSharp.Network;
using CoCSharp.Server.API;
using CoCSharp.Server.API.Core;
using CoCSharp.Server.API.Events.Server;
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
            _config = new ServerConfiguration("config.xml");

            _assets = new AssetManager(ContentPath);

            _db = new LiteDbManager(this);
            _handler = new MessageHandler(this);

            _log = new Log();
            _log.Logger = new ConsoleLogger();
            _log.Logger.Next(new FileLogger());

            _listener = new Socket(SocketType.Stream, ProtocolType.Tcp);
            _acceptPool = new SocketAsyncEventArgsPool(8, AcceptCompleted);
            _settings = new NetworkManagerAsyncSettings(64, 64);
        }
        #endregion

        #region Fields & Properties
        public event EventHandler<EventArgs> Started;
        public event EventHandler<ServerConnectionEventArgs> Accepted;

        private bool _disposed;
        private readonly Log _log;
        private readonly LiteDbManager _db;
        private readonly AssetManager _assets;
        private readonly List<IClient> _clients;
        private readonly MessageHandler _handler;
        private readonly ServerConfiguration _config;
        private readonly NetworkManagerAsyncSettings _settings;

        internal NetworkManagerAsyncSettings Settings => _settings;

        public AssetManager Assets => _assets;
        public Log Log => _log;
        public IDbManager Db => _db;
        public IMessageHandler Handler => _handler;
        public ICollection<IClient> Clients => _clients;
        public IServerConfiguration Configuration => _config;
        #endregion

        #region Methods
        public void Start()
        {
            _assets.Load<CsvDataTable<BuildingData>>("buildings.csv");
            _assets.Load<CsvDataTable<ObstacleData>>("obstacles.csv");
            _assets.Load<CsvDataTable<TrapData>>("traps.csv");
            _assets.Load<CsvDataTable<DecorationData>>("decos.csv");

            StartListener();

            OnStarted(EventArgs.Empty);
        }

        public void Close()
        {
            Dispose();
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

        protected virtual void OnConnection(ServerConnectionEventArgs args)
        {
            Accepted?.Invoke(this, args);
        }
        #endregion
    }
}
