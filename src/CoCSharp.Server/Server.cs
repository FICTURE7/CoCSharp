using CoCSharp.Csv;
using CoCSharp.Data;
using CoCSharp.Data.Models;
using CoCSharp.Network;
using CoCSharp.Server.Api;
using CoCSharp.Server.Api.Core;
using CoCSharp.Server.Api.Core.Factories;
using CoCSharp.Server.Api.Db;
using CoCSharp.Server.Api.Events.Server;
using CoCSharp.Server.Api.Logging;
using CoCSharp.Server.Core;
using CoCSharp.Server.Db;
using CoCSharp.Server.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using System.Threading;

namespace CoCSharp.Server
{
    public partial class Server : IServer
    {
        #region Constructors
        public Server()
        {
            const string LOG_DIR_PATH = "logs";
            const string CONFIG_FILE_PATH = "server_config.xml";

            // Initialize our loggers.
            _logs = new Logs(LOG_DIR_PATH);
            _logs.RegisterLogger<ClanLogger>();
            _logs.RegisterLogger<CleanErrorLogger>();
            _logs.Info($"Loading config at '{CONFIG_FILE_PATH}'...");

            // Initialize our configs.
            _config = new ServerConfiguration();
            // If the config does not exists we create it.
            if (!File.Exists(CONFIG_FILE_PATH))
            {
                _logs.Warn($"Config at '{CONFIG_FILE_PATH}' was not found; creating one.");
                _config.Save(CONFIG_FILE_PATH);
            }
            // If we couldn't load all of the configs or part of the config is missing
            // we overwrite it.
            else if (!_config.Load(CONFIG_FILE_PATH))
            {
                // Keep a backup of the old config.
                var oldName = CONFIG_FILE_PATH;
                var newName = $"{Path.GetFileNameWithoutExtension(CONFIG_FILE_PATH)}_old_{DateTime.Now.ToString("dd-HH-mm-ss-ff")}{Path.GetExtension(CONFIG_FILE_PATH)}";
                File.Move(oldName, newName);

                _logs.Warn($"Was unable to load config at '{CONFIG_FILE_PATH}' completely; overwriting old one.");
                _config.Save(CONFIG_FILE_PATH);
            }

            // Check whether the values in server configuration is valid.
            if (!CheckConfig())
            {
                _logs.Error("Server configuration was incorrect.");
                Close();

                Environment.Exit(1);
            }

            // Initialize our thread-safe client list.
            _clients = new ClientCollection();
            _cache = new CacheManager();

            _factories = new FactoryManager(this);
            _factories.RegisterFactory<LevelSaveFactory>();
            _factories.RegisterFactory<ClanSaveFactory>();

            // Initialize the Web API.
            // TODO: Turn into plugin when the plugin system is set up.
            _webApi = new WebApi(this);

            _levels = new LevelManager(this);
            _clans = new ClanManager(this);
            // Initialize our message handler, to handle incoming messages.
            _handler = new MessageHandler(this);

            _heartbeat = new Timer(DoHeartbeat, null, 5000, 5000);
            _listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _acceptPool = new SocketAsyncEventArgsPool(8, AcceptCompleted);
            _settings = new NetworkManagerAsyncSettings(64, 64, 32);
        }
        #endregion

        #region Fields & Properties
        public event EventHandler<ServerConnectionEventArgs> ClientConnected;

        private bool _disposed;
        private MySqlDbManager _db;
        private AssetManager _assets;

        //TODO: Turn into a plugin instead when plugin system is ready.
        private readonly WebApi _webApi;

        private readonly Logs _logs;
        private readonly Timer _heartbeat;
        private readonly CacheManager _cache;
        private readonly LevelManager _levels;
        private readonly ClanManager _clans;
        private readonly MessageHandler _handler;
        private readonly FactoryManager _factories;
        private readonly ClientCollection _clients;
        private readonly ServerConfiguration _config;
        private readonly NetworkManagerAsyncSettings _settings;

        internal NetworkManagerAsyncSettings Settings => _settings;

        public AssetManager Assets => _assets;
        public Logs Logs => _logs;
        public IDbManager Db => _db;
        public IClanManager Clans => _clans;
        public ILevelManager Levels => _levels;
        public ICacheManager Cache => _cache;
        public IMessageHandler Handler => _handler;
        public IFactoryManager Factories => _factories;
        public ICollection<IClient> Clients => _clients;
        public IServerConfiguration Configuration => _config;
        #endregion

