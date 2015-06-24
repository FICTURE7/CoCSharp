using CoCSharp.Databases;
using CoCSharp.Logger;
using CoCSharp.Networking;
using CoCSharp.Networking.Packets;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace CoCSharp
{
    public class CoCProxyServer
    {
        public const int DefaultPort = 9339;
        public const string DefaultServer = "gamea.clashofclans.com";

        public delegate void PacketHandler(CoCProxyServer proxyServer, CoCProxyClient client, IPacket packet);

        public CoCProxyServer()
        {
            this.PacketLogger = new PacketLogger();
            this.PacketDumper = new PacketDumper();
            this.BuildingDatabase = new BuildingDatabase(@"database\buildings.csv");
            this.TrapDatabase = new TrapDatabase(@"database\traps.csv");
            this.DecorationDatabase = new DecorationDatabase(@"database\decos.csv");
            this.ObstacleDatabase = new ObstacleDatabase(@"database\obstacles.csv");
            this.Clients = new List<CoCProxyClient>();
            this.PacketHandlers = new Dictionary<ushort, PacketHandler>();

            BuildingDatabase.LoadDatabase();
            TrapDatabase.LoadDatabase();
            DecorationDatabase.LoadDatabase();
            ObstacleDatabase.LoadDatabase();
        }

        public PacketLogger PacketLogger { get; set; }
        public PacketDumper PacketDumper { get; set; }
        public BuildingDatabase BuildingDatabase { get; set; }
        public TrapDatabase TrapDatabase { get; set; }
        public DecorationDatabase DecorationDatabase { get; set; }
        public ObstacleDatabase ObstacleDatabase { get; set; }
        public List<CoCProxyClient> Clients { get; set; }
        public Dictionary<ushort, PacketHandler> PacketHandlers { get; set; }
        public TcpListener Listener { get; set; }
        public string ServerAddress { get; set; }
        public int ServerPort { get; set; }

        private bool ShuttingDown { get; set; }
        private Thread NetworkThread { get; set; }

        public void Start(IPEndPoint endPoint)
        {
            ShuttingDown = false;
            NetworkThread = new Thread(HandleNetwork);
            Listener = new TcpListener(endPoint);
            
            Listener.Start(10);
            Listener.BeginAcceptTcpClient(new AsyncCallback(AcceptClientAysnc), Listener);

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
            if (Listener != null) Listener.Stop();
        }

        public void RegisterPacketHandler(IPacket packet, PacketHandler handler)
        {
            PacketHandlers.Add(packet.ID, handler);
        }

        private void AcceptClientAysnc(IAsyncResult result)
        {
            var client = Listener.EndAcceptTcpClient(result);
            var cocClientProxy = new CoCProxyClient(client);

            Clients.Add(cocClientProxy);
            Listener.BeginAcceptTcpClient(new AsyncCallback(AcceptClientAysnc), Listener);
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
    }
}
