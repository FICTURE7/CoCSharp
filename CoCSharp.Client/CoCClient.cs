﻿using CoCSharp.Client.API;
using CoCSharp.Client.API.Events;
using CoCSharp.Client.Handlers;
using CoCSharp.Data;
using CoCSharp.Logging;
using CoCSharp.Logic;
using CoCSharp.Networking;
using CoCSharp.Networking.Packets;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace CoCSharp.Client
{
    public class CoCClient : ICoCClient
    {
        #region Constructors
        public CoCClient()
        {
            Fingerprint = new Fingerprint();
            Home = new Village();
            Avatar = new Avatar();
            Connection = new Socket(SocketType.Stream, ProtocolType.Tcp);
            DefaultPacketHandlers = new Dictionary<ushort, PacketHandler>();
            PacketHandlers = new Dictionary<ushort, PacketHandler>();
            KeepAliveManager = new KeepAliveManager(this);
            PacketLogger = new PacketLogger()
            {
                LogConsole = false
            };
            PluginManager = new PluginManager(this);

            LoginPacketHandlers.RegisterLoginPacketHandlers(this);
            InGamePacketHandlers.RegisterInGamePacketHandler(this);
            PluginManager.LoadPlugins();
            PluginManager.EnablePlugins();
        }
        #endregion

        #region Properties
        public bool Connected { get { return Connection.Connected; } }
        public Socket Connection { get; set; }
        public Village Home { get; set; }
        public Avatar Avatar { get; set; }
        public Fingerprint Fingerprint { get; set; }

        private PacketLogger PacketLogger { get; set; }
        private NetworkManagerAsync NetworkManager { get; set; }
        private KeepAliveManager KeepAliveManager { get; set; }
        private Dictionary<ushort, PacketHandler> PacketHandlers { get; set; }
        private Dictionary<ushort, PacketHandler> DefaultPacketHandlers { get; set; }
        private PluginManager PluginManager { get; set; }

        internal void OnClanSearch(ClanSearchEventArgs clanSearchEventArgs)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Methods
        public void Connect(IPEndPoint endPoint)
        {
            if (endPoint == null)
                throw new ArgumentNullException("endPoint");

            var args = new SocketAsyncEventArgs();
            args.Completed += ConnectAsyncCompleted;
            args.RemoteEndPoint = endPoint;
            Connection.ConnectAsync(args);
        }

        private void ConnectAsyncCompleted(object sender, SocketAsyncEventArgs e)
        {
            if (e.SocketError != SocketError.Success)
                throw new SocketException((int)e.SocketError);

            NetworkManager = new NetworkManagerAsync(e.ConnectSocket);
            NetworkManager.PacketReceived += OnPacketReceived;
            NetworkManager.Seed = new Random().Next();
            SendPacket(new LoginRequestPacket()
            {
                UserID = Avatar.ID,
                UserToken = Avatar.Token,
                ClientMajorVersion = 7,
                ClientContentVersion = 12,
                ClientMinorVersion = 200,
                FingerprintHash = "7af2ba412c1716cffe3949f1dcffcea6822560f2",
                OpenUDID = "563a6f060d8624db",
                MacAddress = null,
                DeviceModel = "GT-I9300",
                LocaleKey = 2000000,
                Language = "en",
                AdvertisingGUID = "",
                OSVersion = "4.0.4",
                IsAdvertisingTrackingEnabled = false,
                AndroidDeviceID = "563a6f060d8624db",
                FacebookDistributionID = "",
                VendorGUID = "",
                Seed = NetworkManager.Seed
            });
            KeepAliveManager.Start();
        }

        public void SendChatMessage(string message)
        {
            SendPacket(new ChatMessageClientPacket()
            {
                Message = message
            });
        }


        public void DoClanSearch(string clanName)
        {
            SendPacket(new AllianceSearchRequestPacket()
            {
                SearchString = clanName
            });
        }


        public void SendPacket(IPacket packet)
        {
            if (packet == null)
                throw new ArgumentNullException("packet");
            if (NetworkManager == null)
                throw new InvalidOperationException("Tried to send a packet before NetworkManager was initialized or before socket was connected.");

            PacketLogger.LogPacket(packet, PacketDirection.Server);
            NetworkManager.SendPacket(packet);
        }

        public void RegisterPacketHandler(IPacket packet, PacketHandler handler)
        {
            if (packet == null)
                throw new ArgumentNullException("packet");
            if (handler == null)
                throw new ArgumentNullException("handler");

            PacketHandlers.Add(packet.ID, handler);
        }

        internal void RegisterDefaultPacketHandler(IPacket packet, PacketHandler handler)
        {
            if (packet == null)
                throw new ArgumentNullException("packet");
            if (handler == null)
                throw new ArgumentNullException("handler");

            DefaultPacketHandlers.Add(packet.ID, handler);
        }

        private void OnPacketReceived(object sender, PacketReceivedEventArgs e)
        {
            if (e.Exception == null)
            {
                PacketLogger.LogPacket(e.Packet, PacketDirection.Client);
                var defaultHandler = (PacketHandler)null;
                var handler = (PacketHandler)null;

                if (DefaultPacketHandlers.TryGetValue(e.Packet.ID, out defaultHandler))
                    defaultHandler(this, e.Packet); // use default handler

                if (PacketHandlers.TryGetValue(e.Packet.ID, out handler))
                    handler(this, e.Packet); // use custom handler
            }
            else Console.WriteLine("Failed to read packet: {0}", e.Exception.Message);
        }
        #endregion

        #region Events
        public event EventHandler<ChatMessageEventArgs> ChatMessage;
        protected internal virtual void OnChatMessage(ChatMessageEventArgs e)
        {
            if (ChatMessage != null)
                ChatMessage(this, e);
        }

        public event EventHandler<LoginEventArgs> Login;
        protected internal virtual void OnLogin(LoginEventArgs e)
        {
            if (Login != null)
                Login(this, e);
        }
        #endregion
    }
}
