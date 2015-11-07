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
    public class NetworkManagerAsync : IDisposable
    {
        private bool m_Disposed = false;

        #region Constructors & Destructors
        /// <summary>
        /// Initializes a new instance of the <see cref="NetworkManagerAsync"/> class.
        /// </summary>
        /// <param name="connection"><see cref="Socket"/> to wrap.</param>
        /// <exception cref="ArgumentNullException"/>
        public NetworkManagerAsync(Socket connection)
        {
            if (connection == null)
                throw new ArgumentNullException("connection");

            Connection = connection;
            Settings = new NetworkManagerAsyncSettings(); // default settings
            CoCCrypto = new CoCCrypto();
            ReceiveEventPool = new SocketAsyncEventArgsPool(25);
            SendEventPool = new SocketAsyncEventArgsPool(25);
            SetAsyncOperationPools();

            StartReceive(ReceiveEventPool.Pop());
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NetworkManagerAsync"/> class with
        /// the specified <see cref="NetworkManagerAsyncSettings"/>.
        /// </summary>
        /// <param name="connection"><see cref="Socket"/> to wrap.</param>
        /// <param name="settings"><see cref="NetworkManagerAsyncSettings"/> to use.</param>
        /// <exception cref="ArgumentNullException"/>
        public NetworkManagerAsync(Socket connection, NetworkManagerAsyncSettings settings)
        {
            if (connection == null)
                throw new ArgumentNullException("connection");
            if (settings == null)
                throw new ArgumentNullException("settings");

            Connection = connection;
            Settings = settings;
            CoCCrypto = new CoCCrypto();
            ReceiveEventPool = new SocketAsyncEventArgsPool(25);
            SendEventPool = new SocketAsyncEventArgsPool(25);
            SetAsyncOperationPools();

            StartReceive(ReceiveEventPool.Pop());
        }

        /// <summary>
        /// 
        /// </summary>
        ~NetworkManagerAsync()
        {
            Dispose(false);
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets whether data is available in the socket.
        /// </summary>
        public bool DataAvailable { get { return Connection.Available > 0; } }
        /// <summary>
        /// Gets whether the socket is disconnected.
        /// </summary>
        public bool IsDisconnected { get; private set; }
        /// <summary>
        /// Gets the <see cref="Socket"/> from which packets will be read and written.
        /// </summary>
        public Socket Connection { get; private set; }
        /// <summary>
        /// Gets or sets the seed for encryption.
        /// </summary>
        public int Seed { get; set; }
        /// <summary>
        /// Gets or sets the assiociated <see cref="NetworkManagerAsyncSettings"/> with
        /// this <see cref="NetworkManagerAsync"/>.
        /// </summary>
        public NetworkManagerAsyncSettings Settings { get; set; }

        private CoCCrypto CoCCrypto { get; set; }
        private PacketBufferManager BufferManager { get; set; } // we are not using this properly. :{
        private SocketAsyncEventArgsPool ReceiveEventPool { get; set; }
        private SocketAsyncEventArgsPool SendEventPool { get; set; } // we are not using this properly. :{
        #endregion

        #region Methods
        /// <summary>
        /// Sends the specified packet to the socket asynchronously.
        /// </summary>
        /// <param name="packet">The <see cref="IPacket"/> that will be sent.</param>
        /// <exception cref="ArgumentNullException"/>
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
            OnDisconnected(new DisconnectedEventArgs(null));
            IsDisconnected = true;
            Connection.Close();
        }

        private void UpdateCiphers(int seed, byte[] key)
        {
            if (key == null)
                throw new ArgumentNullException("key");

            CoCCrypto.UpdateCiphers((ulong)seed, key);
        }

        private void SetAsyncOperationPools()
        {
            BufferManager = new PacketBufferManager(Settings.ReceiveOperationCount, Settings.SendOperationCount, Settings.BufferSize);

            for (int i = 0; i < ReceiveEventPool.Capacity; i++)
            {
                var args = new SocketAsyncEventArgs();
                args.Completed += AsyncOperationCompleted;
                BufferManager.SetBuffer(args);
                PacketToken.Create(args);
                ReceiveEventPool.Push(args);
            }
            for (int i = 0; i < SendEventPool.Capacity; i++)
            {
                var args = new SocketAsyncEventArgs();
                args.Completed += AsyncOperationCompleted;
                BufferManager.SetBuffer(args);
                PacketToken.Create(args);
                SendEventPool.Push(args);
            }
        }

        private IPacket[] ProcessReceive(SocketAsyncEventArgs args)
        {
            var packetList = new List<IPacket>();
            var packetToken = args.UserToken as PacketToken;
            var bytesToProcess = args.BytesTransferred;

            if (bytesToProcess == 0)
            {
                IsDisconnected = true;
                OnDisconnected(new DisconnectedEventArgs(args));
                return null;
            }

            ReadPacket:

            // read header
            if (packetToken.HeaderReceiveOffset != PacketExtractor.HeaderSize) // we do not have the header
            {
                if (PacketExtractor.HeaderSize > bytesToProcess) // we got less that 7 bytes, some parts of the header
                {
                    // Console.WriteLine("[Net:ID {0}] Not enough bytes to read header.", packetToken.TokenID);

                    Buffer.BlockCopy(args.Buffer, packetToken.ReceiveOffset, packetToken.Header, packetToken.HeaderReceiveOffset, bytesToProcess);
                    packetToken.HeaderReceiveOffset += bytesToProcess;
                    packetToken.ReceiveOffset = args.Offset;
                    StartReceive(args);
                    return packetList.ToArray();
                }
                else // if we got more than enough data for the header
                {
                    Buffer.BlockCopy(args.Buffer, packetToken.ReceiveOffset, packetToken.Header, packetToken.HeaderReceiveOffset, PacketExtractor.HeaderSize);
                    packetToken.HeaderReceiveOffset += PacketExtractor.HeaderSize;
                    packetToken.ReceiveOffset += PacketExtractor.HeaderSize;
                    bytesToProcess -= PacketExtractor.HeaderSize;
                    ProcessToken(packetToken);
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
                    packetToken.ReceiveOffset = args.Offset;
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

            var packet = PacketFactory.Create(packetToken.ID);
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
                    OnPacketReceived(new PacketReceivedEventArgs(packet, ex));
                    packetToken.Reset();
                    if (bytesToProcess != 0)
                        goto ReadPacket;
                }
            }

            if (packet is UpdateKeyPacket)
                UpdateCiphers(Seed, ((UpdateKeyPacket)packet).Key);

            packetList.Add(packet);
            packetToken.Reset();
            if (bytesToProcess != 0)
                goto ReadPacket;

            packetToken.ReceiveOffset = args.Offset;
            ReceiveEventPool.Push(args);
            StartReceive(ReceiveEventPool.Pop());
            return packetList.ToArray();
        }

        private static void ProcessToken(PacketToken token)
        {
            token.ID = (ushort)((token.Header[0] << 8) | (token.Header[1]));
            token.Length = (token.Header[2] << 16) | (token.Header[3] << 8) | (token.Header[4]);
            token.Version = (ushort)((token.Header[5] << 8) | (token.Header[6]));
            token.Body = new byte[token.Length];
        }

        private void StartReceive(SocketAsyncEventArgs args)
        {
            if (!Connection.ReceiveAsync(args))
                AsyncOperationCompleted(Connection, args);
        }

        private void AsyncOperationCompleted(object sender, SocketAsyncEventArgs args)
        {
            if (args.SocketError != SocketError.Success)
            {
                IsDisconnected = true;
                OnDisconnected(new DisconnectedEventArgs(args));
            }

            switch (args.LastOperation)
            {
                case SocketAsyncOperation.Receive:
                    var packets = (IPacket[])null;
                    packets = ProcessReceive(args);
                    if (packets == null || packets.Length == 0)
                        break;
                    for (int i = 0; i < packets.Length; i++)
                        OnPacketReceived(new PacketReceivedEventArgs(packets[i], null));
                    break;

                case SocketAsyncOperation.Send:
                    //TODO: Handle large packets.
                    break;

                case SocketAsyncOperation.Disconnect:
                    //TODO: Handle disconnect ops.
                    break;
            }
        }
        #endregion

        #region Events
        /// <summary>
        /// The event raised when a packet was received.
        /// </summary>
        public event EventHandler<PacketReceivedEventArgs> PacketReceived;
        /// <summary>
        /// Use this methods to fire up <see cref="PacketReceived"/> event.
        /// </summary>
        /// <param name="args"></param>
        protected internal void OnPacketReceived(PacketReceivedEventArgs args)
        {
            if (PacketReceived != null)
                PacketReceived(this, args);
        }

        /// <summary>
        /// The event raised when the socket was disconnected.
        /// </summary>
        public event EventHandler<DisconnectedEventArgs> Disconnected;
        /// <summary>
        /// Use this methods to fire up <see cref="Disconnected"/> event.
        /// </summary>
        /// <param name="args"></param>
        protected internal void OnDisconnected(DisconnectedEventArgs args)
        {
            if (Disconnected != null)
                Disconnected(this, args);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (m_Disposed)
                return;

            if (disposing)
            {
                Connection.Close();
                ReceiveEventPool.Dispose();
                SendEventPool.Dispose();
            }
            m_Disposed = true;
        }
        #endregion
    }
}
