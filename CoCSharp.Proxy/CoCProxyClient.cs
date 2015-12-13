using CoCSharp.Networking;
using System;
using System.IO;
using System.Net.Sockets;

namespace CoCSharp.Proxy
{
    public class CoCProxyClient
    {
        public CoCProxyClient(Socket client, Socket server)
        {
            Client = new NetworkManagerAsync(client);
            Client.MessageReceived += ClientReceived;

            Server = new NetworkManagerAsync(server);
            Server.MessageReceived += ServerReceived;
        }

        public NetworkManagerAsync Client { get; private set; }
        public NetworkManagerAsync Server { get; private set; }

        private void ServerReceived(object sender, MessageReceivedEventArgs e)
        {
            //TODO: Make into e.MessageBytes instead.

            var fullmessage = new byte[e.MessageBody.Length + e.MessageHeader.Length];
            Buffer.BlockCopy(e.MessageHeader, 0, fullmessage, 0, e.MessageHeader.Length);
            Buffer.BlockCopy(e.MessageBody, 0, fullmessage, e.MessageHeader.Length, e.MessageBody.Length);

            Client.Connection.Send(fullmessage);
        }

        private void ClientReceived(object sender, MessageReceivedEventArgs e)
        {
            var fullmessage = new byte[e.MessageBody.Length + e.MessageHeader.Length];
            Buffer.BlockCopy(e.MessageHeader, 0, fullmessage, 0, e.MessageHeader.Length);
            Buffer.BlockCopy(e.MessageBody, 0, fullmessage, e.MessageHeader.Length, e.MessageBody.Length);

            Server.Connection.Send(fullmessage);
        }
    }
}
