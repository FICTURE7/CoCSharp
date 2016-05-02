using System;
using System.Net.Sockets;

namespace CoCSharp.Network
{
    internal class MessageBufferManager
    {
        public MessageBufferManager(int receiveCount, int sendCount, int bufferSize)
        {
            if (receiveCount < 1)
                throw new ArgumentOutOfRangeException("receiveCount", "bufferSize cannot be less than 1.");
            if (sendCount < 1)
                throw new ArgumentOutOfRangeException("sendCount", "bufferSize cannot be less than 1.");
            if (bufferSize < 1)
                throw new ArgumentOutOfRangeException("bufferSize", "bufferSize cannot be less than 1.");

            _receiveCount = receiveCount;
            _sendCount = sendCount;

            BufferIndex = 0;
            _bufferSize = bufferSize;
            _buffer = new byte[(receiveCount + sendCount) * bufferSize];
        }

        private readonly int _receiveCount;
        private readonly int _sendCount;

        internal int BufferIndex;
        private readonly int _bufferSize;
        private readonly byte[] _buffer;

        public void SetBuffer(SocketAsyncEventArgs args)
        {
            if (BufferIndex >= _buffer.Length)
            {
                var message = "Cannot SetBuffer of args because offset '" + BufferIndex + "' is larger than buffer size '" + _buffer.Length + "'.";
                throw new InvalidOperationException(message);
            }

            args.SetBuffer(_buffer, BufferIndex, _bufferSize);
            BufferIndex += _bufferSize;
        }
    }
}
