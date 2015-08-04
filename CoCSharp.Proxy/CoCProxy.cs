using CoCSharp.Data;
using CoCSharp.Logging;
using CoCSharp.Networking;
using CoCSharp.Networking.Packets;
using CoCSharp.Proxy.Handlers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace CoCSharp.Proxy
{
    public class CoCProxy : ICoCServer
    {
        public delegate void PacketHandler(CoCProxy proxyServer, CoCProxyConnection client, IPacket packet);

        public CoCProxy()
        {
            Loggers = new List<ILogger>();
            ProxyConnections = new List<CoCProxyConnection>();
            PacketHandlers = new Dictionary<ushort, PacketHandler>();
            AcceptEventPool = new SocketAsyncEventArgsPool(100);

            ProxyPacketHandlers.RegisterHanlders(this);
        }

        public string ServerAddress { get; set; }
        public int ServerPort { get; set; }
        public List<ILogger> Loggers { get; set; }
        public List<CoCProxyConnection> ProxyConnections { get; set; }
        public Dictionary<ushort, PacketHandler> PacketHandlers { get; set; }
        public Socket Listener { get; set; }
        public EndPoint EndPoint { get; set; }

        private bool ShuttingDown { get; set; }
        private SocketAsyncEventArgsPool AcceptEventPool { get; set; }

        public void Start(EndPoint endPoint)
        {
            ShuttingDown = false;
            EndPoint = endPoint;

            Listener = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            Listener.Bind(endPoint);
            Listener.Listen(200);
            StartListen();
        }

        public void Stop()
        {
            ShuttingDown = true;
            if (Listener != null)
                Listener.Close();
        }

        public void RegisterPacketHandler(IPacket packet, PacketHandler handler)
        {
            PacketHandlers.Add(packet.ID, handler);
        }

        public void Log(params object[] parameters)
        {
            //TODO: Implement.
            throw new NotImplementedException();
        }

        private void HandlePacket(CoCProxyConnection client, IPacket packet)
        {
            var handler = (PacketHandler)null;
            if (!PacketHandlers.TryGetValue(packet.ID, out handler))
                return; // throw exception?
            handler(this, client, packet);
        }

        private void StartListen()
        {
            while (AcceptEventPool.Count > 1)
            {
                var acceptEvent = AcceptEventPool.Pop();
                acceptEvent.Completed += AcceptAsyncCompleted;

                if (!Listener.AcceptAsync(acceptEvent))
                    AcceptAsyncCompleted(Listener, acceptEvent);
            }
        }

        private void AcceptAsyncCompleted(object sender, SocketAsyncEventArgs args)
        {
            args.Completed -= AcceptAsyncCompleted;

            switch (args.LastOperation) // TODO: Check for errors
            {
                case SocketAsyncOperation.Accept:
                    var connection = args.AcceptSocket;
                    var remoteClient = new CoCProxyConnection(this, connection);
                    
                    ProxyConnections.Add(remoteClient);               
                    args.AcceptSocket = null;
                    AcceptEventPool.Push(args);
                    StartListen();
                    break;
            }
        }
    }
}
