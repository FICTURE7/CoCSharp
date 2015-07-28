using System;
using System.IO;
using System.Text;

namespace CoCSharp.Networking
{
    /// <summary>
    /// Implements methods to read Clash of Clans packets.
    /// </summary>
    public class PacketReader : Stream
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PacketReader"/> class with
        /// the specified base <see cref="Stream"/>.
        /// </summary>
        /// <param name="baseStream">The base stream.</param>
        public PacketReader(Stream baseStream)
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
        /// <returns></returns>
        public override int Read(byte[] buffer, int offset, int count)
        {
            return BaseStream.Read(buffer, 0, count);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int ReadByte()
        {
            return BaseStream.ReadByte();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool ReadBoolean()
        {
            return ReadByte() == 1 ? true : false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public short ReadInt16()
        {
            return (short)ReadUInt16();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ushort ReadUInt16()
        {
            var buffer = ReadBytes(2);
            return BitConverter.ToUInt16(buffer, 0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int ReadPacketLength()
        {
            var packetLengthBuffer = ReadBytes(3, false);
            return ((packetLengthBuffer[0] << 16) | (packetLengthBuffer[1] << 8)) | packetLengthBuffer[2];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int ReadInt32()
        {
            return (int)ReadUInt32();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public uint ReadUInt32()
        {
            var buffer = ReadBytes(4);
            return BitConverter.ToUInt32(buffer, 0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public long ReadInt64()
        {
            return (long)ReadUInt64();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ulong ReadUInt64()
        {
            var buffer = ReadBytes(8);
            return BitConverter.ToUInt64(buffer, 0);
        }

        public byte[] ReadByteArray()
        {
            var length = ReadInt32();
            var buffer = ReadBytes(length, false);
            return buffer;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string ReadString()
        {
            var length = ReadInt32();
            if (length < 0) 
                return null;
            var buffer = ReadBytes(length, false);
            return Encoding.UTF8.GetString(buffer);
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
        /// <param name="offset"></param>
        /// <param name="count"></param>
        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new InvalidOperationException("PacketReader is not suppose to write stuff.");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="count"></param>
        /// <param name="switchEndian"></param>
        /// <returns></returns>
        private byte[] ReadBytes(int count, bool switchEndian = true)
        {
            var buffer = new byte[count];
            BaseStream.Read(buffer, 0, count);
            if (BitConverter.IsLittleEndian && switchEndian) Array.Reverse(buffer); // CoC uses big-endian
            return buffer;
        }
    }
}
