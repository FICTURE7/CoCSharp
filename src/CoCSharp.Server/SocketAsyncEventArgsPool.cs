using System;
using System.Collections.Generic;
using System.Net.Sockets;

namespace CoCSharp.Server
{
    internal sealed class SocketAsyncEventArgsPool : IDisposable
    {
        public SocketAsyncEventArgsPool(int capacity, EventHandler<SocketAsyncEventArgs> handler)
        {
            if (capacity < 1)
                throw new ArgumentOutOfRangeException("capacity cannot be less that 1.");

            Capacity = capacity;
            _handler = handler;
            _sync = new object();
            _pool = new Stack<SocketAsyncEventArgs>(capacity);
            for (int i = 0; i < capacity; i++)
            {
                var args = CreateNew();
                Push(args);
            }
        }

        private bool _disposed;
        private readonly object _sync;
        private readonly EventHandler<SocketAsyncEventArgs> _handler;
        private readonly Stack<SocketAsyncEventArgs> _pool;

        public int Capacity { get; private set; }
        public int Count { get { return _pool.Count; } }

        public SocketAsyncEventArgs CreateNew()
        {
            var args = new SocketAsyncEventArgs();
            args.Completed += _handler;
            return args;
        }

        public SocketAsyncEventArgs Pop()
        {
            lock (_sync)
            {
                if (_pool.Count == 0)
                    return null;

                return _pool.Pop();
            }
        }

        public void Push(SocketAsyncEventArgs args)
        {
            lock (_sync)
            {
                _pool.Push(args);
            }
        }

        public void Dispose()
        {
            lock (_sync)
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
}
