using System.Collections.Generic;
using System.Net.Sockets;

namespace CoCSharp.Networking
{
    internal class PacketBufferManager
    {
        public PacketBufferManager(int receiveOpCount, int sendOpCount, int bufferSize)
        {
            m_BufferSize = bufferSize;
            m_BufferBlock = new byte[bufferSize * (receiveOpCount + sendOpCount)];
        }

        private byte[] m_BufferBlock = null;
        private int m_BufferSize = 0;
        private int m_CurrentIndex = 0;
        private Stack<int> m_FreeIndex = new Stack<int>();

        public void SetBuffer(SocketAsyncEventArgs args)
        {
            if (m_FreeIndex.Count > 0)
            {
                args.SetBuffer(m_BufferBlock, m_FreeIndex.Pop(), m_BufferSize);
            }
            else
            {
                args.SetBuffer(m_BufferBlock, m_CurrentIndex, m_BufferSize);
                m_CurrentIndex += m_BufferSize;
            }
        }

        public void RemoveBuffer(SocketAsyncEventArgs args)
        {
            m_FreeIndex.Push(args.Offset);
            args.SetBuffer(null, 0, 0);
        }
    }
}
