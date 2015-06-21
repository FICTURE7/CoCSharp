using CoCSharp.Database;
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
    public class CoCProxy
    {
        public const int DefaultPort = 9339;
        public const string DefaultServer = "gamea.clashofclans.com";

        public CoCProxy()
        {
            this.PacketLogger = new PacketLogger();
            this.PacketDumper = new PacketDumper();
            this.BuildingDatabase = new BuildingDatabase(@"database\buildings.csv");
            this.TrapDatabase = new TrapDatabase(@"database\traps.csv");
            this.DecorationDatabase = new DecorationDatabase(@"database\decos.csv");
            this.ObstacleDatabase = new ObstacleDatabase(@"database\obstacles.csv");
            this.Clients = new List<CoCProxyClient>();

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
        public TcpListener Listener { get; set; }

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

        private void AcceptClientAysnc(IAsyncResult result)
        {
            var client = Listener.EndAcceptTcpClient(result);
            var cocClientProxy = new CoCProxyClient(client);

            Clients.Add(cocClientProxy);
            Listener.BeginAcceptTcpClient(new AsyncCallback(AcceptClientAysnc), Listener);
        }

        private void HandleNetwork()
        {
            while (true)
            {
                if (ShuttingDown) return;

                for (int i = 0; i < Clients.Count; i++)
                {
                    //TODO: Kick client due to keep alive timeouts.
                    var client = Clients[i].Client;
                    var server = Clients[i].Server;

                    while (client.NetworkManager.DataAvailable)
                    {
                        try
                        {
                            /* S <- P <- C
                             * Proxying data from client to server.
                             */

                            var rawPacket = (byte[])null; // raw encrypted packet
                            var decryptedPacket = (byte[])null;
                            var packet = client.NetworkManager.ReadPacket(out rawPacket, out decryptedPacket); // receive data from client

                            //TODO: Better handling of packets
                            if (packet is LoginRequestPacket)
                            {
                                Clients[i].Start(new TcpClient(DefaultServer, DefaultPort));

                                var lrPacket = packet as LoginRequestPacket;
                                client.Seed = lrPacket.Seed;
                                client.UserID = lrPacket.UserID;
                                client.UserToken = lrPacket.UserToken;

                                server = Clients[i].Server;
                            }

                            if (rawPacket != null && packet != null)
                            {
                                server.NetworkManager.CoCStream.Write(rawPacket, 0, rawPacket.Length); // sends data back to server
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

                    if (server == null) continue;
                    while (server.NetworkManager.DataAvailable)
                    {
                        try
                        {
                            /* S -> P -> C
                             * Proxying data from server to client.
                             */

                            var rawPacket = (byte[])null;
                            var decryptedPacket = (byte[])null;
                            var packet = server.NetworkManager.ReadPacket(out rawPacket, out decryptedPacket); // receive data from server

                            //TODO: Better handling of packets
                            if (packet is UpdateKeyPacket)
                            {
                                client.NetworkManager.UpdateChipers((ulong)client.Seed, ((UpdateKeyPacket)packet).Key);
                                server.NetworkManager.UpdateChipers((ulong)client.Seed, ((UpdateKeyPacket)packet).Key);
                            }

                            if (packet is LoginSuccessPacket)
                            {
                                var lsPacket = packet as LoginSuccessPacket;
                                client.UserID = lsPacket.UserID;
                                client.UserToken = lsPacket.UserToken;
                                client.LoggedIn = true;
                            }

                            if (packet is OwnHomeDataPacket)
                            {
                                var ohPacket = packet as OwnHomeDataPacket;
                                client.Username = ohPacket.Username;
                                client.UserID = ohPacket.UserID;
                                client.Home = ohPacket.Home;

                                for (int x = 0; x < client.Home.Buildings.Count; x++)
                                    client.Home.Buildings[x].FromDatabase(BuildingDatabase);
                                for (int x = 0; x < client.Home.Traps.Count; x++)
                                    client.Home.Traps[x].FromDatabase(TrapDatabase);
                                for (int x = 0; x < client.Home.Decorations.Count; x++)
                                    client.Home.Decorations[x].FromDatabase(DecorationDatabase);
                                for (int x = 0; x < client.Home.Obstacles.Count; x++)
                                    client.Home.Obstacles[x].FromDatabase(ObstacleDatabase);
                            }

                            if (rawPacket != null && packet != null)
                            {
                                // could log packets in a sperate thread
                                client.NetworkManager.CoCStream.Write(rawPacket, 0, rawPacket.Length); // sends data back to client
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
