using CoCSharp.Networking.Packets;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;

namespace CoCSharp.Networking
{
    /// <summary>
    /// Implements methods to read and write <see cref="IPacket"/>s with <see cref="Socket"/>.
    /// </summary>
    public class NetworkManagerAsync
    {
        private Object _ObjLock = new Object();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        /// <param name="packet"></param>
        public delegate void PacketReceivedHandler(SocketAsyncEventArgs args, IPacket packet);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        /// <param name="ex"></param>
        public delegate void PacketReceivedFailedHandler(SocketAsyncEventArgs args, Exception ex);

        /// <summary>
        /// Initializes a new instance of the <see cref="NetworkManagerAsync"/> class.
        /// </summary>
        /// <param name="connection"><see cref="Socket"/> to wrap.</param>
        /// <param name="networkHandler"><see cref="PacketReceivedHandler"/> that will handle incoming packet.</param>
        /// <param name="packetReceivedFailedHandler"><see cref="PacketReceivedFailedHandler"/> that will handle incoming packet that failed to read.</param>
        /// <exception cref="System.ArgumentNullException"/>
        public NetworkManagerAsync(Socket connection,
                              PacketReceivedHandler packetReceivedHandler,
                              PacketReceivedFailedHandler packetReceivedFailedHandler)
        {
            if (PacketDictionary == null)  // could a use a static constructor?
                InitializePacketDictionary();
            if (connection == null)
                throw new ArgumentNullException("connection");
            if (packetReceivedHandler == null)
                throw new ArgumentNullException("packetReceivedHandler");


            Connection = connection;
            PacketReceived = packetReceivedHandler;
            PacketReceivedFailed = packetReceivedFailedHandler;
            Seed = MathHelper.Random.Next(); // must make this moar flexible
            CoCCrypto = new CoCCrypto();
            ReceiveEventPool = new SocketAsyncEventArgsPool(25);
            SendEventPool = new SocketAsyncEventArgsPool(25);
            StartReceive();
        }

        /// <summary>
        /// Gets whether data is available in the socket.
        /// </summary>
        public bool DataAvailable { get { return Connection.Available > 0; } }
        /// <summary>
        /// Gets whether the socket is disconnected.
        /// </summary>
        public bool Disconnected { get; private set; }
        /// <summary>
        /// Gets the <see cref="Socket"/>.
        /// </summary>
        public Socket Connection { get; private set; }
        /// <summary>
        /// Gets the seed for encryption.
        /// </summary>
        public int Seed { get; private set; }

