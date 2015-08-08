using System;
using System.IO;
using System.Text;

namespace CoCSharp.Networking
{
    //TODO: Change Stream to StreamReader.

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
        /// Reads a sequence of bytes from the underlying stream and advances the 
        /// position within the stream by the number of bytes read.
        /// </summary>
        /// <param name="buffer">The byte array which contains the read bytes from the underlying stream.</param>
        /// <param name="offset">The zero-based index at which to begin reading data.</param>
        /// <param name="count">The number of bytes to read.</param>
        /// <returns>The number of byte read.</returns>
        public override int Read(byte[] buffer, int offset, int count)
        {
            return BaseStream.Read(buffer, 0, count);
        }

        /// <summary>
        /// Reads a <see cref="Byte"/> from the underlying stream.
        /// </summary>
        /// <returns><see cref="Byte"/> read.</returns>
        public override int ReadByte()
        {
            return BaseStream.ReadByte();
        }

        /// <summary>
        /// Reads a <see cref="Boolean"/> from the underlying stream.
        /// </summary>
        /// <returns><see cref="Boolean"/> read.</returns>
        public bool ReadBoolean()
        {
            return ReadByte() == 1 ? true : false;
        }

        /// <summary>
        /// Reads an <see cref="Int16"/> from the underlying stream.
        /// </summary>
        /// <returns><see cref="Int16"/> read.</returns>
        public short ReadInt16()
        {
            return (short)ReadUInt16();
        }

        /// <summary>
        /// Reads a <see cref="UInt16"/> from the underlying stream.
        /// </summary>
        /// <returns><see cref="UInt16"/> read.</returns>
        public ushort ReadUInt16()
        {
            var buffer = ReadBytes(2);
            return BitConverter.ToUInt16(buffer, 0);
        }

        /// <summary>
        /// Reads a 3 bytes long int.
        /// </summary>
        /// <returns>3 bytes int.</returns>
        public int ReadPacketLength()
        {
            var packetLengthBuffer = ReadBytes(3, false);
            return ((packetLengthBuffer[0] << 16) | (packetLengthBuffer[1] << 8)) | packetLengthBuffer[2];
        }

        /// <summary>
        /// Reads an <see cref="Int32"/> from the underlying stream.
        /// </summary>
        /// <returns><see cref="Int32"/> read.</returns>
        public int ReadInt32()
        {
            return (int)ReadUInt32();
        }

        /// <summary>
        /// Reads a <see cref="UInt32"/> from the underlying stream.
        /// </summary>
        /// <returns><see cref="UInt32"/> read.</returns>
        public uint ReadUInt32()
        {
            var buffer = ReadBytes(4);
            return BitConverter.ToUInt32(buffer, 0);
        }

        /// <summary>
        /// Reads an <see cref="Int64"/> from the underlying stream.
        /// </summary>
        /// <returns><see cref="Int64"/> read.</returns>
        public long ReadInt64()
        {
            return (long)ReadUInt64();
        }

        /// <summary>
        /// Reads a <see cref="UInt64"/> from the underlying stream.
        /// </summary>
        /// <returns><see cref="UInt64"/> read.</returns>
        public ulong ReadUInt64()
        {
            var buffer = ReadBytes(8);
            return BitConverter.ToUInt64(buffer, 0);
        }

        /// <summary>
        /// Reads an array of <see cref="Byte"/> from the underlying stream.
        /// </summary>
        /// <returns>The array of <see cref="Byte"/> read.</returns>
        public byte[] ReadByteArray()
        {
            var length = ReadInt32();
            if (length < 0)
                return null;
            if (length > BaseStream.Length - BaseStream.Position)
                throw new InvalidDataException(string.Format("A byte array was larger than remaining bytes. {0} > {1}", length, BaseStream.Length - BaseStream.Position));
            var buffer = ReadBytes(length, false);
            return buffer;
        }

        /// <summary>
        /// Reads a <see cref="String"/> from the underlying stream.
        /// </summary>
        /// <returns></returns>
        public string ReadString()
        {
            var length = ReadInt32();
            if (length < 0)
                return null;
            if (length > BaseStream.Length - BaseStream.Position)
                throw new InvalidDataException(string.Format("A string was larger than remaining bytes. {0} > {1}", length, BaseStream.Length - BaseStream.Position));
            var buffer = ReadBytes(length, false);
            return Encoding.UTF8.GetString(buffer);
        }

        /// <summary>
        /// Not supposed to use this.
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new InvalidOperationException("PacketReader is not suppose to write stuff.");
        }

        /// <summary>
        /// Sets the position of the underlying stream.
        /// </summary>
        /// <param name="offset">A byte offset relative to the origin parameter.</param>
        /// <param name="origin">A value of type <see cref="SeekOrigin"/> indicating the reference point
        ///                      used to obtain the new position.</param>
        /// <returns>The new position of the underlying stream.</returns>
        public override long Seek(long offset, SeekOrigin origin)
        {
            return BaseStream.Seek(offset, origin);
        }

        /// <summary>
        /// Sets the length of the underlying stream.
        /// </summary>
        /// <param name="value">The desired length of the current stream in bytes.</param>
        public override void SetLength(long value)
        {
            BaseStream.SetLength(value);
        }

        /// <summary>
        /// Not implemented.
        /// </summary>
        public override void Flush()
        {
            throw new NotImplementedException();
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
