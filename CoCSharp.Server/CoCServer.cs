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
    public delegate void CommandHandler(CoCServer server, AvatarClient client, Command command);

    public delegate void MessageHandler(CoCServer server, AvatarClient client, Message message);

    public partial class CoCServer
    {
        public CoCServer()
        {
            const int KEEPALIVE_TIIMEOUT = 25000;

            _settings = new NetworkManagerAsyncSettings(64, 64);
            _listener = new Socket(SocketType.Stream, ProtocolType.Tcp);
            _acceptPool = new SocketAsyncEventArgsPool(100, AcceptOperationCompleted);

            _timer = new Timer(TimerCallback, null, KEEPALIVE_TIIMEOUT, KEEPALIVE_TIIMEOUT);

            Console.WriteLine("-> loading config.xml");
            Configuration = new CoCServerConfiguration("config.xml");
            Console.WriteLine("-> setting up AvatarManager...");
            AvatarManager = new AvatarManager(this);

            Console.WriteLine("-> setting up AllianceManager...");
            AllianceManager = new AllianceManager();

            Console.WriteLine("-> setting up AssetManager...");
            AssetManager = new AssetManager(DirectoryPaths.Content);

            LoadCsv<BuildingData>("buildings.csv");
            LoadCsv<TrapData>("traps.csv");
            LoadCsv<ObstacleData>("obstacles.csv");
            LoadCsv<DecorationData>("decos.csv");
            LoadCsv<ResourceData>("resources.csv");

            AssetManager.DefaultInstance = AssetManager;

            Console.WriteLine("-> setting up NpcManager...");
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
            Console.Write("--> loading {0}...", path);
            AssetManager.LoadCsv<T>(path);
            Console.WriteLine("done");
        }

        public CoCServerConfiguration Configuration { get; private set; }
        public List<AvatarClient> Clients { get; private set; }

        public NpcManager NpcManager { get; private set; }
        public AvatarManager AvatarManager { get; private set; }
        public AllianceManager AllianceManager { get; private set; }

        public AssetManager AssetManager { get; private set; }

        private Dictionary<int, CommandHandler> CommandHandlerDictionary { get; set; }
        private Dictionary<ushort, MessageHandler> MessageHandlerDictionary { get; set; }

        private readonly NetworkManagerAsyncSettings _settings;
        private readonly Timer _timer;

        // Starts listening & handling clients async.
        public void Start()
        {
            StartListener();
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
                Console.WriteLine("Exception occurred while handling message: {0}\r\n\t{1}", message.GetType().Name, ex);
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
                Console.WriteLine("Exception occurred while handling command: {0}\r\n\t{1}", command.GetType().Name, ex);
            }
#endif
        }

        // Sends a Message to all connected clients.
        public void SendMessageAll(Message message)
        {
            for (int i = 0; i < Clients.Count; i++)
                Clients[i].NetworkManager.SendMessage(message);
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

            Console.WriteLine("log::activeconn/totalconn: {0}/{1}, sent/receive: {2}/{3} bytes, sent/receive: {4}/{5} messages",
                              activeConn, totalConn, totalSent, totalReceived, totalMsgSent, totalMsgReceived);
        }

        private void DoKeepAlive()
        {
            try
            {
                for (int i = 0; i < Clients.Count; i++)
                {
                    var client = Clients[i];
                    if (DateTime.UtcNow >= client.ExpirationKeepAlive)
                    {
                        var remoteEndPoint = client.NetworkManager.Socket.RemoteEndPoint;
                        client.InternalDisconnect(true);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("exception: error while checking keepalives: {0}", ex.Message);
            }
        }
    }
}
