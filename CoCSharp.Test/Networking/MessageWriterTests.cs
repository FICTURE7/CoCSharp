using CoCSharp.Networking;
using NUnit.Framework;
using System;
using System.IO;

namespace CoCSharp.Test.Networking
{
    [TestFixture]
    public class MessageWriterTests
    {
        [Test]
        public void TestMessageWriterConstructors()
        {
            Assert.Throws<ArgumentNullException>(() => new MessageWriter(null));
        }

        [Test]
        public void TestMessageWriterDisposed()
        {
            var memstream = new MemoryStream();
            var writer = new MessageWriter(memstream);

            // Check that the BaseStream is opened.
            Assert.That(memstream.CanRead == true);

            writer.Dispose();

            // Check that the BaseStream is closed.
            Assert.That(memstream.CanRead == false);

            Assert.Throws<ObjectDisposedException>(() => writer.Write(false));

            Assert.Throws<ObjectDisposedException>(() => writer.Write((ushort)1));
            Assert.Throws<ObjectDisposedException>(() => writer.Write((short)1));

            Assert.Throws<ObjectDisposedException>(() => writer.Write((uint)1));
            Assert.Throws<ObjectDisposedException>(() => writer.Write((int)1));

            Assert.Throws<ObjectDisposedException>(() => writer.Write((float)1));

            Assert.Throws<ObjectDisposedException>(() => writer.Write((ulong)1));
            Assert.Throws<ObjectDisposedException>(() => writer.Write((long)1));

            Assert.Throws<ObjectDisposedException>(() => writer.Write((double)1));
            Assert.Throws<ObjectDisposedException>(() => writer.Write((decimal)1));
        }
    }
}
