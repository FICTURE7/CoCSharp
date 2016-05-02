using System;
using System.IO;
using System.Text;

namespace CoCSharp.Network
{
    /// <summary>
    /// Wrapper of <see cref="BinaryWriter"/> that implements methods to write Clash of Clans messages.
    /// </summary>
    public class MessageWriter : BinaryWriter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MessageWriter"/> class.
        /// </summary>
        public MessageWriter() : base()
        {
            // Space
        }

        private bool _disposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageWriter"/> class
        /// based on the specified stream.
        /// </summary>
        /// <param name="output">The output stream.</param>
        public MessageWriter(Stream output) : base(output)
        {
            // Space
        }

        /// <summary>
        /// Writes a decimal value to the current stream and advances the stream position by sixteen bytes.
        /// The decimal value is casted to a double.
        /// </summary>
        /// <param name="value">The decimal value to write.</param>
        /// <exception cref="ObjectDisposedException">The <see cref="MessageWriter"/> is closed.</exception>
        public override void Write(decimal value)
        {
            CheckDispose();

            Write((double)value);
        }

        /// <summary>
        /// Writes an 8-byte floating-point value to the current stream and advances the stream position by eight bytes.
        /// </summary>
        /// <param name="value">The 8-byte floating-point value.</param>
        /// <exception cref="ObjectDisposedException">The <see cref="MessageWriter"/> is closed.</exception>
        public override void Write(double value)
        {
            CheckDispose();

            var buffer = BitConverter.GetBytes(value);
            WriteByteArrayEndian(buffer);
        }

        /// <summary>
        /// Writes an 8-byte signed integer to the current stream and advances the stream position by eight bytes.
        /// </summary>
        /// <param name="value">The 8-byte signed integer to write.</param>
        /// <exception cref="ObjectDisposedException">The <see cref="MessageWriter"/> is closed.</exception>
        public override void Write(long value)
        {
            CheckDispose();

            Write((ulong)value);
        }

        /// <summary>
        /// Writes an 8-byte unsigned integer to the current stream and advances the stream position by eight bytes.
        /// </summary>
        /// <param name="value">The 8-byte unsigned integer to write.</param>
        /// <exception cref="ObjectDisposedException">The <see cref="MessageWriter"/> is closed.</exception>
        public override void Write(ulong value)
        {
            CheckDispose();

            var buffer = BitConverter.GetBytes(value);
            WriteByteArrayEndian(buffer);
        }

        /// <summary>
        /// Writes a 4-byte floating-point value to the current stream and advances the stream position by four bytes.
        /// </summary>
        /// <param name="value">The 4-byte floating-point value to write.</param>
        /// <exception cref="ObjectDisposedException">The <see cref="MessageWriter"/> is closed.</exception>
        public override void Write(float value)
        {
            CheckDispose();

            var buffer = BitConverter.GetBytes(value);
            WriteByteArrayEndian(buffer);
        }

        /// <summary>
        /// Writes a 4-byte signed integer to the current stream and advances the stream position by four bytes.
        /// </summary>
        /// <param name="value">The 4-byte signed integer to write.</param>
        /// <exception cref="ObjectDisposedException">The <see cref="MessageWriter"/> is closed.</exception>
        public override void Write(int value)
        {
            CheckDispose();

            Write((uint)value);
        }

        /// <summary>
        /// Writes a 4-byte unsigned integer to the current stream and advances the stream position by four bytes.
        /// </summary>
        /// <param name="value">The 4-byte unsigned integer to write.</param>
        /// <exception cref="ObjectDisposedException">The <see cref="MessageWriter"/> is closed.</exception>
        public override void Write(uint value)
        {
            CheckDispose();

            var buffer = BitConverter.GetBytes(value);
            WriteByteArrayEndian(buffer);
        }

        /// <summary>
        /// Writes a 2-byte signed integer to the current stream and advances the stream position by two bytes.
        /// </summary>
        /// <param name="value">The 2-byte signed integer to write.</param>
        /// <exception cref="ObjectDisposedException">The <see cref="MessageWriter"/> is closed.</exception>
        public override void Write(short value)
        {
            CheckDispose();

            Write((ushort)value);
        }

        /// <summary>
        /// Writes a 2-byte unsigned integer to the current stream and advances the stream position by two bytes.
        /// </summary>
        /// <param name="value">The 2-byte unsigned integer to write.</param>
        /// <exception cref="ObjectDisposedException">The <see cref="MessageWriter"/> is closed.</exception>
        public override void Write(ushort value)
        {
            CheckDispose();

            var buffer = BitConverter.GetBytes(value);
            WriteByteArrayEndian(buffer);
        }

        /// <summary>
        /// Writes a length-prefixed string encoded in UTF-8 to the current stream and advances the stream position
        /// by the length of the string and the length of the prefix which is 4 bytes long.
        /// </summary>
        /// <param name="value">The string to write.</param>
        /// <exception cref="ObjectDisposedException">The <see cref="MessageWriter"/> is closed.</exception>
        public override void Write(string value)
        {
            CheckDispose();

            if (value == null)
                Write(-1);
            else
            {
                var buffer = Encoding.UTF8.GetBytes(value);
                Write(buffer.Length);
                Write(buffer);
            }
        }

        /// <summary>
        /// Writes a 1-byte boolean to the current stream and advances the stream position by one byte.
        /// </summary>
        /// <param name="value">The boolean to write.</param>
        public override void Write(bool value)
        {
            CheckDispose();

            base.Write(value);
        }

        /// <summary>
        /// Writes a byte array to the underlying stream and a four-byte signed integer
        /// of the length of the byte array if prefixed is set to <c>true</c>.
        /// </summary>
        /// <param name="buffer">A byte array containing the data to write. </param>
        /// <param name="prefixed">
        /// If set to <c>true</c> a 4-byte signed integer of the length of the byte array
        /// will be written.
        /// </param>
        /// <exception cref="ObjectDisposedException">The <see cref="MessageWriter"/> is closed.</exception>
        public void Write(byte[] buffer, bool prefixed)
        {
            CheckDispose();

            Write(buffer, 0, buffer.Length, prefixed);
        }

        /// <summary>
        /// Writes a region of a byte array to the current stream and a four-byte signed integer
        /// of the length of the byte array if prefixed is set to <c>true</c>.
        /// </summary>
        /// <param name="buffer">A byte array containing the data to write.</param>
        /// <param name="index">The starting point in <paramref name="buffer"/> at which to begin writing.</param>
        /// <param name="count">The number of bytes to write.</param>
        /// <param name="prefixed">
        /// If set to <c>true</c>, a 4-byte signed integer of the length of the byte array
        /// will be written.
        /// </param>
        /// <exception cref="ObjectDisposedException">The <see cref="MessageWriter"/> is closed.</exception>
        public void Write(byte[] buffer, int index, int count, bool prefixed)
        {
            CheckDispose();

            if (!prefixed)
                Write(buffer, index, count);
            else
            {
                Write(buffer.Length);
                Write(buffer, index, count);
            }
        }

        /// <summary>
        /// Releases all resources used by the current instance of the <see cref="MessageWriter"/> class.
        /// </summary>
        public new void Dispose()
        {
            if (!_disposed)
            {
                Dispose(true);
                _disposed = true;
            }
        }

        private void CheckDispose()
        {
            if (_disposed)
                throw new ObjectDisposedException(null, "Cannot access the MessageWriter object because it was disposed.");
        }

        private void WriteByteArrayEndian(byte[] buffer)
        {
            if (BitConverter.IsLittleEndian)
                Array.Reverse(buffer);
            Write(buffer);
        }
    }
}
