using CoCSharp.Network;
using NUnit.Framework;
using System;
using System.IO;

namespace CoCSharp.Test.Network
{
    [TestFixture]
    public class MessageReaderTests
    {
        [Test]
        public void Constructors_InvalidArgs()
        {
            Assert.Throws<ArgumentNullException>(() => new MessageReader(null));
        }

		[Test]
        public void Reading_Disposed_Exception()
        {
            var memstream = new MemoryStream();
            var reader = new MessageReader(memstream);

            // Check that the BaseStream is opened.
            Assert.That(memstream.CanRead == true);

            reader.Dispose();

            // Check that the BaseStream is closed.
            Assert.That(memstream.CanRead == false);

            Assert.Throws<ObjectDisposedException>(() => reader.ReadBoolean());

            Assert.Throws<ObjectDisposedException>(() => reader.ReadInt16());
            Assert.Throws<ObjectDisposedException>(() => reader.ReadUInt16());

            Assert.Throws<ObjectDisposedException>(() => reader.ReadInt32());
            Assert.Throws<ObjectDisposedException>(() => reader.ReadUInt32());

            Assert.Throws<ObjectDisposedException>(() => reader.ReadSingle());

            Assert.Throws<ObjectDisposedException>(() => reader.ReadInt64());
            Assert.Throws<ObjectDisposedException>(() => reader.ReadUInt64());


            Assert.Throws<ObjectDisposedException>(() => reader.ReadDecimal());
            Assert.Throws<ObjectDisposedException>(() => reader.ReadDouble());
        }
    }
}
