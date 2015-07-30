using CoCSharp.Logging;
using CoCSharp.Networking.Packets;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;

namespace CoCSharp.Networking
{
    public class NetworkManager
    {
        // use this handler to handle network
        public delegate void NetworkHandler(SocketAsyncEventArgs args, IPacket packet);

        public NetworkManager(ICoCServer server, Socket connection, NetworkHandler networkHandler, PacketDirection direction)
        {
            if (PacketDictionary == null)  // could a use a static constructor?
                InitializePacketDictionary();

            Connection = connection;
            Handler = networkHandler;
            Direction = direction;
            CoCServer = server;
            CoCStream = new CoCStream(connection);
            CoCCrypto = new CoCCrypto();
            ReceiveEventPool = new SocketAsyncEventArgsPool(25);
            SendEventPool = new SocketAsyncEventArgsPool(25);
            StartReceive();
        }

        public bool DataAvailable { get { return Connection.Available > 0; } }
        public bool Disconnected { get; private set; }
        public CoCStream CoCStream { get; set; }
        public Socket Connection { get; set; }

        private CoCCrypto CoCCrypto { get; set; }
        private ICoCServer CoCServer { get; set; }
        private PacketDirection Direction { get; set; }
        private NetworkHandler Handler { get; set; }
        private SocketAsyncEventArgsPool ReceiveEventPool { get; set; }
        private SocketAsyncEventArgsPool SendEventPool { get; set; }
        private static Dictionary<ushort, Type> PacketDictionary { get; set; }

        public IPacket ReadPacket(SocketAsyncEventArgs args)
        {
            /* Receive data from the socket, saves it a buffer,
             * then reads packet from the buffer. 
             */

            if (args.BytesTransferred == 0)
                return null; //TODO: Handle disconnection

            var timeout = DateTime.Now.AddMilliseconds(500);
            while (DateTime.Now < timeout)
            {
                var packetBuffer = (PacketBuffer)args.UserToken;
                var enPacketReader = new PacketReader(new MemoryStream(packetBuffer.Buffer));

                if (PacketBuffer.HeaderSize > args.BytesTransferred) // check if there is a header
                    continue;

                // read header
                var packetID = enPacketReader.ReadUInt16();
                var packetLength = enPacketReader.ReadPacketLength();
                var packetVersion = enPacketReader.ReadUInt16();

                // read body
                if (packetLength > args.BytesTransferred) // check if data is enough data is avaliable in the buffer
                    continue;

                var encryptedData = packetBuffer.ExtractPacket(PacketExtractionFlags.Body |
                                                               PacketExtractionFlags.Remove, 
                                                               packetLength);
                var decryptedData = (byte[])encryptedData.Clone(); // cloning just cause we want the encrypted data

                CoCCrypto.Decrypt(decryptedData);

                var dePacketReader = new PacketReader(new MemoryStream(decryptedData));
                var packet = GetPacketInstance(packetID);
                if (packet is UnknownPacket)
                {
                    packet = new UnknownPacket
                    {
                        ID = packetID,
                        Length = packetLength,
                        Version = packetVersion
                    };
                    ((UnknownPacket)packet).EncryptedData = encryptedData;
                }

                packet.ReadPacket(dePacketReader);
                // CoCServer.PacketLogger.LogPacket(packet, PacketDirection.Server);
                // CoCServer.PacketDumper.LogPacket(packet, PacketDirection.Server, decryptedData);
                return packet;
            }
            return null;
        }

        public void UpdateChipers(ulong seed, byte[] key)
        {
            CoCCrypto.UpdateChipers(seed, key);
        }

        public void Disconnect()
        {
            Disconnected = true;
            Connection.Close();
        }

        private void StartReceive()
        {
            while (ReceiveEventPool.Count > 1)
            {
                var receiveArgs = ReceiveEventPool.Pop();
                receiveArgs.Completed += OperationCompleted;

                if (!Connection.ReceiveAsync(receiveArgs))
                    OperationCompleted(Connection, receiveArgs);
            }
        }

        private void OperationCompleted(object sender, SocketAsyncEventArgs args)
        {
            args.Completed -= OperationCompleted;

            if (args.SocketError != SocketError.Success) // handle stuff better here
                return;

            switch (args.LastOperation)
            {
                case SocketAsyncOperation.Receive:
                    var packet = ReadPacket(args);
                    Handler(args, packet);
                    ReceiveEventPool.Push(args);
                    StartReceive();
                    break;
            }
        }

        private static IPacket GetPacketInstance(ushort id)
        {
            var packetType = (Type)null;
            if (!PacketDictionary.TryGetValue(id, out packetType)) 
                return new UnknownPacket();
            return (IPacket)Activator.CreateInstance(packetType);
        }

        private static void InitializePacketDictionary()
        {
            PacketDictionary = new Dictionary<ushort, Type>();

            // Serverbound
            PacketDictionary.Add(new LoginRequestPacket().ID, typeof(LoginRequestPacket)); // 10101
            PacketDictionary.Add(new KeepAliveRequestPacket().ID, typeof(KeepAliveRequestPacket)); // 10108
            PacketDictionary.Add(new SetDeviceTokenPacket().ID, typeof(SetDeviceTokenPacket)); // 10113
            PacketDictionary.Add(new ChangeAvatarNamePacket().ID, typeof(ChangeAvatarNamePacket)); // 10212
            PacketDictionary.Add(new BindFacebookAccountPacket().ID, typeof(BindFacebookAccountPacket)); // 14201
            PacketDictionary.Add(new AvatarProfileRequestPacket().ID, typeof(AvatarProfileRequestPacket)); // 14325
            PacketDictionary.Add(new AllianceChatMessageClientPacket().ID, typeof(AllianceChatMessageClientPacket)); // 14315
            PacketDictionary.Add(new ChatMessageClientPacket().ID, typeof(ChatMessageClientPacket)); // 14715

            // Clientbound
            PacketDictionary.Add(new UpdateKeyPacket().ID, typeof(UpdateKeyPacket)); // 20000
            PacketDictionary.Add(new LoginSuccessPacket().ID, typeof(LoginSuccessPacket)); // 20104
            PacketDictionary.Add(new KeepAliveResponsePacket().ID, typeof(KeepAliveResponsePacket)); // 20108
            PacketDictionary.Add(new OwnHomeDataPacket().ID, typeof(OwnHomeDataPacket)); // 24101
            PacketDictionary.Add(new AllianceChatMessageServerPacket().ID, typeof(AllianceChatMessageServerPacket)); // 24312
            PacketDictionary.Add(new ChatMessageServerPacket().ID, typeof(ChatMessageServerPacket)); // 24715
        }
    }
}
