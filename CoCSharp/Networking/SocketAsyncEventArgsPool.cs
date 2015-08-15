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
                var packetBuffer = new PacketBuffer(new byte[65535], i);
                var args = new SocketAsyncEventArgs();

                args.UserToken = packetBuffer;
                args.SetBuffer(packetBuffer.Buffer, 0, packetBuffer.Buffer.Length);
                Push(args);
            }
        }

        /// <summary>
        /// Max capacity of the pool.
        /// </summary>
        public int Capacity { get; private set; }
        /// <summary>
        /// Current number of <see cref="SocketAsyncEventArgs"/> objects.
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
            lock (Pool)
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
            lock (Pool)
            {
                return Pool.Pop();
            }
        }
    }
}
