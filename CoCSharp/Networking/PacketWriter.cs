using System;
using System.IO;
using System.Text;

namespace CoCSharp.Networking
{
    //TODO: Change Stream to StreamWriter.

    /// <summary>
    /// Implements methods to write Clash of Clans packets.
    /// </summary>
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
        public override void WriteByte(byte value)
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
        public void WritePacketLength(int value)
        {
            var buffer = BitConverter.GetBytes(value);
            if (BitConverter.IsLittleEndian) 
                Array.Reverse(buffer);
            Write(buffer, 1, 3);
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
            WriteInt32(value.Length);
            WriteBytes(value, false);
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
        /// Not supposed to use this.
        /// </summary>
        /// <param name="buffer">Why are you even reading this.</param>
        /// <param name="offset">Lol?</param>
        /// <param name="count">xD</param>
        /// <returns>Well, I warned you.</returns>
        public override int Read(byte[] buffer, int offset, int count)
        {
            throw new InvalidOperationException("PacketWriter is not suppose to write.");
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

        private void WriteBytes(byte[] buffer, bool switchEndian = true)
        {
            if (BitConverter.IsLittleEndian && switchEndian) Array.Reverse(buffer);
            Write(buffer, 0, buffer.Length);
        }
    }
}
