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
        public const int HeaderSize = 7;

        public NetworkManager(TcpClient client)
        {
            this.Client = client;
            this.CoCStream = new CoCStream(client);
            this.CoCCrypto = new CoCCrypto();

            if (PacketDictionary == null) InitializePacketDictionary(); // intialize dictionary
        }

        public bool DataAvailable { get { return Client.Available > 0; } }
        public CoCStream CoCStream { get; set; }

        private TcpClient Client { get; set; }
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

                var enPacketReader = new PacketReader(CoCStream.ReadBuffer);

                // read header
                var packetID = enPacketReader.ReadUShort();
                var packetLength = enPacketReader.ReadPacketLength();
                var packetVersion = enPacketReader.ReadUShort();

                // read body
                if (packetLength > enPacketReader.Length) // check if data is enough data is avaliable in the buffer
                    continue;

                var encryptedData = GetPacketBody(packetLength);
                var decryptedData = (byte[])encryptedData.Clone(); // cloning just cause we want the encrypted data

                CoCCrypto.Decrypt(decryptedData);

                var dePacketReader = new PacketReader(new MemoryStream(decryptedData));

                var packet = GetPacket(packetID);
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
                rawPacket = ExtractRawPacket(packetLength); // raw encrypted packet
                packet.ReadPacket(dePacketReader);
                return packet;
            }
            decryptedPacket = null;
            rawPacket = null;
            return null;
        }

        public void WritePacket(IPacket packet)
        {
            /* Writes packet to a buffer,
             * then sends the buffer to the socket
             */

            throw new NotImplementedException();
        }

        public void UpdateChipers(ulong seed, byte[] key)
        {
            CoCCrypto.UpdateChipers(seed, key);
        }

        //TODO: Implement this in CoCStream
        private byte[] ExtractRawPacket(int packetLength)
        {
            /* Extract packet body + header from CoCStream.ReadBuffer and 
             * removes it from the stream.
             */

            var packetData = CoCStream.ReadBuffer.ToArray().Take(packetLength + HeaderSize).ToArray(); // extract packet
            var otherData = CoCStream.ReadBuffer.ToArray().Skip(packetData.Length).ToArray(); // remove packet from buffer

            CoCStream.ReadBuffer = new MemoryStream(4096); // clear buffer
            CoCStream.ReadBuffer.Write(otherData, 0, otherData.Length);

            return packetData;
        }

        //TODO: Implement this in CoCStream
        private byte[] GetPacketBody(int packetLength)
        {
            /* Get packet body bytes from CoCStream.ReadBuffer without 
             * removing it from the stream.
             */

            var packetData = CoCStream.ReadBuffer.ToArray().Skip(HeaderSize).ToArray().Take(packetLength).ToArray(); // extract packet
            return packetData;
        }

        private static IPacket GetPacket(ushort id)
        {
            var packetType = (Type)null;
            var packet = (IPacket)null;

            if (PacketDictionary.TryGetValue(id, out packetType)) packet = (IPacket)Activator.CreateInstance(packetType);
            else packet = new UnknownPacket();

            return packet;
        }

        private static void InitializePacketDictionary()
        {
            PacketDictionary = new Dictionary<ushort, Type>();

            // Serverbound
            PacketDictionary.Add(new LoginRequestPacket().ID, typeof(LoginRequestPacket)); // 10101
            PacketDictionary.Add(new KeepAliveRequestPacket().ID, typeof(KeepAliveRequestPacket)); // 10108
            PacketDictionary.Add(new SetDeviceToken().ID, typeof(SetDeviceToken)); // 10113
            PacketDictionary.Add(new ChangeAvatarName().ID, typeof(ChangeAvatarName)); // 10212
            PacketDictionary.Add(new BindFacebookAccount().ID, typeof(BindFacebookAccount)); // 14201
            PacketDictionary.Add(new AvatarProfileRequest().ID, typeof(AvatarProfileRequest)); // 14325
            PacketDictionary.Add(new ChatMessageClientPacket().ID, typeof(ChatMessageClientPacket)); // 14715

            // Clientbound
            PacketDictionary.Add(new UpdateKeyPacket().ID, typeof(UpdateKeyPacket)); // 20000
            PacketDictionary.Add(new KeepAliveResponsePacket().ID, typeof(KeepAliveResponsePacket)); // 20108
            PacketDictionary.Add(new ChatMessageServerPacket().ID, typeof(ChatMessageServerPacket)); // 24715
            PacketDictionary.Add(new LoginSuccessPacket().ID, typeof(LoginSuccessPacket)); // 20104
            PacketDictionary.Add(new OwnHomeData().ID, typeof(OwnHomeData)); // 24101
        }
    }
}
