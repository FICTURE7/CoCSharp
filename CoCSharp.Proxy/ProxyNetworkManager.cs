using CoCSharp.Networking.Packets;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;

namespace CoCSharp.Networking
{
    public class ProxyNetworkManager
    {
        public const int HeaderSize = 7;

        public ProxyNetworkManager(Socket connection)
        {
            if (PacketDictionary == null)
                InitializePacketDictionary();

            Connection = connection;
            CoCStream = new CoCStream(connection);
            CoCCrypto = new CoCCrypto();
        }

        public bool DataAvailable { get { return Connection.Available > 0; } }
        public CoCStream CoCStream { get; set; }

        private Socket Connection { get; set; }
        private CoCCrypto CoCCrypto { get; set; }
        private static Dictionary<ushort, Type> PacketDictionary { get; set; }

        public IPacket ReadPacket(out byte[] rawPacket, out byte[] decryptedPacket)
        {
            /* Receive data from the socket, saves it a buffer,
             * then reads packet from the buffer. 
             */

            var timeout = DateTime.Now.AddMilliseconds(500); // 500ms
            while (DataAvailable && DateTime.Now < timeout)
            {
                CoCStream.ReadToBuffer(); // reads data saves it a buffer

                //var packetBuffer = new PacketBuffer(CoCStream.ReadBuffer.ToArray());
                var enPacketReader = new PacketReader(CoCStream.ReadBuffer);

                // read header
                var packetID = enPacketReader.ReadUInt16();
                var packetLength = enPacketReader.ReadInt24();
                var packetVersion = enPacketReader.ReadUInt16();

                // read body
                if (packetLength > enPacketReader.BaseStream.Length) // check if data is enough data is avaliable in the buffer
                    continue;

                var encryptedData = GetPacketBody(packetLength);
                var decryptedData = (byte[])encryptedData.Clone(); // cloning just cause we want the encrypted data

                CoCCrypto.Decrypt(decryptedData);

                var dePacketReader = new PacketReader(new MemoryStream(decryptedData));

                var packet = CreatePacketInstance(packetID);
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

                decryptedPacket = decryptedData;
                rawPacket = ExtractRawPacket(packetLength);
                //CoCStream.ReadBuffer = new MemoryStream(4096);
                //CoCStream.Write(packetBuffer.Buffer, 0, packetBuffer.Buffer.Length);
                try
                {
                    packet.ReadPacket(dePacketReader);
                }
                catch { }
                return packet;
            }
            decryptedPacket = null;
            rawPacket = null;
            return null;
        }

        public void UpdateCiphers(ulong seed, byte[] key)
        {
            CoCCrypto.UpdateCiphers(seed, key);
        }

        private byte[] ExtractRawPacket(int packetLength)
        {
            /* Extract packet body + header from CoCStream.ReadBuffer and 
             * removes it from the stream.
             */

            var packetData = CoCStream.ReadBuffer.ToArray().Take(packetLength + HeaderSize).ToArray(); // extract packet
            var otherData = CoCStream.ReadBuffer.ToArray().Skip(packetData.Length).ToArray(); // remove packet from buffer

            CoCStream.ReadBuffer = new MemoryStream(4096);
            CoCStream.ReadBuffer.Write(otherData, 0, otherData.Length);

            return packetData;
        }

        private byte[] GetPacketBody(int packetLength)
        {
            /* Get packet body bytes from CoCStream.ReadBuffer without 
             * removing it from the stream.
             */

            var packetData = CoCStream.ReadBuffer.ToArray().Skip(HeaderSize).ToArray().Take(packetLength).ToArray(); // extract packet
            return packetData;
        }

        private static IPacket CreatePacketInstance(ushort id)
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
            PacketDictionary.Add(new AllianceInfoRequestPacket().ID, typeof(AllianceInfoRequestPacket));
            PacketDictionary.Add(new AvatarRankListRequestPacket().ID, typeof(AvatarRankListRequestPacket));
            PacketDictionary.Add(new AvatarLocalRankListRequestPacket().ID, typeof(AvatarLocalRankListRequestPacket));

            // Clientbound
            PacketDictionary.Add(new UpdateKeyPacket().ID, typeof(UpdateKeyPacket)); // 20000
            PacketDictionary.Add(new LoginSuccessPacket().ID, typeof(LoginSuccessPacket)); // 20104
            PacketDictionary.Add(new KeepAliveResponsePacket().ID, typeof(KeepAliveResponsePacket)); // 20108
            PacketDictionary.Add(new OwnHomeDataPacket().ID, typeof(OwnHomeDataPacket)); // 24101
            PacketDictionary.Add(new AllianceChatMessageServerPacket().ID, typeof(AllianceChatMessageServerPacket)); // 24312
            PacketDictionary.Add(new ChatMessageServerPacket().ID, typeof(ChatMessageServerPacket)); // 24715
            PacketDictionary.Add(new AllianceInfoResponsePacket().ID, typeof(AllianceInfoResponsePacket));
            PacketDictionary.Add(new AvatarRankListResponsePacket().ID, typeof(AvatarRankListResponsePacket));
            PacketDictionary.Add(new AvatarLocalRankListResponsePacket().ID, typeof(AvatarLocalRankListResponsePacket));
            PacketDictionary.Add(new AllianceJoinRequestFailedPacket().ID, typeof(AllianceJoinRequestFailedPacket));
            PacketDictionary.Add(new AvatarProfileResponsePacket().ID, typeof(AvatarProfileResponsePacket));
        }
    }
}

