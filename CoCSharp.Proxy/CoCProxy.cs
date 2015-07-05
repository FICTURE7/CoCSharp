using CoCSharp.Databases;
using CoCSharp.Networking;
using CoCSharp.Networking.Packets;
using CoCSharp.Proxy.Handlers;
using CoCSharp.Proxy.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace CoCSharp.Proxy
{
    public class CoCProxy : CoCServer
    {
        public const int DefaultPort = 9339;
        public const string DefaultServer = "gamea.clashofclans.com";

        public delegate void PacketHandler(CoCProxy proxyServer, CoCProxyClient client, IPacket packet);

        public CoCProxy()
        {
            this.PacketLogger = new PacketLogger();
            this.PacketDumper = new PacketDumper();
            this.Clients = new List<CoCProxyClient>();
            this.PacketHandlers = new Dictionary<ushort, PacketHandler>();
            this.DatabaseManagers = new Dictionary<string, DatabaseManager>();
            this.AcceptAsyncEventPool = new Stack<SocketAsyncEventArgs>();
            for (int i = 0; i < 10; i++)
            {
                var acceptEvent = new SocketAsyncEventArgs();
                acceptEvent.Completed += AsyncOperationCompleted;
                AcceptAsyncEventPool.Push(acceptEvent);
            }

            RegisterDownloadedDatabases();
            ProxyPacketHandlers.RegisterHanlders(this);
        }

        public string ServerAddress { get; set; }
        public int ServerPort { get; set; }
        public PacketLogger PacketLogger { get; set; }
        public PacketDumper PacketDumper { get; set; }
        public List<CoCProxyClient> Clients { get; set; }
        public Dictionary<ushort, PacketHandler> PacketHandlers { get; set; }
        public Dictionary<string, DatabaseManager> DatabaseManagers { get; set; }
        public Socket Listener { get; set; }
        public IPEndPoint EndPoint { get; set; }

        private bool ShuttingDown { get; set; }
        private Thread NetworkThread { get; set; }
        private Stack<SocketAsyncEventArgs> AcceptAsyncEventPool { get; set; }

        public void Start(IPEndPoint endPoint)
        {
            ShuttingDown = false;
            EndPoint = endPoint;
            NetworkThread = new Thread(HandleNetwork);

            Listener = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            Listener.Bind(endPoint);
            Listener.Listen(100);

            NetworkThread.Name = "NetworkThread";
            NetworkThread.Start();
        }

        public void Stop()
        {
            if (NetworkThread != null)
            {
                ShuttingDown = true;
                NetworkThread.Abort();
            }

            if (Listener != null)
                Listener.Close();
        }

        public void RegisterPacketHandler(IPacket packet, PacketHandler handler)
        {
            PacketHandlers.Add(packet.ID, handler);
        }

        public void RegisterDatabaseManager(DatabaseManager manager, string hash)
        {
            DatabaseManagers.Add(hash, manager);
        }

        private void HandlePacket(CoCProxyClient client, IPacket packet)
        {
            var handler = (PacketHandler)null;
            if (!PacketHandlers.TryGetValue(packet.ID, out handler)) return;
            handler(this, client, packet);
        }

        private void HandleNetwork()
        {
            while (true)
            {
                if (ShuttingDown) return;

                while (AcceptAsyncEventPool.Count > 1) // make use of 9 of the aysnc objs
                {
                    var acceptEvent = AcceptAsyncEventPool.Pop();
                    var willRaiseEvent = Listener.AcceptAsync(acceptEvent);

                    if (!willRaiseEvent)
                        HandleAcceptOperation(acceptEvent);
                }

                for (int i = 0; i < Clients.Count; i++)
                {
                    //TODO: Kick client due to keep alive timeouts.
                    while (Clients[i].Client.NetworkManager.DataAvailable)
                    {
                        try
                        {
                            /* S <- P <- C
                             * Proxying data from client to server.
                             */

                            var rawPacket = (byte[])null; // raw encrypted packet
                            var decryptedPacket = (byte[])null;
                            var packet = Clients[i].Client.NetworkManager.ReadPacket(out rawPacket, out decryptedPacket); // receive data from client

                            if (packet != null) HandlePacket(Clients[i], packet);

                            if (rawPacket != null && packet != null)
                            {
                                Clients[i].Server.NetworkManager.CoCStream.Write(rawPacket, 0, rawPacket.Length); // sends data back to server
                                PacketLogger.LogPacket(packet, PacketDirection.Server);
                                PacketDumper.LogPacket(packet, PacketDirection.Server, decryptedPacket);
                            }
                        }
                        catch (SocketException ex)
                        {
                            Console.WriteLine("[SocketException]: Client => {0}", ex.Message);
                            Clients.RemoveAt(i);
                            break;
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("[Exception]: Client => {0}", ex.Message);
                            Clients.RemoveAt(i);
                            break;
                        }
                    }

                    if (Clients[i].Server == null) continue;

                    while (Clients[i].Server.NetworkManager.DataAvailable)
                    {
                        try
                        {
                            /* S -> P -> C
                             * Proxying data from server to client.
                             */

                            var rawPacket = (byte[])null;
                            var decryptedPacket = (byte[])null;
                            var packet = Clients[i].Server.NetworkManager.ReadPacket(out rawPacket, out decryptedPacket); // receive data from server

                            if (packet != null) HandlePacket(Clients[i], packet);

                            if (rawPacket != null && packet != null)
                            {
                                // could log packets in a sperate thread
                                Clients[i].Client.NetworkManager.CoCStream.Write(rawPacket, 0, rawPacket.Length); // sends data back to client
                                PacketLogger.LogPacket(packet, PacketDirection.Client);
                                PacketDumper.LogPacket(packet, PacketDirection.Client, decryptedPacket);
                            }
                        }
                        catch (SocketException ex)
                        {
                            Console.WriteLine("[SocketException]: ProxyClient => {0}", ex.Message);
                            Clients.RemoveAt(i);
                            break;
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("[Exception]: ProxyClient => {0}", ex.Message);
                            Clients.RemoveAt(i);
                            break;
                        }
                    }
                }
                Thread.Sleep(1);
            }
        }

        private void AsyncOperationCompleted(object sender, SocketAsyncEventArgs e)
        {
            switch (e.LastOperation) // TODO: Check for errors
            {
                case SocketAsyncOperation.Accept:
                    HandleAcceptOperation(e);
                    break;
            }
        }

        private void HandleAcceptOperation(SocketAsyncEventArgs acceptEvent)
        {
            var remoteClient = new CoCProxyClient(acceptEvent.AcceptSocket);
            Clients.Add(remoteClient);

            acceptEvent.AcceptSocket = null;
            AcceptAsyncEventPool.Push(acceptEvent); // reuse the obj
        }

        private void RegisterDownloadedDatabases()
        {
            if (!Directory.Exists("databases")) 
                Directory.CreateDirectory("databases");

            var dbFiles = Directory.GetDirectories("databases");
            for (int i = 0; i < dbFiles.Length; i++)
            {
                var hash = Path.GetFileName(dbFiles[i]);
                var dbManager = new DatabaseManager(hash);
                DatabaseManagers.Add(dbManager.FingerprintHash, dbManager);
                dbManager.LoadDatabases();
            }
        }
    }
}
