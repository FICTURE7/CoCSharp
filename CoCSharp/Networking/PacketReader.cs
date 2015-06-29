using System;
using System.IO;
using System.Text;

namespace CoCSharp.Networking
{
    public class PacketReader : Stream
    {
        public PacketReader(Stream stream)
        {
            this.BaseStream = stream;
        }

        public override bool CanRead { get { return BaseStream.CanRead; } }
        public override bool CanWrite { get { return BaseStream.CanWrite; } }
        public override bool CanSeek { get { return BaseStream.CanSeek; } }
        public override long Length { get { return BaseStream.Length; } }
        public override long Position
        {
            get { return BaseStream.Position; }
            set { BaseStream.Position = value; }
        }

        public Stream BaseStream { get; set; }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return BaseStream.Read(buffer, 0, count);
        }

        public override int ReadByte()
        {
            return BaseStream.ReadByte();
        }

        public bool ReadBool()
        {
            return ReadByte() == 1 ? true : false;
        }

        public short ReadShort()
        {
            return (short)ReadUShort();
        }

        public ushort ReadUShort()
        {
            var buffer = ReadBytes(2);
            return BitConverter.ToUInt16(buffer, 0);
        }

        public int ReadPacketLength()
        {
            var packetLengthBuffer = ReadBytes(3, false);
            return ((packetLengthBuffer[0] << 16) | (packetLengthBuffer[1] << 8)) | packetLengthBuffer[2];
        }

        public int ReadInt()
        {
            return (int)ReadUInt();
        }

        public uint ReadUInt()
        {
            var buffer = ReadBytes(4);
            return BitConverter.ToUInt32(buffer, 0);
        }

        public long ReadLong()
        {
            return (long)ReadULong();
        }

        public ulong ReadULong()
        {
            var buffer = ReadBytes(8);
            return BitConverter.ToUInt64(buffer, 0);
        }

        public byte[] ReadByteArray()
        {
            var length = ReadInt();
            var buffer = ReadBytes(length, false);
            return buffer;
        }

        public string ReadString()
        {
            var length = ReadInt();
            if (length < 0) return null;
            var buffer = ReadBytes(length, false);
            return Encoding.UTF8.GetString(buffer);
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return BaseStream.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            BaseStream.SetLength(value);
        }

        public override void Flush()
        {
            throw new NotImplementedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new InvalidOperationException("PacketReader is not suppose to write stuff.");
        }

        private byte[] ReadBytes(int count, bool switchEndian = true)
        {
            var buffer = new byte[count];
            BaseStream.Read(buffer, 0, count);
            if (BitConverter.IsLittleEndian && switchEndian) Array.Reverse(buffer); // CoC uses big-endian
            return buffer;
        }
    }
}
