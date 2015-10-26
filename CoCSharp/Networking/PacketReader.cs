using System;
using System.IO;
using System.Text;

namespace CoCSharp.Networking
{
    /// <summary>
    /// Implements methods to read Clash of Clans packets.
    /// </summary>
    public class PacketReader : BinaryReader
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PacketReader"/> class with
        /// the specified base <see cref="Stream"/>.
        /// </summary>
        /// <param name="stream">The base stream.</param>
        public PacketReader(Stream stream)
            : base(stream)
        {
            // Space
        }

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
        public override byte ReadByte()
        {
            return (byte)BaseStream.ReadByte();
        }

        /// <summary>
        /// Reads a <see cref="Boolean"/> from the underlying stream.
        /// </summary>
        /// <returns><see cref="Boolean"/> read.</returns>
        public override bool ReadBoolean()
        {
            var state = ReadByte();
            switch (state)
            {
                case 1:
                    return true;
                case 0:
                    return false;
                default:
                    throw new InvalidPacketException("A boolean had an incorrect value: " + state + ".");
            }
        }

        /// <summary>
        /// Reads an <see cref="Int16"/> from the underlying stream.
        /// </summary>
        /// <returns><see cref="Int16"/> read.</returns>
        public override short ReadInt16()
        {
            return (short)ReadUInt16();
        }

        /// <summary>
        /// Reads a <see cref="UInt16"/> from the underlying stream.
        /// </summary>
        /// <returns><see cref="UInt16"/> read.</returns>
        public override ushort ReadUInt16()
        {
            var buffer = ReadBytesWithEndian(2);
            return BitConverter.ToUInt16(buffer, 0);
        }

        /// <summary>
        /// Reads a 3 bytes long int. Clash of Clans packets uses this to encode there length.
        /// </summary>
        /// <returns>3 bytes int.</returns>
        public int ReadInt24()
        {
            var packetLengthBuffer = ReadBytesWithEndian(3, false);
            return ((packetLengthBuffer[0] << 16) | (packetLengthBuffer[1] << 8)) | packetLengthBuffer[2];
        }

        /// <summary>
        /// Reads a 3 bytes long uint.
        /// </summary>
        /// <returns>3 bytes int.</returns>
        public uint ReadUInt24()
        {
            return (uint)ReadInt24();
        }

        /// <summary>
        /// Reads an <see cref="Int32"/> from the underlying stream.
        /// </summary>
        /// <returns><see cref="Int32"/> read.</returns>
        public override int ReadInt32()
        {
            return (int)ReadUInt32();
        }

        /// <summary>
        /// Reads a <see cref="UInt32"/> from the underlying stream.
        /// </summary>
        /// <returns><see cref="UInt32"/> read.</returns>
        public override uint ReadUInt32()
        {
            var buffer = ReadBytesWithEndian(4);
            return BitConverter.ToUInt32(buffer, 0);
        }

        /// <summary>
        /// Reads an <see cref="Int64"/> from the underlying stream.
        /// </summary>
        /// <returns><see cref="Int64"/> read.</returns>
        public override long ReadInt64()
        {
            return (long)ReadUInt64();
        }

        /// <summary>
        /// Reads a <see cref="UInt64"/> from the underlying stream.
        /// </summary>
        /// <returns><see cref="UInt64"/> read.</returns>
        public override ulong ReadUInt64()
        {
            var buffer = ReadBytesWithEndian(8);
            return BitConverter.ToUInt64(buffer, 0);
        }

        /// <summary>
        /// Reads an array of <see cref="Byte"/> from the underlying stream.
        /// </summary>
        /// <returns>The array of <see cref="Byte"/> read.</returns>
        public byte[] ReadByteArray()
        {
            var length = ReadInt32();
            if (length == -1)
                return null;
            if (length < -1)
                throw new InvalidPacketException("A byte array length was incorrect: " + length + ".");
            if (length > BaseStream.Length - BaseStream.Position)
                throw new InvalidPacketException(string.Format("A byte array was larger than remaining bytes. {0} > {1}.", length, BaseStream.Length - BaseStream.Position));
            var buffer = ReadBytesWithEndian(length, false);
            return buffer;
        }

        /// <summary>
        /// Reads a <see cref="String"/> from the underlying stream.
        /// </summary>
        /// <returns></returns>
        public override string ReadString()
        {
            var length = ReadInt32();
            if (length == -1)
                return null;
            if (length < -1)
                throw new InvalidPacketException("A string length was incorrect: " + length);
            if (length > BaseStream.Length - BaseStream.Position)
                throw new InvalidPacketException(string.Format("A string was larger than remaining bytes. {0} > {1}.", length, BaseStream.Length - BaseStream.Position));
            var buffer = ReadBytesWithEndian(length, false);
            return Encoding.UTF8.GetString(buffer);
        }

        /// <summary>
        /// Sets the position of the underlying stream.
        /// </summary>
        /// <param name="offset">A byte offset relative to the origin parameter.</param>
        /// <param name="origin">A value of type <see cref="SeekOrigin"/> indicating the reference point
        ///                      used to obtain the new position.</param>
        /// <returns>The new position of the underlying stream.</returns>
        public long Seek(long offset, SeekOrigin origin)
        {
            return BaseStream.Seek(offset, origin);
        }

        private byte[] ReadBytesWithEndian(int count, bool switchEndian = true)
        {
            var buffer = new byte[count];
            BaseStream.Read(buffer, 0, count);
            if (BitConverter.IsLittleEndian && switchEndian)
                Array.Reverse(buffer);
            return buffer;
        }
    }
}
