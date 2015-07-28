using System.Collections.Generic;
using System.Net.Sockets;

namespace CoCSharp.Networking
{
    public class SocketAsyncEventArgsPool
    {
        public SocketAsyncEventArgsPool(int capacity)
        {
            this.Capacity = capacity;
            this.Pool = new Stack<SocketAsyncEventArgs>(capacity);

            for (int i = 0; i < capacity; i++)
            {
                var packetBuffer = new PacketBuffer(new byte[65535]);
                var args = new SocketAsyncEventArgs();

                args.UserToken = packetBuffer;
                args.SetBuffer(packetBuffer.Buffer, 0, packetBuffer.Buffer.Length);
                Push(args);
            }
        }

        public int Capacity { get; private set; }
        public int Count { get { return Pool.Count; } }

        private Stack<SocketAsyncEventArgs> Pool { get; set; }

        public void Clear()
        {
            Pool.Clear();
        }

        public void Push(SocketAsyncEventArgs args)
        {
            Pool.Push(args);
        }

        public SocketAsyncEventArgs Pop()
        {
            return Pool.Pop();
        }
    }
}