        private CoCCrypto CoCCrypto { get; set; }
        private PacketReceivedHandler PacketReceived { get; set; }
        private PacketReceivedFailedHandler PacketReceivedFailed { get; set; }
        private SocketAsyncEventArgsPool ReceiveEventPool { get; set; }
        private SocketAsyncEventArgsPool SendEventPool { get; set; } // we are not using this properly. :{
        private static Dictionary<ushort, Type> PacketDictionary { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public IPacket[] ReadPackets(SocketAsyncEventArgs args)
        {
            var list = new List<IPacket>();
            var numBytesToProcess = args.BytesTransferred;
            if (numBytesToProcess == 0)
                return null;

            while (true)
            {
                if (numBytesToProcess == 0)
                    break;

                var packet = (IPacket)null;
                var packetBuffer = (PacketBuffer)args.UserToken;
                try
                {
                    using (var enPacketReader = new PacketReader(new MemoryStream(packetBuffer.Buffer)))
                    {
                        if (PacketBuffer.HeaderSize > numBytesToProcess) // check if there is a header
                            break;

                        // read header
                        var packetID = enPacketReader.ReadUInt16();
                        var packetLength = enPacketReader.ReadInt24();
                        var packetVersion = enPacketReader.ReadUInt16(); // the unknown

                        // read body
                        if (packetLength > numBytesToProcess) // check if data is enough data is avaliable in the buffer
                            break;

                        var encryptedData = packetBuffer.ExtractPacket(PacketExtractionFlags.Body |
                                                                       PacketExtractionFlags.Remove,
                                                                       packetLength);
                        var decryptedData = (byte[])encryptedData.Clone(); // cloning just cause we want the encrypted data
                        CoCCrypto.Decrypt(decryptedData);

                        //TODO: PacketBuffer should use SocketAsyncEventArgs directly.
                        numBytesToProcess -= packetLength + PacketBuffer.HeaderSize;

                        using (var dePacketReader = new PacketReader(new MemoryStream(decryptedData)))
                        {
                            packet = CreatePacketInstance(packetID);
                            if (packet is UnknownPacket)
                            {
                                packet = new UnknownPacket
                                {
                                    ID = packetID,
                                    Length = packetLength,
                                    Version = packetVersion,
                                    EncryptedData = encryptedData,
                                    DecryptedData = decryptedData
                                };
                            }
                            packet.ReadPacket(dePacketReader);
                        }
                    }
                    if (packet is UpdateKeyPacket)
                        UpdateCiphers(Seed, ((UpdateKeyPacket)packet).Key);
                    list.Add(packet);
                }
                catch (Exception ex)
                {
                    if (PacketReceivedFailed != null)
                        PacketReceivedFailed(args, ex);
                }
            }
            return list.ToArray();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="packet"></param>
        /// <exception cref="System.ArgumentNullException"/>
        public void WritePacket(IPacket packet)
        {
            if (packet == null)
                throw new ArgumentNullException("packet");

            using (var dePacketWriter = new PacketWriter(new MemoryStream()))
            {
                packet.WritePacket(dePacketWriter);
                var body = ((MemoryStream)dePacketWriter.BaseStream).ToArray();
                CoCCrypto.Encrypt(body);
                using (var enPacketWriter = new PacketWriter(new MemoryStream())) // write header
                {
                    enPacketWriter.WriteUInt16(packet.ID);
                    enPacketWriter.WriteInt24(body.Length);
                    enPacketWriter.WriteUInt16(0); // the unknown or the packet version
                    enPacketWriter.Write(body, 0, body.Length);

                    var rawPacket = ((MemoryStream)enPacketWriter.BaseStream).ToArray();

                    // should avoid new objs
                    var args = new SocketAsyncEventArgs();
                    args.SetBuffer(rawPacket, 0, rawPacket.Length);
                    if (!Connection.SendAsync(args))
                        AsyncOperationCompleted(Connection, args);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="seed"></param>
        /// <param name="key"></param>
        /// <exception cref="System.ArgumentNullException"/>
        public void UpdateCiphers(int seed, byte[] key)
        {
            if (key == null)
                throw new ArgumentNullException("key");

            CoCCrypto.UpdateCiphers((ulong)seed, key);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Disconnect()
        {
            Disconnected = true;
            Connection.Close();
        }

        private void StartReceive()
        {
            var receiveArgs = ReceiveEventPool.Pop();
            receiveArgs.Completed += AsyncOperationCompleted;
            if (!Connection.ReceiveAsync(receiveArgs))
                AsyncOperationCompleted(Connection, receiveArgs);
        }

        private int _counter = 0;
        public void AsyncOperationCompleted(object sender, SocketAsyncEventArgs args)
        {
            args.Completed -= AsyncOperationCompleted;
            if (args.SocketError != SocketError.Success)
                throw new SocketException((int)args.SocketError);

            lock (_ObjLock)
            {
                switch (args.LastOperation)
                {
                    case SocketAsyncOperation.Receive:
                        var packets = (IPacket[])null;
                        packets = ReadPackets(args);

                        if (packets == null)
                            return;

                        for (int i = 0; i < packets.Length; i++)
                            PacketReceived(args, packets[i]); // pass it to the handler

                        StartReceive();
                        ReceiveEventPool.Push(args);
                        break;
                }
            }
        }

        private static IPacket CreatePacketInstance(ushort id)
        {
            var packetType = (Type)null;
            if (!PacketDictionary.TryGetValue(id, out packetType))
                return new UnknownPacket();
            return (IPacket)Activator.CreateInstance(packetType); // creates an instance for that packetid
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
            PacketDictionary.Add(new LoginFailedPacket().ID, typeof(LoginFailedPacket)); // 20103
            PacketDictionary.Add(new LoginSuccessPacket().ID, typeof(LoginSuccessPacket)); // 20104
            PacketDictionary.Add(new KeepAliveResponsePacket().ID, typeof(KeepAliveResponsePacket)); // 20108
            PacketDictionary.Add(new OwnHomeDataPacket().ID, typeof(OwnHomeDataPacket)); // 24101
            PacketDictionary.Add(new AllianceChatMessageServerPacket().ID, typeof(AllianceChatMessageServerPacket)); // 24312
            PacketDictionary.Add(new ChatMessageServerPacket().ID, typeof(ChatMessageServerPacket)); // 24715
        }
    }
}
