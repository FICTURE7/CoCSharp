using CoCSharp.Networking.Packets;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;

namespace CoCSharp.Networking
{
    /// <summary>
    /// Implements methods to read and write <see cref="IPacket"/>s to <see cref="Socket"/>.
    /// </summary>
    public class NetworkManagerAsync
    {
        private Object _ObjLock = new Object();

        //TODO: Remove SocketAsyncEventArgs from params cause its UserToken is from an internal class.

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
            CoCCrypto = new CoCCrypto();
            ReceiveEventPool = new SocketAsyncEventArgsPool(25);
            SendEventPool = new SocketAsyncEventArgsPool(25);
            StartReceive(ReceiveEventPool.Pop());
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
        /// Gets the <see cref="Socket"/> from which packets will be read and written.
        /// </summary>
        public Socket Connection { get; private set; }
        /// <summary>
        /// Gets or sets the seed for encryption.
        /// </summary>
        public int Seed { get; set; }

        private CoCCrypto CoCCrypto { get; set; }
        private PacketReceivedHandler PacketReceived { get; set; }
        private PacketReceivedFailedHandler PacketReceivedFailed { get; set; }
        private SocketAsyncEventArgsPool ReceiveEventPool { get; set; }
        private SocketAsyncEventArgsPool SendEventPool { get; set; } // we are not using this properly. :{
        private static Dictionary<ushort, Type> PacketDictionary { get; set; }

