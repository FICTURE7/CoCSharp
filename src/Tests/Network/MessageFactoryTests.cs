using CoCSharp.Network;
using CoCSharp.Network.Messages;
using NUnit.Framework;

namespace CoCSharp.Test.Network
{
    [TestFixture]
    public class MessageFactoryTests
    {
        [Test]
        public void Constructor_Initialize()
        {
            MessageFactory.Initialize();
        }

        [Test]
        public void TestMessageFactoryCreate()
        {
            // Check if it returns the proper Type.
            var keepAliveRequestMessage = MessageFactory.Create(10108);
            Assert.AreEqual(keepAliveRequestMessage.GetType(), typeof(KeepAliveRequestMessage));
            Assert.AreNotEqual(keepAliveRequestMessage.GetType(), typeof(KeepAliveResponseMessage));

            // Check if it returns UnknownMessage for unknown IDs.
            var unknownMessage = MessageFactory.Create(1337);
            Assert.AreEqual(unknownMessage.GetType(), typeof(UnknownMessage));

            // Check if it returns new instances.
            var instance1 = MessageFactory.Create(0);
            var instance2 = MessageFactory.Create(0);
            Assert.AreNotSame(instance1, instance2);
        }

        [Test]
        public void TestMessageFactoryTryCreate()
        {
            // Check if it returns true if it succeeded.
            var keepAliveRequestMessage = (Message)null;
            Assert.True(MessageFactory.TryCreate(10108, out keepAliveRequestMessage));

            // Check if it returns false if it failed.
            var unknownMessage = (Message)null;
            Assert.False(MessageFactory.TryCreate(1337, out unknownMessage));

            // Check if it returns the proper Type.
            Assert.AreEqual(keepAliveRequestMessage.GetType(), typeof(KeepAliveRequestMessage));
            Assert.AreNotEqual(keepAliveRequestMessage.GetType(), typeof(KeepAliveResponseMessage));

            var instance1 = (Message)null;
            var instance2 = (Message)null;

            // It will return false but we are checking if it returns new instances.
            MessageFactory.TryCreate(0, out instance1); 
            MessageFactory.TryCreate(0, out instance2);

            Assert.AreNotSame(instance1, instance2);
        }
    }
}
