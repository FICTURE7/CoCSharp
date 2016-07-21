using System;
using System.Collections.Generic;
using System.Net.Sockets;

namespace CoCSharp.Network
{
    internal sealed class SocketAsyncEventArgsPool : IDisposable
    {
        public SocketAsyncEventArgsPool(int capacity)
        {
            if (capacity < 1)
                throw new ArgumentOutOfRangeException("capacity", "capacity cannot be less that 1.");

            Capacity = capacity;
            _lock = new object();
            _pool = new Stack<SocketAsyncEventArgs>(capacity);
        }

        private object _lock;
        private bool _disposed;
        private Stack<SocketAsyncEventArgs> _pool;

        public int Capacity { get; private set; }

        public int Count { get { return _pool.Count; } }

        public SocketAsyncEventArgs Pop()
        {
            if (_disposed)
                throw new ObjectDisposedException(null, "Cannot Pop because the SocketAsyncEventArgsPool was disposed.");

            lock (_lock)
            {
                if (_pool.Count == 0)
                    return null;

                return _pool.Pop();
            }
        }

        public void Push(SocketAsyncEventArgs args)
        {
            if (_disposed)
                throw new ObjectDisposedException(null, "Cannot Push because the SocketAsyncEventArgsPool was disposed.");

            lock (_lock)
            {
                // Resize the capacity of the pool.
                if (Count >= Capacity)
                    Capacity++;

                _pool.Push(args);
            }
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                for (int i = 0; i < _pool.Count; i++)
                {
                    var args = _pool.Pop();
                    args.Dispose();
                }
                _disposed = true;
            }
        }
    }
}
