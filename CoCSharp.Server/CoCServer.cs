using CoCSharp.Networking;
using CoCSharp.Server.Core;
using CoCSharp.Server.Handlers;
using CoCSharp.Server.Handlers.Commands;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace CoCSharp.Server
{
    public delegate void CommandHandler(CoCServer server, CoCRemoteClient client, Command command);

    public delegate void MessageHandler(CoCServer server, CoCRemoteClient client, Message message);

    public class CoCServer
    {
        public CoCServer()
        {
            _settings = new NetworkManagerAsyncSettings(50, 50);
            _listener = new Socket(SocketType.Stream, ProtocolType.Tcp);
            _acceptPool = new SocketAsyncEventArgsPool(100);

            AvatarManager = new AvatarManager();
            DataManager = new DataManager(); // This bad boi is hitting start performance hard. Need to rework the Csv implementation.
            Clients = new List<CoCRemoteClient>();
            MessageHandlerDictionary = new Dictionary<ushort, MessageHandler>();
            CommandHandlerDictionary = new Dictionary<int, CommandHandler>();

            LoginMessageHandlers.RegisterLoginMessageHandlers(this);
            InGameMessageHandlers.RegisterInGameMessageHandlers(this);

            CommandHandlers.RegisterCommandHandlers(this);
        }

        public AvatarManager AvatarManager { get; private set; }
        public DataManager DataManager { get; private set; }
        private Dictionary<int, CommandHandler> CommandHandlerDictionary { get; set; }
        private Dictionary<ushort, MessageHandler> MessageHandlerDictionary { get; set; }
        public List<CoCRemoteClient> Clients { get; private set; }

        private readonly Socket _listener;
        private readonly SocketAsyncEventArgsPool _acceptPool;
        private readonly NetworkManagerAsyncSettings _settings;

        public void Start()
        {
            _listener.Bind(new IPEndPoint(IPAddress.Any, 9339));
            _listener.Listen(100);
            StartAccept();
        }

        public void RegisterMessageHandler(Message message, MessageHandler handler)
        {
            MessageHandlerDictionary.Add(message.ID, handler);
        }

        public void RegisterCommandHandler(Command command, CommandHandler handler)
        {
            CommandHandlerDictionary.Add(command.ID, handler);
        }

        public void HandleMessage(CoCRemoteClient client, Message message)
        {
            try
            {
                var handler = (MessageHandler)null;
                if (MessageHandlerDictionary.TryGetValue(message.ID, out handler))
                    handler(this, client, message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception occurred while handling message: {0}\r\n\t{1}", message.GetType().Name, ex);
            }
        }

        public void HandleCommand(CoCRemoteClient client, Command command)
        {
            try
            {
                var handler = (CommandHandler)null;
                if (CommandHandlerDictionary.TryGetValue(command.ID, out handler))
                    handler(this, client, command);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception occurred while handling command: {0}\r\n\t{1}", command.GetType().Name, ex);
            }
        }

        public void SendMessageAll(Message message)
        {
            for (int i = 0; i < Clients.Count; i++)
                Clients[i].NetworkManager.SendMessage(message);
        }

        private void StartAccept()
        {
            var acceptArgs = (SocketAsyncEventArgs)null;
            if (_acceptPool.Count > 1)
            {
                try
                {
                    acceptArgs = _acceptPool.Pop();
                    acceptArgs.Completed += AcceptOperationCompleted;
                }
                catch
                {
                    acceptArgs = CreateNewAcceptArgs();
                }
            }
            else
            {
                acceptArgs = CreateNewAcceptArgs();
            }

            if (!_listener.AcceptAsync(acceptArgs))
                ProcessAccept(acceptArgs);
        }

        private SocketAsyncEventArgs CreateNewAcceptArgs()
        {
            var args = new SocketAsyncEventArgs();
            args.Completed += AcceptOperationCompleted;
            return args;
        }

        private void ProcessAccept(SocketAsyncEventArgs args)
        {
            args.Completed -= AcceptOperationCompleted;
            if (args.SocketError != SocketError.Success)
            {
                StartAccept(); // start accept asap
                ProcessBadAccept(args);
            }

            StartAccept(); // start accept asap

            FancyConsole.WriteLine("[&(darkyellow)Listener&(default)] -> New connection accepted: &(darkcyan){0}&(default).",
                                   args.AcceptSocket.RemoteEndPoint);
            Clients.Add(new CoCRemoteClient(this, args.AcceptSocket, _settings));
            args.AcceptSocket = null;
            _acceptPool.Push(args);
        }

        private void ProcessBadAccept(SocketAsyncEventArgs args)
        {
            args.AcceptSocket.Close();
            _acceptPool.Push(args);
        }

        private void AcceptOperationCompleted(object sender, SocketAsyncEventArgs e)
        {
            ProcessAccept(e);
        }
    }
}
