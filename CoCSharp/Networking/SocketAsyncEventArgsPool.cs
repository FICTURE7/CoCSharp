using System;
using System.Collections.Generic;
using System.Net.Sockets;

namespace CoCSharp.Networking
{
    internal sealed class SocketAsyncEventArgsPool : IDisposable
    {
        private bool m_Disposed = false;
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
            if (args == null)
                throw new ArgumentNullException("args");

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

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (m_Disposed)
                return;

            if (disposing)
            {
                for (int i = 0; i < Count; i++)
                {
                    var args = Pool.Pop();
                    args.Dispose();
                }
                Pool.Clear();
            }
            m_Disposed = true;
        }
    }
}