        /// <summary>
        /// Sends the specified packet to the socket asynchronously.
        /// </summary>
        /// <param name="packet">The <see cref="IPacket"/> that will be sent.</param>
        /// <exception cref="System.ArgumentNullException"/>
        public void SendPacket(IPacket packet)
        {
            if (packet == null)
                throw new ArgumentNullException("packet");

            using (var dePacketWriter = new PacketWriter(new MemoryStream()))
            {
                packet.WritePacket(dePacketWriter);
                var body = ((MemoryStream)dePacketWriter.BaseStream).ToArray();
                CoCCrypto.Encrypt(body);

                if (packet is UpdateKeyPacket)
                    UpdateCiphers(Seed, ((UpdateKeyPacket)packet).Key); // handle update key packet

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
        public void Disconnect()
        {
            //TODO: Better handling

            Disconnected = true;
            Connection.Close();
        }

        private void UpdateCiphers(int seed, byte[] key)
        {
            if (key == null)
                throw new ArgumentNullException("key");

            CoCCrypto.UpdateCiphers((ulong)seed, key);
        }


        private IPacket[] ProcessReceive(SocketAsyncEventArgs args)
        {
            var packetList = new List<IPacket>();
            var packetToken = args.UserToken as PacketToken;
            var bytesToProcess = args.BytesTransferred;

            if (bytesToProcess == 0)
            {
                //TODO: Fire event
                Disconnected = true;
                return null;
            }

        ReadPacket:

            // read header
            if (packetToken.HeaderReceiveOffset != PacketBuffer.HeaderSize) // we do not have the header
            {
                if (PacketBuffer.HeaderSize > bytesToProcess) // we got less that 7 bytes, some parts of the header
                {
                    // Console.WriteLine("[Net:ID {0}] Not enough bytes to read header.", packetToken.TokenID);

                    Buffer.BlockCopy(args.Buffer, packetToken.ReceiveOffset, packetToken.Header, packetToken.HeaderReceiveOffset, bytesToProcess);
                    packetToken.HeaderReceiveOffset += bytesToProcess;
                    packetToken.ReceiveOffset = 0;
                    StartReceive(args);
                    return packetList.ToArray();
                }
                else // if we got more than enough data for the header
                {
                    Buffer.BlockCopy(args.Buffer, packetToken.ReceiveOffset, packetToken.Header, packetToken.HeaderReceiveOffset, PacketBuffer.HeaderSize);
                    packetToken.HeaderReceiveOffset += PacketBuffer.HeaderSize;
                    packetToken.ReceiveOffset += PacketBuffer.HeaderSize; // probs here?
                    bytesToProcess -= PacketBuffer.HeaderSize;
                    ReadHeader(packetToken);
                }
            }

            // read body
            if (packetToken.BodyReceiveOffset != packetToken.Length)
            {
                if (packetToken.Length - packetToken.BodyReceiveOffset > bytesToProcess) // if we dont have enough to read body
                {
                    // Console.WriteLine("[Net:ID {0}] Not enough bytes to read body.", packetToken.TokenID);

                    Buffer.BlockCopy(args.Buffer, packetToken.ReceiveOffset, packetToken.Body, packetToken.BodyReceiveOffset, bytesToProcess);
                    packetToken.BodyReceiveOffset += bytesToProcess;
                    packetToken.ReceiveOffset = 0;
                    StartReceive(args);
                    return packetList.ToArray();
                }
                else // if we got more than enough data for the body
                {
                    Buffer.BlockCopy(args.Buffer, packetToken.ReceiveOffset, packetToken.Body, packetToken.BodyReceiveOffset, packetToken.Length - packetToken.BodyReceiveOffset);
                    bytesToProcess -= packetToken.Length - packetToken.BodyReceiveOffset;
                    packetToken.ReceiveOffset += packetToken.Length - packetToken.BodyReceiveOffset;
                    packetToken.BodyReceiveOffset += packetToken.Length;
                }
            }

            var packet = CreatePacketInstance(packetToken.ID);
            var packetDeData = (byte[])packetToken.Body.Clone();
            CoCCrypto.Decrypt(packetDeData);

            if (packet is UnknownPacket)
            {
                packet = new UnknownPacket()
                {
                    ID = packetToken.ID,
                    Length = packetToken.Length,
                    Version = packetToken.Version,
                    EncryptedData = packetToken.Body,
                    DecryptedData = packetDeData
                };
            }

            using (var reader = new PacketReader(new MemoryStream(packetDeData)))
            {
                try
                {
                    if (!(packet is UnknownPacket))
                        packet.ReadPacket(reader);
                }
                catch (Exception ex)
                {
                    if (PacketReceivedFailed != null)
                        PacketReceivedFailed(args, ex);
                    packetToken.Reset();
                    goto ReadPacket;
                }
            }

            if (packet is UpdateKeyPacket)
                UpdateCiphers(Seed, ((UpdateKeyPacket)packet).Key);

            packetList.Add(packet);
            packetToken.Reset();
            if (bytesToProcess != 0)
                goto ReadPacket;

            packetToken.ReceiveOffset = 0;
            ReceiveEventPool.Push(args);
            StartReceive(ReceiveEventPool.Pop());
            return packetList.ToArray();
        }

        private void ReadHeader(PacketToken token)
        {
            token.ID = (ushort)((token.Header[0] << 8) | (token.Header[1]));
            token.Length = (token.Header[2] << 16) | (token.Header[3] << 8) | (token.Header[4]);
            token.Version = (ushort)((token.Header[5] << 8) | (token.Header[6]));
            token.Body = new byte[token.Length];
        }

        private void StartReceive(SocketAsyncEventArgs e)
        {
            e.Completed += AsyncOperationCompleted;
            if (!Connection.ReceiveAsync(e))
                AsyncOperationCompleted(Connection, e);
        }

        public void AsyncOperationCompleted(object sender, SocketAsyncEventArgs args)
        {
            args.Completed -= AsyncOperationCompleted;
            if (args.SocketError != SocketError.Success)
                throw new SocketException((int)args.SocketError);

            switch (args.LastOperation)
            {
                case SocketAsyncOperation.Receive:
                    var packets = (IPacket[])null;
                    packets = ProcessReceive(args);
                    if (packets == null || packets.Length == 0)
                        break;
                    for (int i = 0; i < packets.Length; i++)
                        PacketReceived(args, packets[i]); // pass it to the handler
                    break;

                case SocketAsyncOperation.Send:
                    break;
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
            PacketDictionary.Add(new AllianceChatMessageClientPacket().ID, typeof(AllianceChatMessageClientPacket)); // 14315
            PacketDictionary.Add(new AvatarProfileRequestPacket().ID, typeof(AvatarProfileRequestPacket)); // 14325
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
