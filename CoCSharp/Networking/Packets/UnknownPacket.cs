using Ionic.Zlib;
using System;
using System.Text;
using System.Text.RegularExpressions;

namespace CoCSharp.Networking.Packets
{
    public class UnknownPacket : IPacket
    {
        public ushort ID { get; set; }
        public int Length { get; set; }
        public ushort Version { get; set; }

        public byte[] EncryptedData;
        public byte[] DecryptedData;
        public string Text;
        public string HexString;

        public void ReadPacket(PacketReader reader)
        {
            DecryptedData = new byte[Length];
            reader.Read(DecryptedData, 0, Length);

            //just for debugging purposes to see what is inside
            Text = Regex.Replace(Encoding.UTF8.GetString(DecryptedData, 0, Length), @"[^\u0020-\u007F]", "."); 
            HexString = BitConverter.ToString(DecryptedData).Replace("-", " ");
           
        }

        public void WritePacket(PacketWriter writer)
        {
            writer.Write(EncryptedData, 0, EncryptedData.Length);
        }
    }
}
