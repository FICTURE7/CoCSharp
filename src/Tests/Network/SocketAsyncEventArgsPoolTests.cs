using CoCSharp.Network;
using NUnit.Framework;
using System;
using System.Net.Sockets;

namespace CoCSharp.Test.Network
{
    [TestFixture]
    public class SocketAsyncEventArgsPoolTests
    {
        [Test]
        public void Constructors_InvalidArgs_Exception()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new SocketAsyncEventArgsPool(-1));
        }

        [Test]
        public void PopPush_Disposed_Exception()
        {
            var pool = new SocketAsyncEventArgsPool(64);

            // Populate the pool.
            for (int i = 0; i < 64; i++)
                pool.Push(new SocketAsyncEventArgs());

            // Remove a couple of the args.
            for (int i = 0; i < 32; i++)
                pool.Pop();

            pool.Dispose();

            Assert.Throws<ObjectDisposedException>(() => pool.Push(new SocketAsyncEventArgs()));
            Assert.Throws<ObjectDisposedException>(() => pool.Pop());
        }

        [Test]
        public void Pop_PoolEmpty_RetusnNull()
        {
            var pool = new SocketAsyncEventArgsPool(1);
            var args = pool.Pop();
            Assert.Null(args);
        }

        [Test]
        public void Push__ExceedCapacity__Pushed_And_CapacityResized()
        {
            var pool = new SocketAsyncEventArgsPool(1);
            var args = new SocketAsyncEventArgs();

            pool.Push(args);
            Assert.AreEqual(1, pool.Capacity);

            pool.Push(args);
            Assert.AreEqual(2, pool.Capacity);
        }
    }
}
