using System;
using System.IO;
using System.Text;

namespace CoCSharp.Networking
{
    public class PacketWriter : Stream
    {
        /// <summary>
        /// Initalizes a new instance of the <see cref="PacketWriter"/> class with
        /// the specified base <see cref="Stream"/>.
        /// </summary>
        /// <param name="baseStream">The base stream.</param>
        public PacketWriter(Stream baseStream)
        {
            BaseStream = baseStream;
        }

        /// <summary>
        /// Gets a value indicating whether the underlying stream suppors reading.
        /// </summary>
        public override bool CanRead { get { return BaseStream.CanRead; } }
        /// <summary>
        /// Gets a value indicating whether the underlying stream suppors writing.
        /// </summary>
        public override bool CanWrite { get { return BaseStream.CanWrite; } }
        /// <summary>
        /// Gets a value indicating whether the underlying stream suppors seeking.
        /// </summary>
        public override bool CanSeek { get { return BaseStream.CanSeek; } }
        /// <summary>
        /// Gets the length of the underlying stream in bytes.
        /// </summary>
        public override long Length { get { return BaseStream.Length; } }
        /// <summary>
        /// Gets or sets the current position in the underlying stream.
        /// </summary>
        public override long Position
        {
            get { return BaseStream.Position; }
            set { BaseStream.Position = value; }
        }

        /// <summary>
        /// Gets the underlying stream.
        /// </summary>
        public Stream BaseStream { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        public override void Write(byte[] buffer, int offset, int count)
        {
            BaseStream.Write(buffer, offset, count);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public override void WriteByte(byte value)
        {
            BaseStream.WriteByte(value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public void WriteBoolean(bool value)
        {
            WriteByte(value == true ? (byte)1 : (byte)0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public void WriteInt16(short value)
        {
            WriteUInt16((ushort)value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public void WriteUInt16(ushort value)
        {
            var buffer = BitConverter.GetBytes(value);
            WriteBytes(buffer);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public void WritePacketLength(int value)
        {
            var buffer = BitConverter.GetBytes(value);
            if (BitConverter.IsLittleEndian) Array.Reverse(buffer);
            Write(buffer, 0, 3);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public void WriteInt32(int value)
        {
            WriteUInt32((uint)value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public void WriteUInt32(uint value)
        {
            var buffer = BitConverter.GetBytes(value);
            WriteBytes(buffer);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public void WriteInt64(long value)
        {
            WriteUInt64((ulong)value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public void WriteUInt64(ulong value)
        {
            var buffer = BitConverter.GetBytes(value);
            WriteBytes(buffer);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public void WriteByteArray(byte[] value)
        {
            WriteInt32(value.Length);
            WriteBytes(value, false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public void WriteString(string value)
        {
            if (value == null) WriteInt32(-1);
            else
            {
                var buffer = Encoding.UTF8.GetBytes(value);
                WriteInt32(buffer.Length);
                WriteBytes(buffer, false);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public override int Read(byte[] buffer, int offset, int count)
        {
            throw new InvalidOperationException("PacketWriter is not suppose to write stuff.");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="origin"></param>
        /// <returns></returns>
        public override long Seek(long offset, SeekOrigin origin)
        {
            return BaseStream.Seek(offset, origin);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public override void SetLength(long value)
        {
            BaseStream.SetLength(value);
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Flush()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="switchEndian"></param>
        private void WriteBytes(byte[] buffer, bool switchEndian = true)
        {
            if (BitConverter.IsLittleEndian && switchEndian) Array.Reverse(buffer);
            Write(buffer, 0, buffer.Length);
        }
    }
}
