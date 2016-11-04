using CoCSharp.Data;
using NUnit.Framework;
using System;

namespace CoCSharp.Test.Data
{
    [TestFixture]
    public class AssetDownloaderTests
    {
        // Tests the constructors for invalid arguments.
        [Test]
        public void Constructors_InvalidArgs_Exception()
        {
            Assert.Throws<ArgumentNullException>(() => new AssetDownloader(null));
            Assert.Throws<ArgumentOutOfRangeException>(() => new AssetDownloader(""));
            Assert.Throws<ArgumentException>(() => new AssetDownloader("invalidhex-string"));

            Assert.Throws<ArgumentNullException>(() => new AssetDownloader("3a4d26e5d18f907db2a82a0f808e0cced9f25dfd", null));
        }
    }
}
