using System.Net.Sockets;

namespace CoCSharp.Networking
{
    internal class MessageBufferManager
    {
        public MessageBufferManager(int receiveCount, int sendCount, int bufferSize)
        {
            _receiveCount = receiveCount;
            _sendCount = sendCount;
            _bufferIndex = 0;
            _bufferSize = bufferSize;
            _buffer = new byte[(receiveCount + sendCount) * bufferSize];
        }

        private readonly int _receiveCount;
        private readonly int _sendCount;
        private int _bufferIndex;
        private readonly int _bufferSize;
        private readonly byte[] _buffer;

        public void SetBuffer(SocketAsyncEventArgs args)
        {
            args.SetBuffer(_buffer, _bufferIndex, _bufferSize);
            _bufferIndex += _bufferSize;
        }
    }
}
