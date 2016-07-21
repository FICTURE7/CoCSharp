using System;
using System.Net.Sockets;

namespace CoCSharp.Network
{
    /// <summary>
    /// Provides settings for the <see cref="NetworkManagerAsync"/> class. It is recommended to use
    /// this for better management of <see cref="SocketAsyncEventArgs"/> objects. This class cannot
    /// be inherited.
    /// </summary>
    public sealed class NetworkManagerAsyncSettings : IDisposable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NetworkManagerAsync"/> class
        /// with default settings.
        /// </summary>
        public NetworkManagerAsyncSettings() 
            : this(25, 25, 65535)
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
            : this(receiveCount, sendCount, 65535)
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
                throw new ArgumentOutOfRangeException("bufferSize", "Argument bufferSize cannot be less than 1.");

            BufferSize = bufferSize;
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
        }

        /// <summary>
        /// Gets a new instance of the default state of the <see cref="NetworkManagerAsyncSettings"/> class.
        /// </summary>
        public static NetworkManagerAsyncSettings DefaultSettings
        {
            get { return new NetworkManagerAsyncSettings(); }
        }

        /// <summary>
        /// Gets the number of receive operation <see cref="SocketAsyncEventArgs"/> objects
        /// being used.
        /// </summary>
        public int ReceiveCount { get { return _receivePool.Capacity; } }

        /// <summary>
        /// Gets the number of send operation <see cref="SocketAsyncEventArgs"/> objects
        /// being used.
        /// </summary>
        public int SendCount { get { return _receivePool.Capacity; } }

        /// <summary>
        /// Gets the buffer size of each <see cref="SocketAsyncEventArgs"/> object.
        /// </summary>
        public int BufferSize { get; private set; }

        private bool _disposed;
        private readonly MessageBufferManager _bufferManager;

        internal readonly SocketAsyncEventArgsPool _receivePool;
        internal readonly SocketAsyncEventArgsPool _sendPool;

        /// <summary>
        /// Releases all resources used by the current instance of the <see cref="NetworkManagerAsyncSettings"/> class.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }

        private void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _receivePool.Dispose();
                    _sendPool.Dispose();
                    //TODO: Dipose all NetworkManagerAsync instances using this also
                }
                _disposed = true;
            }
        }
    }
}
