using System;
using System.Collections.Generic;
using System.Net.Sockets;

namespace CoCSharp.Networking
{
    /// <summary>
    /// Implements methods to manage <see cref="SocketAsyncEventArgs"/> objects
    /// in pools.
    /// </summary>
    public class SocketAsyncEventArgsPool
    {
        private Object _ObjLock = new Object();

        /// <summary>
        /// Initializes a new instance of the <see cref="SocketAsyncEventArgsPool"/> class with
        /// the specified capacity.
        /// </summary>
        /// <param name="capacity">Max capacity of the pool.</param>
        public SocketAsyncEventArgsPool(int capacity)
        {
            Capacity = capacity;
            Pool = new Stack<SocketAsyncEventArgs>(capacity);
            for (int i = 0; i < capacity; i++)
            {
                var args = new SocketAsyncEventArgs();
                var packetBuffer = new PacketBuffer(args);
                Push(args);
            }
        }

        /// <summary>
        /// Gets the capacity of the pool.
        /// </summary>
        public int Capacity { get; private set; }
        /// <summary>
        /// Gets the current number of <see cref="SocketAsyncEventArgs"/> objects.
        /// </summary>
        public int Count { get { return Pool.Count; } }

        private Stack<SocketAsyncEventArgs> Pool { get; set; }

        /// <summary>
        /// Clears the pool.
        /// </summary>
        public void Clear()
        {
            Pool.Clear();
        }

        /// <summary>
        /// Push the specified <see cref="SocketAsyncEventArgs"/> object to the pool.
        /// </summary>
        /// <param name="args">The <see cref="SocketAsyncEventArgs"/> object.</param>
        public void Push(SocketAsyncEventArgs args)
        {
            lock (_ObjLock)
            {
                Pool.Push(args);
            }
        }

        /// <summary>
        /// Pops the first <see cref="SocketAsyncEventArgs"/> object from the pool.
        /// </summary>
        /// <returns></returns>
        public SocketAsyncEventArgs Pop()
        {
            lock (_ObjLock)
            {
                return Pool.Pop();
            }
        }
    }
}
