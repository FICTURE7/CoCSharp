using System.IO;
using System.Text;
namespace CoCSharp.Networking.Packets
{
    public class UnknownPacket : IPacket
    {
        public ushort ID { get; set; }
        public int Length { get; set; }
        public ushort Version { get; set; }

        public byte[] EncryptedData;
        public byte[] DecryptedData;

        public void ReadPacket(PacketReader reader)
        {
            //EncryptedData = new byte[Length];
            //reader.Read(EncryptedData, 0, Length);

            //reader.Seek(-Length, SeekOrigin.Current);

            DecryptedData = new byte[Length];
            reader.Read(DecryptedData, 0, Length);
        }

        public void WritePacket(CoCStream stream)
        {
            stream.Write(EncryptedData, 0, Length);
        }
    }
}
