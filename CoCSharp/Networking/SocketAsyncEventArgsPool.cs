using System.Collections.Generic;
using System.Net.Sockets;

namespace CoCSharp.Networking
{
    /// <summary>
    /// Implements methods to manage <see cref="SocketAsyncEventArgs"/> objects
    /// in pools.
    /// </summary>
    internal class SocketAsyncEventArgsPool
    {
        private object m_ObjLock = new object();

        /// <summary>
        /// Initializes a new instance of the <see cref="SocketAsyncEventArgsPool"/> class with
        /// the specified capacity.
        /// </summary>
        /// <param name="capacity">Max capacity of the pool.</param>
        public SocketAsyncEventArgsPool(int capacity)
        {
            Capacity = capacity;
            Pool = new Stack<SocketAsyncEventArgs>(capacity);
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
        /// Push the specified <see cref="SocketAsyncEventArgs"/> object to the pool.
        /// </summary>
        /// <param name="args">The <see cref="SocketAsyncEventArgs"/> object.</param>
        public void Push(SocketAsyncEventArgs args)
        {
            lock (m_ObjLock)
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
            lock (m_ObjLock)
            {
                return Pool.Pop();
            }
        }
    }
}