        #region Methods
        public void Start()
        {
            if (_disposed)
                throw new ObjectDisposedException(null, "Can't Start disposed Server object.");

            const string CONTENT_DIR_PATH = "contents";

            if (Configuration.SynchronizeAssets)
            {
                try
                {
                    // Sync assets with the assets available on the asset server.
                    SyncAssets(CONTENT_DIR_PATH);
                }
                catch (Exception ex)
                {
                    Logs.Error("Unable to synchronize assets with asset server.");
                    Logs.Error(ex.ToString());
                    Close();

                    Environment.Exit(1);
                }
            }

            // After we've been synced up with the asset server we can load
            // the assets.
            _assets = new AssetManager(CONTENT_DIR_PATH);

            LoadCsvDataTable<BuildingData>("logic/buildings.csv");
            LoadCsvDataTable<ObstacleData>("logic/obstacles.csv");
            LoadCsvDataTable<TrapData>("logic/traps.csv");
            LoadCsvDataTable<DecorationData>("logic/decos.csv");
            LoadCsvDataTable<ResourceData>("logic/resources.csv");
            LoadCsvDataTable<GlobalData>("logic/globals.csv");
            LoadCsvDataTable<CharacterData>("logic/characters.csv");
            LoadCsvDataTable<ExperienceLevelData>("logic/experience_levels.csv");

            // Lock the AssetManager to prevent unloading of assets.
            _assets.Lock(AssetManagerLockMode.Both);

            try
            {
                _db = new MySqlDbManager(this);
            }
            catch (Exception ex)
            {
                Logs.Error($"Unable to start IDbManager instance: {ex}.");
                Environment.Exit(1);
            }

            if (!StartListener())
            {
                Logs.Error("Listening socket failed to bind or listen.");
                Logs.Error("Closing server...");
                Close();

                Environment.Exit(1);
            }

            Logs.Info("Starting web API...");

            try
            {
                _webApi.Start();
            }
            catch (Exception ex)
            {
                Logs.Error($"Unable to start web API, {ex.Message}.");
            }
        }

        public void Close()
        {
            Dispose();
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
                StopListener();

                if (_clients != null)
                {
                    // Disconnects all clients.
                    foreach (var c in _clients)
                        c.Dispose();
                }

                _heartbeat?.Dispose();
                _assets?.Dispose();
                _acceptPool?.Dispose();
                _listener?.Close();
                _logs?.Dispose();
                _db?.Dispose();
            }

            _disposed = true;
        }

        private void DoHeartbeat(object state)
        {
            try
            {
                // Look for clients who's keep-alives have been expired.
                foreach (var c in Clients)
                {
                    // Remove and disconnect clients whose keep alive has been expired.
                    if (DateTime.UtcNow >= c.KeepAliveExpireTime)
                    {
                        try
                        {
                            c.Dispose();
                        }
                        catch (Exception ex)
                        {
                            Logs.Error($"Failed to disconnect client: {ex}");

                            // Try to remove refs to it.
                            Clients.Remove(c);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logs.Error($"DoHeartbeat failed: {ex}");
            }
        }

        private void SyncAssets(string contentPath)
        {
            var sw = Stopwatch.StartNew();
            // Synchronize the assets with the asset server.
            using (var downloader = new AssetDownloader(_config.MasterHash, new Uri(_config.ContentUrl)))
            {
                downloader.DownloadProgressChanged += (sender, e) =>
                Logs.Info(!e.WasDownloaded ? $"Synced asset '{e.FileDownloaded.Path}'... {Math.Round(e.ProgressPercentage, 2)}%" :
                                            $"Synced asset & downloaded '{e.FileDownloaded.Path}'... {Math.Round(e.ProgressPercentage, 2)}% ");
                downloader.DownloadCompleted += (sender, e) => Logs.Info($"Syncing completed in {sw.Elapsed.TotalMilliseconds}ms.");

                downloader.DownloadAssets(contentPath);
            }
        }

        private void LoadCsvDataTable<T>(string path) where T : CsvData, new()
        {
            Logs.Info($"Loading CsvDataTable at '{path}' into AssetManager...");
            _assets.Load<CsvDataTable<T>>(path);
        }

        protected virtual void OnConnection(ServerConnectionEventArgs args)
        {
            ClientConnected?.Invoke(this, args);
        }

        private bool CheckConfig()
        {
            var result = true;
            var errorList = new List<string>();

            if (_config.MasterHash.Length != 40)
            {
                errorList.Add("MasterHash in server configuration must be 40 characters long.");
                result = false;
            }
            if (!IsValidHexString(_config.MasterHash))
            {
                errorList.Add("MasterHash in server configuration contains invalid hex characters.");
                result = false;
            }
            if (!Uri.IsWellFormedUriString(_config.ContentUrl, UriKind.Absolute))
            {
                errorList.Add("ContentUrl in server configuration must be a valid URL.");
                result = false;
            }

            foreach (var err in errorList)
                Logs.Error(err);

            return result;
        }

        public static bool IsValidHexString(string value)
        {
            const string VALID_HEX_CHARS = "0123456789abcdef";
            if (value.Length == 0)
                return true;

            for (int i = 0; i < value.Length; i++)
            {
                if (!VALID_HEX_CHARS.Contains(value[i].ToString()))
                    return false;
            }
            return true;
        }
        #endregion
    }
}
