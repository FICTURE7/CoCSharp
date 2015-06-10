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
            this.Clients = new List<CoCProxyClient>();
        }

        public PacketLogger PacketLogger { get; set; }
        public List<CoCProxyClient> Clients { get; set; }
        public TcpListener Listener { get; set; }

        private bool ShuttingDown { get; set; }
        private Thread NetworkThread { get; set; }

        public void Start(IPEndPoint endPoint)
        {
            NetworkThread = new Thread(HandleNetwork);
            Listener = new TcpListener(endPoint);

            Listener.Start(10);
            Listener.BeginAcceptTcpClient(new AsyncCallback(AcceptClientAysnc), Listener);

            NetworkThread.Name = "NetworkHandler";
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
            var cocClientProxy = new CoCProxyClient(client, new TcpClient(DefaultServer, DefaultPort));
            Clients.Add(cocClientProxy);
            Listener.BeginAcceptTcpClient(new AsyncCallback(AcceptClientAysnc), Listener);

            Console.WriteLine("Client connected {0}", client.Client.RemoteEndPoint);
            Console.WriteLine("Created new connection to {0}:{1}", DefaultServer, DefaultPort);
        }

        private void HandleNetwork()
        {
            while (true)
            {
                if (ShuttingDown) return; // try to avoid ThreadAbortExceptions

                for (int i = 0; i < Clients.Count; i++)
                {
                    var client = Clients[i].Client;
                    var server = Clients[i].Server;

                    try
                    {
                        while (client.NetworkManager.DataAvailable)
                        {
                            try
                            {
                                /* S <- P <- C
                                 * Proxying data from client to server.
                                 */

                                var rawPacket = (byte[])null; // raw encrypted packet
                                var packet = client.NetworkManager.ReadPacket(out rawPacket); // receive data from client

                                if (packet != null) PacketLogger.LogPacket(packet, PacketDirection.Server);
                                if (rawPacket != null) server.NetworkManager.CoCStream.Write(rawPacket, 0, rawPacket.Length); // sends data back to server

                                if (packet is LoginRequestPacket) client.Seed = ((LoginRequestPacket)packet).ClientSeed;

                                //TODO: Better handling of packets
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

                        while (server.NetworkManager.DataAvailable)
                        {
                            try
                            {
                                /* S -> P -> C
                                 * Proxying data from server to client.
                                 */

                                var rawPacket = (byte[])null;
                                var packet = server.NetworkManager.ReadPacket(out rawPacket); // receive data from server

                                if (packet != null) PacketLogger.LogPacket(packet, PacketDirection.Client);
                                if (rawPacket != null) client.NetworkManager.CoCStream.Write(rawPacket, 0, rawPacket.Length); // sends data back to client

                                if (packet is UpdateKeyPacket)
                                {
                                    //client.NetworkManager.UpdateKey(client.Seed, ((UpdateKeyPacket)packet).Key);
                                    client.NetworkManager.CoCCrypto.UpdateChipers(client.Seed, ((UpdateKeyPacket)packet).Key);

                                    // cant set server key properly copying client's key, i no liky :{
                                    server.NetworkManager.CoCCrypto.UpdateChipers(client.Seed, ((UpdateKeyPacket)packet).Key);
                                    //server.NetworkManager.DecryptKey = (byte[])client.NetworkManager.DecryptKey.Clone();
                                    //server.NetworkManager.EncryptKey = (byte[])client.NetworkManager.EncryptKey.Clone();
                                }

                                //TODO: Better handling of packets
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
                    catch (Exception ex) { Console.WriteLine("[Exception]: {0}", ex.Message); break; }
                }

                Thread.Sleep(1);
            }
        }
    }
}
