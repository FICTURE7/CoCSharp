using CoCSharp.Logic;
using CoCSharp.Networking;
using CoCSharp.Networking.Packets;
using System;
using System.Collections.Generic;
using System.Net.Sockets;

namespace CoCSharp.Server
{
    public class CoCRemoteClient
    {
        public delegate void PacketHandler(CoCRemoteClient client, CoCServer server, IPacket packet);

        public CoCRemoteClient(CoCServer server, Socket connection)
        {
            Connection = connection;
            Server = server;
            NetworkManager = new NetworkManagerAsync(connection, HandlePacketReceived, HandleReceicedPacketFailed);
            PacketHandlers = new Dictionary<ushort, PacketHandler>();
            Avatar = new Avatar();
            Home = new Village();
        }

        public Village Home { get; set; }
        public Avatar Avatar { get; set; }
        public TimeSpan PlayTime { get; set; }
        public DateTime DateJoined { get; set; }
        public DateTime DateLastPlayed { get; set; }
        public NetworkManagerAsync NetworkManager { get; set; }
        public Socket Connection { get; set; }

        private CoCServer Server { get; set; }
        private Dictionary<ushort, PacketHandler> PacketHandlers { get; set; }

        public void QueuePacket(IPacket packet)
        {
            if (packet == null)
                throw new ArgumentNullException("packet");
            NetworkManager.SendPacket(packet);
        }

        public void RegisterPacketHandler(IPacket packet, PacketHandler handler)
        {
            PacketHandlers.Add(packet.ID, handler);
        }

        private void HandlePacketReceived(SocketAsyncEventArgs args, IPacket packet)
        {
            var handler = (PacketHandler)null;
            if (!PacketHandlers.TryGetValue(packet.ID, out handler))
                return;
            handler(this, Server, packet);
        }

        private void HandleReceicedPacketFailed(SocketAsyncEventArgs args, Exception ex)
        {
            Console.WriteLine("Failed to read packet: \r\n{0}", ex.Message);
        }
    }
}
