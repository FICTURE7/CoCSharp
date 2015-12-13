using System;
using System.Collections.Generic;
using System.Net.Sockets;

namespace CoCSharp.Networking
{
    /// <summary>
    /// Provides methods to manage <see cref="SocketAsyncEventArgs"/> in pools.
    /// </summary>
    public class SocketAsyncEventArgsPool : IDisposable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SocketAsyncEventArgsPool"/> with
        /// the specified capacity.
        /// </summary>
        /// <param name="capacity">The capacity of the pool.</param>
        /// <exception cref="ArgumentException"><paramref name="capacity"/> is less that 1.</exception>
        public SocketAsyncEventArgsPool(int capacity)
        {
            if (capacity < 1)
                throw new ArgumentException("capacity cannot be less that 1.");

            Capacity = capacity;
            _objLock = new object();
            _pool = new Stack<SocketAsyncEventArgs>(capacity);
        }

        private object _objLock;
        private bool _disposed;
        private Stack<SocketAsyncEventArgs> _pool;

        /// <summary>
        /// Gets the capacity of the <see cref="SocketAsyncEventArgsPool"/>.
        /// </summary>
        public int Capacity { get; private set; }

        /// <summary>
        /// Gets the number of <see cref="SocketAsyncEventArgs"/> in 
        /// the <see cref="SocketAsyncEventArgsPool"/>.
        /// </summary>
        public int Count { get { return _pool.Count; } }

        /// <summary>
        /// Removes and return the <see cref="SocketAsyncEventArgs"/> on top of
        /// the <see cref="SocketAsyncEventArgsPool"/>.
        /// </summary>
        /// <returns></returns>
        public SocketAsyncEventArgs Pop()
        {
            lock(_objLock)
            {
                return _pool.Pop();
            }
        }

        /// <summary>
        /// Inserts a <see cref="SocketAsyncEventArgs"/> on top of
        /// the <see cref="SocketAsyncEventArgsPool"/>.
        /// </summary>
        /// <param name="args"></param>
        public void Push(SocketAsyncEventArgs args)
        {
            lock(_objLock)
            {
                _pool.Push(args);
            }
        }

        /// <summary>
        /// Releases all resources used by the current instance of the <see cref="SocketAsyncEventArgsPool"/> class.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    for (int i = 0; i < _pool.Count; i++)
                    {
                        var args = _pool.Pop();
                        args.Dispose();
                    }
                }

                _disposed = true;
            }
        }
    }
}
