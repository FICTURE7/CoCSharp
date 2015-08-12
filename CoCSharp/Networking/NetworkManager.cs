using CoCSharp.Networking.Packets;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Threading;

namespace CoCSharp.Networking
{
    /// <summary>
    /// Implements methods to read and write <see cref="IPacket"/>s with <see cref="Socket"/>.
    /// </summary>
    public class NetworkManager
    {
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
        /// Initializes a new instance of the <see cref="NetworkManager"/> class.
        /// </summary>
        /// <param name="connection"><see cref="Socket"/> to wrap.</param>
        /// <param name="networkHandler"><see cref="PacketReceivedHandler"/> that will handle incoming packet.</param>
        /// <param name="packetReceivedFailedHandler"><see cref="PacketReceivedFailedHandler"/> that will handle incoming packet that failed to read.</param>
        public NetworkManager(Socket connection, 
                              PacketReceivedHandler packetReceivedHandler, 
                              PacketReceivedFailedHandler packetReceivedFailedHandler)
        {
            if (PacketDictionary == null)  // could a use a static constructor?
                InitializePacketDictionary();

            Connection = connection;
            PacketReceived = packetReceivedHandler;
            PacketReceivedFailed = packetReceivedFailedHandler;
            Seed = MathHelper.Random.Next();
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
        public int Seed { get; private set; }

        private CoCCrypto CoCCrypto { get; set; }
        private PacketDirection Direction { get; set; }
        private PacketReceivedHandler PacketReceived { get; set; }
        private PacketReceivedFailedHandler PacketReceivedFailed { get; set; }
        private SocketAsyncEventArgsPool ReceiveEventPool { get; set; }
        private SocketAsyncEventArgsPool SendEventPool { get; set; }
        private static Dictionary<ushort, Type> PacketDictionary { get; set; }

        /// <summary>
        /// Reads an <see cref="IPacket"/> from a <see cref="SocketAsyncEventArgs"/> object.
        /// </summary>
        /// <param name="args">The <see cref="SocketAsyncEventArgs"/> object from which 
        /// the <see cref="IPacket"/> will be read.</param>
        /// <returns>The <see cref="IPacket"/> read.</returns>
        public IPacket ReadPacket(SocketAsyncEventArgs args)
        {
            // Console.WriteLine("Being reading packet on thread {0}", Thread.CurrentThread.ManagedThreadId);
            if (args.BytesTransferred == 0)
                return null; //TODO: Handle disconnection

            var packetBuffer = (PacketBuffer)args.UserToken;
            using (var enPacketReader = new PacketReader(new MemoryStream(packetBuffer.Buffer)))
            {
                if (PacketBuffer.HeaderSize > args.BytesTransferred) // check if there is a header
                    return null;

                // read header
                var packetID = enPacketReader.ReadUInt16();
                var packetLength = enPacketReader.ReadInt24();
                var packetVersion = enPacketReader.ReadUInt16(); // the unknown

                // read body
                if (packetLength > args.BytesTransferred) // check if data is enough data is avaliable in the buffer
                    return null;

                var encryptedData = packetBuffer.ExtractPacket(PacketExtractionFlags.Body |
                                                               PacketExtractionFlags.Remove,
                                                               packetLength);
                var decryptedData = (byte[])encryptedData.Clone(); // cloning just cause we want the encrypted data
                CoCCrypto.Decrypt(decryptedData);

                args.SetBuffer(packetBuffer.Buffer, 0, packetBuffer.Buffer.Length);

                using (var dePacketReader = new PacketReader(new MemoryStream(decryptedData)))
                {
                    var packet = GetPacketInstance(packetID);
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
                    // Console.WriteLine("End reading packet on thread {0}", Thread.CurrentThread.ManagedThreadId);
                    return packet;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="packet"></param>
        public void WritePacket(IPacket packet)
        {
            using (var dePacketWriter = new PacketWriter(new MemoryStream()))
            {
                packet.WritePacket(dePacketWriter);

                var buffer = ((MemoryStream)dePacketWriter.BaseStream).ToArray();
                CoCCrypto.Encrypt(buffer);

                using (var enPacketWriter = new PacketWriter(new MemoryStream()))
                {
                    enPacketWriter.WriteUInt16(packet.ID);
                    enPacketWriter.WritePacketLength(buffer.Length);
                    enPacketWriter.WriteUInt16(0); // the unknown or the packet version
                    enPacketWriter.Write(buffer, 0, buffer.Length);

                    var rawPacket = ((MemoryStream)enPacketWriter.BaseStream).ToArray();
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
        public void UpdateChipers(ulong seed, byte[] key)
        {
            CoCCrypto.UpdateChipers(seed, key);
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

        private void AsyncOperationCompleted(object sender, SocketAsyncEventArgs args)
        {
            args.Completed -= AsyncOperationCompleted;
            if (args.SocketError != SocketError.Success)
                throw new SocketException((int)args.SocketError);

            switch (args.LastOperation)
            {
                case SocketAsyncOperation.Receive:
                    var packet = (IPacket)null;

                    try 
                    { packet = ReadPacket(args); }
                    catch (Exception ex)
                    { PacketReceivedFailed(args, ex); }

                    if (packet != null)
                        PacketReceived(args, packet); // pass it to the handler
                    StartReceive();
                    ReceiveEventPool.Push(args);
                    break;
            }
        }

        private static IPacket GetPacketInstance(ushort id)
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
