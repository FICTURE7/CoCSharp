using CoCSharp.Network;
using CoCSharp.Network.Messages;
using NUnit.Framework;

namespace CoCSharp.Test.Network
{
    [TestFixture]
    public class MessageTests
    {
        [Test]
        public void GetMessageDirection_()
        {
            // NOTE: Test of Message.GetMessageDirection(Message).
            // LoginRequestMessage is sent by the client to the server.
            var directionServer = Message.GetMessageDirection(new KeepAliveRequestMessage());
            Assert.AreEqual(MessageDirection.Server, directionServer);

            // LoginSuccessMessage is sent by the client to the client.
            var directionClient = Message.GetMessageDirection(new KeepAliveResponseMessage());
            Assert.AreEqual(MessageDirection.Client, directionClient);


            


            // NOTE: Test of Message.GetMessageDirection<T>().
            // LoginRequestMessage is sent by the client to the server.
            var directionServer2 = Message.GetMessageDirection<LoginRequestMessage>();
            Assert.AreEqual(MessageDirection.Server, directionServer2);

            // LoginSuccessMessage is sent by the client to the client.
            var directionClient2 = Message.GetMessageDirection<LoginSuccessMessage>();
            Assert.AreEqual(MessageDirection.Client, directionClient2);
        }
    }
}
