using System;
using System.IO;
using System.Text;

namespace CoCSharp.Networking
{
    public class PacketWriter : Stream
    {
        public PacketWriter(Stream baseStream)
        {
            this.BaseStream = baseStream;
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

        public override void Write(byte[] buffer, int offset, int count)
        {
            BaseStream.Write(buffer, offset, count);
        }

        public override void WriteByte(byte value)
        {
            BaseStream.WriteByte(value);
        }

        public void WriteBool(bool value)
        {
            WriteByte(value == true ? (byte)1 : (byte)0);
        }

        public void WriteShort(short value)
        {
            WriteUShort((ushort)value);
        }

        public void WriteUShort(ushort value)
        {
            var buffer = BitConverter.GetBytes(value);
            WriteBytes(buffer);
        }

        public void WritePacketLength(int value)
        {
            var buffer = BitConverter.GetBytes(value);
            if (BitConverter.IsLittleEndian) Array.Reverse(buffer);
            Write(buffer, 0, 3);
        }

        public void WriteInt(int value)
        {
            WriteUInt((uint)value);
        }

        public void WriteUInt(uint value)
        {
            var buffer = BitConverter.GetBytes(value);
            WriteBytes(buffer);
        }

        public void WriteLong(long value)
        {
            WriteULong((ulong)value);
        }

        public void WriteULong(ulong value)
        {
            var buffer = BitConverter.GetBytes(value);
            WriteBytes(buffer);
        }

        public void WriteByteArray(byte[] value)
        {
            WriteInt(value.Length);
            WriteBytes(value, false);
        }

        public void WriteString(string value)
        {
            if (value == null) WriteInt(-1);
            else
            {
                var buffer = Encoding.UTF8.GetBytes(value);
                WriteInt(buffer.Length);
                WriteBytes(buffer, false);
            }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            throw new InvalidOperationException("PacketWriter is not suppose to write stuff.");
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

        private void WriteBytes(byte[] buffer, bool switchEndian = true)
        {
            if (BitConverter.IsLittleEndian && switchEndian) Array.Reverse(buffer);
            Write(buffer, 0, buffer.Length);
        }
    }
}
