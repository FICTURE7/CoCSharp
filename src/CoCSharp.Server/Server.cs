using CoCSharp.Csv;
using CoCSharp.Data;
using CoCSharp.Data.Models;
using CoCSharp.Network;
using CoCSharp.Server.API;
using CoCSharp.Server.API.Core;
using CoCSharp.Server.API.Events.Server;
using CoCSharp.Server.API.Logging;
using CoCSharp.Server.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;

namespace CoCSharp.Server
{
    public partial class Server : IServer
    {
        #region Constructors
        public Server()
        {
            const string CONTENT_PATH = "contents";
            const string CONFIG_PATH = "server_config.xml";

            // Initialize our loggers.
            _log = new Log();
            _log.MainLogger
                .Next(new ConsoleLogger())
                .Next(new FileLogger("logs"));

            _log.Info($"Loading config at '{CONFIG_PATH}'...");
            // Initialize our configs.
            _config = new ServerConfiguration();
            // If the config does not exists we create it.
            if (!File.Exists(CONFIG_PATH))
            {
                _log.Warn($"Config at '{CONFIG_PATH}' was not found; creating one.");
                _config.Save(CONFIG_PATH);
            }
            // If we couldn't load all of the configs or part of the config is missing
            // we overwrite it.
            else if (!_config.Load(CONFIG_PATH))
            {
                // Keep a backup of the old config.
                var oldName = CONFIG_PATH;
                var newName = Path.GetFileNameWithoutExtension(CONFIG_PATH) + "_old_" + DateTime.Now.ToString("dd-HH-mm-ss-ff") + Path.GetExtension(CONFIG_PATH);
                File.Move(oldName, newName);

                _log.Warn($"Was unable to load config at '{CONFIG_PATH}' completely; overwriting old one.");
                _config.Save(CONFIG_PATH);
            }

            // Initialize our client list.
            // TODO: Make thread safe.
            _clients = new List<IClient>();

            _assets = new AssetManager(CONTENT_PATH);

            _db = new LiteDbManager(this);
            _handler = new MessageHandler(this);

            _listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _acceptPool = new SocketAsyncEventArgsPool(8, AcceptCompleted);
            _settings = new NetworkManagerAsyncSettings(64, 64);
        }
        #endregion

        #region Fields & Properties
        public event EventHandler<EventArgs> Started;
        public event EventHandler<ServerConnectionEventArgs> ClientConnected;

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
            if (_disposed)
                throw new ObjectDisposedException(null, "Can't Start disposed Server object.");

            _assets.Load<CsvDataTable<BuildingData>>("logic/buildings.csv");
            _assets.Load<CsvDataTable<ObstacleData>>("logic/obstacles.csv");
            _assets.Load<CsvDataTable<TrapData>>("logic/traps.csv");
            _assets.Load<CsvDataTable<DecorationData>>("logic/decos.csv");

            if (!StartListener())
            {
                Log.Error("Listening socket failed to bind or listen.");
                Log.Error("Closing server...");
                Close();

                Environment.Exit(1);
            }

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

            StopListener();

            // TODO: Might want to lock this operation.
            for (int i = 0; i < _clients.Count; i++)
                _clients[i].Disconnect();

            _db.Dispose();
            _acceptPool.Dispose();
            _listener.Close();
            _log.Dispose();

            _disposed = true;
        }

        protected virtual void OnStarted(EventArgs args)
        {
            Started?.Invoke(this, args);
        }

        protected virtual void OnConnection(ServerConnectionEventArgs args)
        {
            ClientConnected?.Invoke(this, args);
        }
        #endregion
    }
}
