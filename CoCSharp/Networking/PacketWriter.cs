using System;
using System.IO;
using System.Text;

namespace CoCSharp.Networking
{
    /// <summary>
    /// Implements methods to write Clash of Clans packets.
    /// </summary>
    public class PacketWriter : BinaryWriter
    {
        /// <summary>
        /// Initalizes a new instance of the <see cref="PacketWriter"/> class with
        /// the specified base <see cref="Stream"/>.
        /// </summary>
        /// <param name="baseStream">The base stream.</param>
        public PacketWriter(Stream baseStream)
            : base(baseStream)
        {
            // Space
        }

        /// <summary>
        /// Writes the specified sequence of bytes to the underlying stream and advances the
        /// position of the underlying stream by the number of bytes.
        /// </summary>
        /// <param name="buffer">The byte array which will be written to the underlying stream.</param>
        /// <param name="offset">The zero-based index at which to begin writing data.</param>
        /// <param name="count">The number of bytes to write.</param>
        public override void Write(byte[] buffer, int offset, int count)
        {
            BaseStream.Write(buffer, offset, count);
        }

        /// <summary>
        /// Writes a <see cref="Byte"/> to the underlying stream.
        /// </summary>
        /// <param name="value"><see cref="Byte"/> to write.</param>
        public void WriteByte(byte value)
        {
            BaseStream.WriteByte(value);
        }

        /// <summary>
        /// Writes a <see cref="Boolean"/> to the underlying stream.
        /// </summary>
        /// <param name="value"><see cref="Boolean"/> to write.</param>
        public void WriteBoolean(bool value)
        {
            WriteByte(value == true ? (byte)1 : (byte)0);
        }

        /// <summary>
        /// Writes an <see cref="Int16"/> to the underlying stream.
        /// </summary>
        /// <param name="value"><see cref="Int16"/> to write.</param>
        public void WriteInt16(short value)
        {
            WriteUInt16((ushort)value);
        }

        /// <summary>
        /// Writes a <see cref="UInt16"/> to the underlying stream.
        /// </summary>
        /// <param name="value"><see cref="UInt16"/> to write.</param>
        public void WriteUInt16(ushort value)
        {
            var buffer = BitConverter.GetBytes(value);
            WriteBytes(buffer);
        }

        /// <summary>
        /// Writes the given <see cref="Int32"/> as a 3 bytes long int
        /// to the underlying stream.
        /// </summary>
        /// <param name="value"><see cref="Int32"/> to write.</param>
        public void WriteInt24(int value)
        {
            var buffer = BitConverter.GetBytes(value);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(buffer);
            Write(buffer, 1, 3);
        }

        /// <summary>
        /// Writes the given <see cref="Int32"/> as a 3 bytes long int
        /// to the underlying stream.
        /// </summary>
        /// <param name="value"><see cref="Int32"/> to write.</param>
        public void WriteUInt24(uint value)
        {
            WriteInt24((int)value);
        }

        /// <summary>
        /// Writes an <see cref="Int32"/> to the underlying stream.
        /// </summary>
        /// <param name="value"><see cref="Int32"/> to write.</param>
        public void WriteInt32(int value)
        {
            WriteUInt32((uint)value);
        }

        /// <summary>
        /// Writes a <see cref="UInt32"/> to the underlying stream.
        /// </summary>
        /// <param name="value"><see cref="UInt32"/> to write.</param>
        public void WriteUInt32(uint value)
        {
            var buffer = BitConverter.GetBytes(value);
            WriteBytes(buffer);
        }

        /// <summary>
        /// Writes an <see cref="Int64"/> to the underlying stream.
        /// </summary>
        /// <param name="value"><see cref="Int64"/> to write.</param>
        public void WriteInt64(long value)
        {
            WriteUInt64((ulong)value);
        }

        /// <summary>
        /// Writes a <see cref="UInt64"/> to the underlying stream.
        /// </summary>
        /// <param name="value"><see cref="UInt64"/> to write.</param>
        public void WriteUInt64(ulong value)
        {
            var buffer = BitConverter.GetBytes(value);
            WriteBytes(buffer);
        }

        /// <summary>
        /// Writes an array of <see cref="Byte"/> to the underlying stream.
        /// </summary>
        /// <param name="value">Array of <see cref="Byte"/> to write.</param>
        public void WriteByteArray(byte[] value)
        {
            if (value == null) WriteInt32(-1);
            else
            {
                WriteInt32(value.Length);
                WriteBytes(value, false);
            }
        }

        /// <summary>
        /// Writes a <see cref="String"/> to the underlying stream.
        /// </summary>
        /// <param name="value"><see cref="String"/> to write.</param>
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

        private void WriteBytes(byte[] buffer, bool switchEndian = true)
        {
            if (BitConverter.IsLittleEndian && switchEndian) Array.Reverse(buffer);
            Write(buffer, 0, buffer.Length);
        }
    }
}
