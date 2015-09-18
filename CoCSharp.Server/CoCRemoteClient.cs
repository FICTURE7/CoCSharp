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
            NetworkManager = new NetworkManagerAsync(connection);
            NetworkManager.PacketReceived += OnPacketReceived;
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

        private void OnPacketReceived(object sender, PacketReceivedEventArgs e)
        {
            if (e.Exception == null)
            {
                var handler = (PacketHandler)null;
                if (!PacketHandlers.TryGetValue(e.Packet.ID, out handler))
                    return;
                handler(this, Server, e.Packet);
            }
            else Console.WriteLine("Failed to read packet: \r\n{0}", e.Exception.Message);
        }

        public void RegisterPacketHandler(IPacket packet, PacketHandler handler)
        {
            PacketHandlers.Add(packet.ID, handler);
        }
    }
}
