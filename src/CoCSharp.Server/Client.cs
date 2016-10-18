using System;
using System.Net.Sockets;
using CoCSharp.Logic;
using CoCSharp.Network;
using CoCSharp.Server.API;

namespace CoCSharp.Server
{
    public class Client : IClient
    {
        public Client(OldServer server)
        {
            if (server == null)
                throw new ArgumentNullException(nameof(server));

            _server = server;
        }

        private readonly OldServer _server;

        public IServer Server => _server;


        public Socket Connection
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public Level Level
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public event EventHandler<DisconnectedEventArgs> Disconnected;
        public event EventHandler<MessageReceivedEventArgs> MessageReceived;

        public void Disconnect()
        {
            throw new NotImplementedException();
        }

        public void SendMessage(Message message)
        {
            throw new NotImplementedException();
        }
    }
}
