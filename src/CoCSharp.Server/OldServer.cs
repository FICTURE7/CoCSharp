using CoCSharp.Csv;
using CoCSharp.Data;
using CoCSharp.Data.Models;
using CoCSharp.Network;
using CoCSharp.Server.API;
using CoCSharp.Server.API.Core;
using CoCSharp.Server.Core;
using CoCSharp.Server.Handlers;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;

namespace CoCSharp.Server
{
    public delegate void MessageHandler(OldServer server, AvatarClient client, Message message);

    public partial class OldServer : IServer
    {
        public OldServer()
        {
            Log.FullException = true;

            _settings = new NetworkManagerAsyncSettings(64, 64);
            _listener = new Socket(SocketType.Stream, ProtocolType.Tcp);
            _acceptPool = new SocketAsyncEventArgsPool(8, AcceptOperationCompleted);

            Console.WriteLine("> Loading config.xml");
            Configuration = new ServerConfiguration("config.xml");

            Console.WriteLine("> Setting up AvatarManager...");
            AvatarManager = new AvatarManager(this);

            Console.WriteLine("> Setting up AllianceManager...");
            AllianceManager = new AllianceManager();

            Console.WriteLine("> Setting up AssetManager...\n");
            AssetManager = new AssetManager(DirectoryPaths.Content);

            LoadCsv<ResourceData>("resources.csv");
            LoadCsv<BuildingData>("buildings.csv");
            LoadCsv<TrapData>("traps.csv");
            LoadCsv<ObstacleData>("obstacles.csv");
            LoadCsv<DecorationData>("decos.csv");

            AssetManager.Default = AssetManager;

            Console.WriteLine("\n> Setting up NpcManager...");
            NpcManager = new NpcManager();

            Clients = new List<AvatarClient>();
            MessageHandlerDictionary = new Dictionary<ushort, MessageHandler>();

            LoginMessageHandlers.RegisterLoginMessageHandlers(this);
            InGameMessageHandlers.RegisterInGameMessageHandlers(this);
            AllianceMessageHandlers.RegisterAllianceMessageHandlers(this);
        }

        private void LoadCsv<T>(string path) where T : CsvData, new()
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write("-> Loading {0}...", path);
            Console.ResetColor();

            AssetManager.Load<CsvDataTable<T>>(path);

            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine(" done");
            Console.ResetColor();
        }

        public ServerConfiguration Configuration { get; private set; }
        public List<AvatarClient> Clients { get; private set; }

        public NpcManager NpcManager { get; private set; }
        public AvatarManager AvatarManager { get; private set; }
        public AllianceManager AllianceManager { get; private set; }
        public AssetManager AssetManager { get; private set; }

        private Dictionary<ushort, MessageHandler> MessageHandlerDictionary { get; set; }

        public IDbManager DbManager
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        private readonly NetworkManagerAsyncSettings _settings;
        private Timer _timer;

        // Starts listening & handling clients async.
        public void Start()
        {
            const int KEEPALIVE_TIIMEOUT = 25000;

            StartListener();
            _timer = new Timer(TimerCallback, null, KEEPALIVE_TIIMEOUT, KEEPALIVE_TIIMEOUT);
        }

        public void Close()
        {
            //TODO: Clean up.
        }

        // Registers the specified MessageHandler for the specific Message.
        public void RegisterMessageHandler(Message message, MessageHandler handler)
        {
            if (message == null)
                throw new ArgumentNullException(nameof(message));
            if (handler == null)
                throw new ArgumentNullException(nameof(handler));
            if (MessageHandlerDictionary.ContainsKey(message.ID))
                throw new ArgumentException("Already contain handler for message '" + message.ID + "'.", "message");

            MessageHandlerDictionary.Add(message.ID, handler);
        }

        // Tries to handles the specified Message with the registered MessageHandlers.
        public void HandleMessage(AvatarClient client, Message message)
        {
            try
            {
                var handler = (MessageHandler)null;
                if (MessageHandlerDictionary.TryGetValue(message.ID, out handler))
                {
                    handler(this, client, message);
                }
            }
            catch (Exception ex)
            {
                Log.Exception("Failed to handle message", ex);
            }
        }

        // Sends a Message to all connected clients.
        public void SendMessageAll(Message message)
        {
            for (int i = 0; i < Clients.Count; i++)
                Clients[i].SendMessage(message);
        }

        // Called by _timer to check whether KeepAlives has expired
        // and to provide some logs.
        private void TimerCallback(object state)
        {
            DoKeepAlive();

            var activeConn = Clients.Count; // <- Can be wrong some times.
            var totalConn = Thread.VolatileRead(ref _totalConnection);
            var totalSent = _settings.Statistics.TotalByteSent;
            var totalReceived = _settings.Statistics.TotalByteReceived;
            var totalMsgSent = _settings.Statistics.TotalMessagesSent;
            var totalMsgReceived = _settings.Statistics.TotalMessagesReceived;

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("LOG: Active Connections/Total Connections: {0}/{1}, Sent/Received: {2}/{3} bytes, Sent/Received: {4}/{5} messages.",
                              activeConn, totalConn, totalSent, totalReceived, totalMsgSent, totalMsgReceived);
            Console.ResetColor();
        }

        private void DoKeepAlive()
        {
            try
            {
                for (int i = 0; i < Clients.Count; i++)
                {
                    var client = Clients[i]; // <- Can be null. 
                    if (DateTime.UtcNow >= client.ExpirationKeepAlive)
                        client.InternalDisconnect(true);
                }
            }
            catch (Exception ex)
            {
                Log.Exception("Failed to check keepalives", ex);
            }
        }
    }
}
