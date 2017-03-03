using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace CoCSharp.Network
{
    /// <summary>
    /// Provides methods to read and write on internal buffer pools.
    /// </summary>
    public class BufferStream : Stream
    {
        #region Constructors
        internal BufferStream(NetworkManagerAsyncSettings settings, Pool<byte[]> buffers)
        {
            if (settings == null)
                throw new ArgumentNullException(nameof(settings));
            if (buffers == null)
                throw new ArgumentNullException(nameof(buffers));

            _settings = settings;
            _buffers = buffers;
            _bufIndex = -1;

            TakeNewBuffer();
        }
        #endregion

        #region Fields & Properties
        internal long _len;
        // Pointer in _buf of where we're at.
        internal int _ptr;
        // Current buffer we're working with.
        internal byte[] _buf;
        // Current buffer we're working with's index in _usedBuffers.
        private int _bufIndex;

        // List of buffers that this instance have acquired.
        private readonly Pool<byte[]> _buffers;
        // NetworkManageAsyncSettings object that provides the buffers.
        private readonly NetworkManagerAsyncSettings _settings;

        /// <summary>
        /// Gets a value indicating whether the <see cref="BufferStream"/> can read.
        /// </summary>
        public override bool CanRead => true;
        /// <summary>
        /// Gets a value indicating weather the <see cref="BufferStream"/> can write.
        /// </summary>
        public override bool CanWrite => true;
        /// <summary>
        /// Gets a value indicating weather the <see cref="BufferStream"/> can seek.
        /// </summary>
        public override bool CanSeek => true;
        /// <summary>
        /// Gets the length of the <see cref="BufferStream"/>.
        /// </summary>
        public override long Length => _len;

        /// <summary>
        /// Gets or sets the position of the <see cref="BufferStream"/>.
        /// </summary>
        public override long Position
        {
            get
            {
                return (_bufIndex * _settings.BufferSize) + _ptr;
            }
            set
            {
                _bufIndex = (int)value / _settings.BufferSize;
                _buf = _buffers[_bufIndex];

                _ptr = (int)value % _settings.BufferSize;
            }
        }
        #endregion

        #region Methods
        public override void Flush()
        {
            // Space
        }

        public override void SetLength(long value)
        {
            // Space
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            switch (origin)
            {
                case SeekOrigin.Begin:
                    Position = offset;
                    break;

                case SeekOrigin.Current:
                    Position += offset;
                    break;

                case SeekOrigin.End:
                    Position = Length - offset;
                    break;
            }
            return Position;
        }

        public int Free(int count)
        {
            return -1;
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            if (buffer == null)
                throw new ArgumentNullException(nameof(buffer));
            if (offset < 0 || count < 0)
                throw new ArgumentOutOfRangeException();
            if (offset + count > buffer.Length)
                throw new ArgumentOutOfRangeException();

            // Remaining amount of bytes to copy.
            var remaining = count;

            // Copy until there are no bytes to copy.
            while (remaining > 0)
            {
                // Amount of bytes to copy.
                var cpCount = _buf.Length - _ptr;

                // Check if the data is larger than the current remaining bytes
                // in the buffer. (_buf)
                if (remaining >= cpCount)
                {
                    Buffer.BlockCopy(buffer, offset, _buf, _ptr, cpCount);

                    offset += cpCount;
                    remaining -= cpCount;

                    // Since we've filled the current buffer, we move to the next buffer
                    // which could be a new buffer from the pool or a freshly created instance or
                    // from the _usedBuffers pool.
                    NextBuffer();
                }
                else
                {
                    Buffer.BlockCopy(buffer, offset, _buf, _ptr, remaining);
                    _ptr += remaining;

                    // We've copied all of the buffer so we can exit early.
                    break;
                }
            }

            // If we've written at the end of the buffer list, it means we've written
            // on a new buffer.
            if (_bufIndex >= _buffers.Count)
                _len += count;
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            if (buffer == null)
                throw new ArgumentNullException(nameof(buffer));
            if (offset < 0 || count < 0)
                throw new ArgumentOutOfRangeException();
            if (offset + count > buffer.Length)
                throw new ArgumentOutOfRangeException();

            var remaining = count;
            while (remaining > 0)
            {
                Debug.Assert(_buf != null);

                // Amount of bytes we can copy from the current buffer.
                var cpCount = _buf.Length - _ptr;

                // If we need to copy the current buffer completely, we point
                // to the next buffer.
                if (remaining >= cpCount)
                {
                    Buffer.BlockCopy(_buf, _ptr, buffer, offset, cpCount);
                    NextBuffer();

                    //TODO: Handle remaining or out of next buffers.

                    offset += cpCount;
                    remaining -= cpCount;
                }
                else
                {
                    Buffer.BlockCopy(_buf, _ptr, buffer, offset, remaining);

                    _ptr += remaining;
                    remaining = 0;
                }
            }

            return count - remaining;
        }

        private void NextBuffer()
        {
            // If we're on the edge of used buffers we 
            // take a new one from the buffer pool.
            if (_bufIndex >= _buffers.Count)
            {
                TakeNewBuffer();
            }
            // Else we use one from the used buffers and
            // overwrite data.
            else
            {
                // Reset the pointer to point to the first byte in the current memory block.
                _ptr = 0;
                _buf = _buffers[++_bufIndex];
            }

            Debug.Assert(_buf != null);
        }

        internal void TakeNewBuffer()
        {
            // Take a new buffer from our pool of buffers or
            // create a new buffer if we don't have any.
            _buf = _settings.GetBuffer();
            _buffers.Push(_buf);

            // Reset the pointer in the current in buffer.
            _ptr = 0;
            _bufIndex++;

            Debug.WriteLine("Took new buffer from NetworkManagerAsync. {0}", args: _bufIndex);
        }

        internal string Dump()
        {
            var str = string.Empty;
            for (int i = 0; i < _buffers.Count; i++)
                str += Encoding.UTF8.GetString(_buffers[i]);
            return str;
        }
        #endregion
    }
}
