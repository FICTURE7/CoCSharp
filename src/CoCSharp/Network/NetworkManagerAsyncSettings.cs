using System;
using System.Diagnostics;
using System.Net.Sockets;
using System.Threading;

namespace CoCSharp.Network
{
    /// <summary>
    /// Provides settings for the <see cref="NetworkManagerAsync"/> class. It is recommended to use
    /// it for better management of <see cref="SocketAsyncEventArgs"/> objects.
    /// </summary>
    public class NetworkManagerAsyncSettings : IDisposable
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="NetworkManagerAsync"/> class
        /// with default settings.
        /// </summary>
        public NetworkManagerAsyncSettings()
            : this(25, 25, 4096)
        {
            // Space
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NetworkManagerAsyncSettings"/> class
        /// with the specified number of receive operation <see cref="SocketAsyncEventArgs"/> objects
        /// and the specified number of send operation <see cref="SocketAsyncEventArgs"/> objects.
        /// </summary>
        /// <param name="receiveCount">Number of receive operation <see cref="SocketAsyncEventArgs"/> objects.</param>
        /// <param name="sendCount">Number of send operation <see cref="SocketAsyncEventArgs"/> objects.</param>
        public NetworkManagerAsyncSettings(int receiveCount, int sendCount)
            : this(receiveCount, sendCount, 4096)
        {
            // Space
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NetworkManagerAsyncSettings"/> class
        /// with the specified number of receive operation <see cref="SocketAsyncEventArgs"/> objects
        /// and the specified number of send operation <see cref="SocketAsyncEventArgs"/> objects with
        /// the specified buffer size of each <see cref="SocketAsyncEventArgs"/> object.
        /// </summary>
        /// <param name="receiveCount">Number of receive operation <see cref="SocketAsyncEventArgs"/> objects.</param>
        /// <param name="sendCount">Number of send operation <see cref="SocketAsyncEventArgs"/> objects.</param>
        /// <param name="bufferSize">Buffer size of each <see cref="SocketAsyncEventArgs"/> object.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="bufferSize"/> less than 1.</exception>
        public NetworkManagerAsyncSettings(int receiveCount, int sendCount, int bufferSize)
        {
            if (bufferSize < 1)
                throw new ArgumentOutOfRangeException(nameof(bufferSize), "Buffer size cannot be less than 1.");

            _bufferSize = bufferSize;
            _statistics = new NetworkManagerAsyncStatistics();

            // Don't waste IO threads.
            var concurrentOpsCount = Environment.ProcessorCount * 2;
            _concurrentOps = new Semaphore(concurrentOpsCount, concurrentOpsCount);
            _receivePool = new SocketAsyncEventArgsPool(receiveCount);
            _sendPool = new SocketAsyncEventArgsPool(sendCount);
            _bufferManager = new MessageBufferManager(receiveCount, sendCount, bufferSize);

            for (int i = 0; i < ReceiveCount; i++)
            {
                var args = new SocketAsyncEventArgs();
                _bufferManager.SetBuffer(args);
                MessageReceiveToken.Create(args);
                _receivePool.Push(args);
            }

            for (int i = 0; i < SendCount; i++)
            {
                var args = new SocketAsyncEventArgs();
                _bufferManager.SetBuffer(args);
                MessageSendToken.Create(args);
                _sendPool.Push(args);
            }

            _buffers = new Pool<byte[]>(64);
            _args = new Pool<SocketAsyncEventArgs>(receiveCount + sendCount);
        }
        #endregion

        #region Fields & Properties
        private bool _disposed;
        private readonly int _bufferSize;
        private readonly NetworkManagerAsyncStatistics _statistics;

        internal readonly Semaphore _concurrentOps;

        [Obsolete]
        internal readonly MessageBufferManager _bufferManager;
        [Obsolete]
        internal readonly SocketAsyncEventArgsPool _receivePool;
        [Obsolete]
        internal readonly SocketAsyncEventArgsPool _sendPool;

        // Pool of byte array buffers.
        internal readonly Pool<byte[]> _buffers;
        // Pool of SocketAsyncEventArgs.
        internal readonly Pool<SocketAsyncEventArgs> _args;

        /// <summary>
        /// Gets a new instance of the default state of the <see cref="NetworkManagerAsyncSettings"/> class.
        /// </summary>
        public static NetworkManagerAsyncSettings DefaultSettings => new NetworkManagerAsyncSettings();

        /// <summary>
        /// Gets the number of receive operation <see cref="SocketAsyncEventArgs"/> objects
        /// being used.
        /// </summary>
        public int ReceiveCount => _receivePool.Capacity;

        /// <summary>
        /// Gets the number of send operation <see cref="SocketAsyncEventArgs"/> objects
        /// being used.
        /// </summary>
        public int SendCount => _receivePool.Capacity;

        /// <summary>
        /// Gets the buffer size of each <see cref="SocketAsyncEventArgs"/> object.
        /// </summary>
        public int BufferSize => _bufferSize;

        /// <summary>
        /// Gets the <see cref="NetworkManagerAsyncStatistics"/> associated with this
        /// <see cref="NetworkManagerAsyncSettings"/> and represents the overall stats of <see cref="NetworkManagerAsync"/> instances
        /// associated with the current <see cref="NetworkManagerAsyncSettings"/>.
        /// </summary>
        public NetworkManagerAsyncStatistics Statistics => _statistics;
        #endregion

        #region Methods
        internal byte[] GetBuffer()
        {
            return _buffers.Pop() ?? new byte[BufferSize];
        }

        internal SocketAsyncEventArgs GetArgs()
        {
            return _args.Pop() ?? new SocketAsyncEventArgs();
        }

        internal void Recyle(byte[] buffer)
        {
            // Push back the buffer to the pool.
            if (buffer != null)
                _buffers.Push(buffer);
        }

        internal void Recyle(SocketAsyncEventArgs args)
        {
            if (args != null)
            {
                // Set AcceptSocket to null.
                args.AcceptSocket = null;

                // If the SocketAsyncEventArgs has a buffer associated with it
                // we recycle it as well.
                if (args.Buffer != null)
                {
                    Debug.Assert(args.Buffer.Length == BufferSize);

                    // Push buffer back and set to null.
                    _buffers.Push(args.Buffer);
                    args.SetBuffer(null, 0, 0);
                }

                _args.Push(args);
            }
            else
            {
                Debug.Write("Tried to recycle a null SocketAsyncEventArgs.");
            }
        }

        /// <summary>
        /// Releases all resources used by the current instance of the <see cref="NetworkManagerAsyncSettings"/> class.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }

        /// <summary>
        /// Releases unmanaged resources and optionally disposes managed resources.
        /// </summary>
        /// <param name="disposing">If set to <c>true</c>; it will dispose managed resources as well.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _receivePool.Dispose();
                    _sendPool.Dispose();

                    _concurrentOps.Dispose();
                }
                _disposed = true;
            }
        }
        #endregion
    }
}
