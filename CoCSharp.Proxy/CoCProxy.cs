using CoCSharp.Logging;
using CoCSharp.Networking;
using CoCSharp.Networking.Packets;
using CoCSharp.Proxy.Handlers;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace CoCSharp.Proxy
{
    public class CoCProxy
    {
        public const int DefaultPort = 9339;
        public const string DefaultServer = "gamea.clashofclans.com";

        public delegate void PacketHandler(CoCProxy proxyServer, CoCProxyClient client, IPacket packet);

        public CoCProxy()
        {
            ExceptionLog = new ExceptionLog("exceptions");
            PacketLog = new PacketLog("packets.log");
            PacketLog.AutoSave = true;
            PacketDumper = new PacketDumper();
            Clients = new List<CoCProxyClient>();
            PacketHandlers = new Dictionary<ushort, PacketHandler>();

            ProxyPacketHandlers.RegisterHanlders(this);
        }

        public string ServerAddress { get; set; }
        public int ServerPort { get; set; }
        public ExceptionLog ExceptionLog { get; set; }
        public PacketLog PacketLog { get; set; }
        public PacketDumper PacketDumper { get; set; }
        public List<CoCProxyClient> Clients { get; set; }
        public Dictionary<ushort, PacketHandler> PacketHandlers { get; set; }
        public Socket Listener { get; set; }
        public IPEndPoint EndPoint { get; set; }

        private bool ShuttingDown { get; set; }
        private Thread NetworkThread { get; set; }

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
            Listener.BeginAccept(AcceptClient, Listener);
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

        private void AcceptClient(IAsyncResult ar)
        {
            var socket = Listener.EndAccept(ar);
            var client = new CoCProxyClient(socket);
            Console.WriteLine("Accepted new socket: {0}", socket.RemoteEndPoint);
            client.Client.NetworkManager.ExceptionLog = ExceptionLog;
            Clients.Add(client);
            Listener.BeginAccept(AcceptClient, Listener);
        }

        private void HandlePacket(CoCProxyClient client, IPacket packet)
        {
            var handler = (PacketHandler)null;
            if (!PacketHandlers.TryGetValue(packet.ID, out handler))
                return;
            handler(this, client, packet);
        }

        private void HandleNetwork()
        {
            while (true)
            {
                if (ShuttingDown)
                    return;

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

                            if (packet != null)
                            {
                                HandlePacket(Clients[i], packet);
                                if (rawPacket != null)
                                {
                                    Clients[i].Server.NetworkManager.CoCStream.Write(rawPacket, 0, rawPacket.Length); // sends data back to server
                                    PacketDumper.LogPacket(packet, PacketDirection.Server, decryptedPacket);
                                    PacketLog.LogData(packet, PacketDirection.Server);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            ExceptionLog.LogData(ex);
                            Clients.RemoveAt(i);
                            goto ResetLoop;
                        }
                    }

                    if (Clients[i].Server == null)
                        continue;

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

                            if (packet != null)
                            {
                                HandlePacket(Clients[i], packet);
                                if (rawPacket != null)
                                {
                                    Clients[i].Client.NetworkManager.CoCStream.Write(rawPacket, 0, rawPacket.Length); // sends data back to client
                                    PacketDumper.LogPacket(packet, PacketDirection.Client, decryptedPacket);
                                    PacketLog.LogData(packet, PacketDirection.Client);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            ExceptionLog.LogData(ex);
                            Clients.RemoveAt(i);
                            goto ResetLoop;
                        }
                    }

                    continue;
                    ResetLoop:
                    break;
                }
                Thread.Sleep(1);
            }
        }
    }
}

