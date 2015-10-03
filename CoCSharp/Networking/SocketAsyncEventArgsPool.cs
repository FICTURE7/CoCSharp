using System.Collections.Generic;
using System.Net.Sockets;

namespace CoCSharp.Networking
{
    internal class SocketAsyncEventArgsPool
    {
        private object m_ObjLock = new object();

        public SocketAsyncEventArgsPool(int capacity)
        {
            Capacity = capacity;
            Pool = new Stack<SocketAsyncEventArgs>(capacity);
        }

        public int Capacity { get; private set; }
        public int Count { get { return Pool.Count; } }

        private Stack<SocketAsyncEventArgs> Pool { get; set; }

        public void Push(SocketAsyncEventArgs args)
        {
            lock (m_ObjLock)
            {
                Pool.Push(args);
            }
        }

        public SocketAsyncEventArgs Pop()
        {
            lock (m_ObjLock)
            {
                return Pool.Pop();
            }
        }
    }
}
