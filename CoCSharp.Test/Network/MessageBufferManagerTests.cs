using CoCSharp.Network;
using NUnit.Framework;
using System;
using System.Net.Sockets;

namespace CoCSharp.Test.Network
{
    [TestFixture]
    public class MessageBufferManagerTests
    {
        [Test]
        public void Constructors_InvalidArgs()
        {
            var receiveCountEx = Assert.Throws<ArgumentOutOfRangeException>(() => new MessageBufferManager(0, 1, 1));
            Assert.That(receiveCountEx.ParamName == "receiveCount");

            var sendCountEx = Assert.Throws<ArgumentOutOfRangeException>(() => new MessageBufferManager(1, 0, 1));
            Assert.That(sendCountEx.ParamName == "sendCount");

            var bufferSizeEx = Assert.Throws<ArgumentOutOfRangeException>(() => new MessageBufferManager(1, 1, 0));
            Assert.That(bufferSizeEx.ParamName == "bufferSize");
        }

        [Test]
        public void SetBuffer_ExceedingBufferSize_Exception()
        {
            var bufferManager = new MessageBufferManager(64, 64, 65535);
            for (int i = 0; i < 128; i++)
            {
                var args = new SocketAsyncEventArgs();
                bufferManager.SetBuffer(args);
            }

            // We completely used the buffer therefore buffer index == buffer length.
            Assert.AreEqual(bufferManager.BufferIndex, 128 * 65535);

            Assert.Throws<InvalidOperationException>(() => bufferManager.SetBuffer(new SocketAsyncEventArgs()));
        }
    }
}
