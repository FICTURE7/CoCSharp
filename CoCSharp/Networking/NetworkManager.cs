using System;

namespace CoCSharp.Networking
{
    public class NetworkManager
    {
        public NetworkManager()
        {
<<<<<<< HEAD
            throw new NotImplementedException();
=======
            this.Connection = connection;
            this.CoCStream = new CoCStream(connection);
            this.CoCCrypto = new CoCCrypto();

            if (PacketDictionary == null) InitializePacketDictionary(); // intialize dictionary
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
            MemoryStream tempStream = new MemoryStream();
            packet.WritePacket(new PacketWriter(tempStream));
            byte[] buffer = new byte[tempStream.Length];
            tempStream.Read(buffer, 0, buffer.Length);
            buffer = (buffer.Skip(HeaderSize).ToArray());
            CoCCrypto.Encrypt(buffer);
            CoCStream.Write(buffer, 0, buffer.Length);
>>>>>>> master
        }
    }
}
