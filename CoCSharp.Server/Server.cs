using CoCSharp.Csv;
using CoCSharp.Data;
using CoCSharp.Data.Models;
using CoCSharp.Network;
using CoCSharp.Server.Core;
using CoCSharp.Server.Handlers;
using CoCSharp.Server.Handlers.Commands;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;

namespace CoCSharp.Server
{
    public delegate void CommandHandler(Server server, AvatarClient client, Command command);

    public delegate void MessageHandler(Server server, AvatarClient client, Message message);

    public partial class Server
    {
        public Server()
        {
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

            LoadCsv<BuildingData>("buildings.csv");
            LoadCsv<TrapData>("traps.csv");
            LoadCsv<ObstacleData>("obstacles.csv");
            LoadCsv<DecorationData>("decos.csv");
            LoadCsv<ResourceData>("resources.csv");

            AssetManager.DefaultInstance = AssetManager;

            Console.WriteLine("\n> Setting up NpcManager...");
            NpcManager = new NpcManager();

            Clients = new List<AvatarClient>();
            MessageHandlerDictionary = new Dictionary<ushort, MessageHandler>();
            CommandHandlerDictionary = new Dictionary<int, CommandHandler>();

            LoginMessageHandlers.RegisterLoginMessageHandlers(this);
            InGameMessageHandlers.RegisterInGameMessageHandlers(this);
            AllianceMessageHandlers.RegisterAllianceMessageHandlers(this);

            CommandHandlers.RegisterCommandHandlers(this);
        }

        private void LoadCsv<T>(string path) where T : CsvData, new()
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write("-> Loading {0}...", path);
            Console.ResetColor();
            AssetManager.LoadCsv<T>(path);
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine(" DONE");
            Console.ResetColor();
        }

        public ServerConfiguration Configuration { get; private set; }
        public List<AvatarClient> Clients { get; private set; }

        public NpcManager NpcManager { get; private set; }
        public AvatarManager AvatarManager { get; private set; }
        public AllianceManager AllianceManager { get; private set; }

        public AssetManager AssetManager { get; private set; }

        private Dictionary<int, CommandHandler> CommandHandlerDictionary { get; set; }
        private Dictionary<ushort, MessageHandler> MessageHandlerDictionary { get; set; }

        private readonly NetworkManagerAsyncSettings _settings;
        private Timer _timer;

        // Starts listening & handling clients async.
        public void Start()
        {
            const int KEEPALIVE_TIIMEOUT = 25000;

            StartListener();
            _timer = new Timer(TimerCallback, null, KEEPALIVE_TIIMEOUT, KEEPALIVE_TIIMEOUT);
        }

        // Registers the specified MessageHandler for the specific Message.
        public void RegisterMessageHandler(Message message, MessageHandler handler)
        {
            if (message == null)
                throw new ArgumentNullException("message");
            if (handler == null)
                throw new ArgumentNullException("handler");
            if (MessageHandlerDictionary.ContainsKey(message.ID))
                throw new ArgumentException("Already contain handler for message '" + message.ID + "'.", "message");

            MessageHandlerDictionary.Add(message.ID, handler);
        }

        // Registers the specified CommandHandler for the specific Command.
        public void RegisterCommandHandler(Command command, CommandHandler handler)
        {
            if (command == null)
                throw new ArgumentNullException("message");
            if (handler == null)
                throw new ArgumentNullException("handler");
            if (CommandHandlerDictionary.ContainsKey(command.ID))
                throw new ArgumentException("Already contain handler for message '" + command.ID + "'.", "message");

            CommandHandlerDictionary.Add(command.ID, handler);
        }

        // Tries to handles the specified Message with the registered MessageHandlers.
        public void HandleMessage(AvatarClient client, Message message)
        {
#if !DEBUG
            try
            {
#endif
                var handler = (MessageHandler)null;
                if (MessageHandlerDictionary.TryGetValue(message.ID, out handler))
                {
                    handler(this, client, message);
                }
#if !DEBUG
            }
            catch (Exception ex)
            {
                Log.Exception("Failed to handle message", ex);
            }
#endif
        }

        // Tries to handles the specified Command with the registered CommandHandlers.
        public void HandleCommand(AvatarClient client, Command command)
        {
#if !DEBUG
            try
            {
#endif
                var handler = (CommandHandler)null;
                if (CommandHandlerDictionary.TryGetValue(command.ID, out handler))
                {
                    handler(this, client, command);
                }
#if !DEBUG
            }
            catch (Exception ex)
            {
                Log.Exception("Failed to handle command", ex);
            }
#endif
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
            Console.WriteLine("LOG: Active Connections / Total Connections: {0} / {1}, Sent / Received: {2} / {3} bytes, Sent / Received: {4} / {5} messages.",
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
