using CoCSharp.Data;
using NUnit.Framework;
using System;
using System.IO;

namespace CoCSharp.Test.Data
{
    [TestFixture]
    public class FingerprintTests
    {
        public FingerprintTests()
        {
            _fingerprintPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Content/fingerprint.json");
        }

        private string _fingerprintPath;

        [Test]
        public void Constructors_InvalidArgs_Exception()
        {
            Assert.Throws<ArgumentNullException>(() => new Fingerprint(null));
            Assert.Throws<ArgumentException>(() => new Fingerprint(string.Empty));
            Assert.Throws<FileNotFoundException>(() => new Fingerprint("missingfile.json"));
        }

        [Test]
        public void Constructors_Valid()
        {
            var fingerprint = new Fingerprint(_fingerprintPath);
            Assert.AreEqual(681, fingerprint.Files.Length, "Did not fully deserialize the fingerprint file.");
            Assert.AreEqual("9d075b9546211da641d06f4c576aa9b9c62212fb", Utils.BytesToString(fingerprint.MasterHash));
            Assert.AreEqual("8.116.5", fingerprint.Version);
        }

        [Test]
        public void FromJson_InvalidArgs_Exception()
        {
            Assert.Throws<ArgumentNullException>(() => Fingerprint.FromJson(null));
            Assert.Throws<ArgumentException>(() => Fingerprint.FromJson(string.Empty));
            Assert.Throws<ArgumentException>(() => Fingerprint.FromJson("   "));
        }

        [Test]
        public void FromJson_ValidArgs()
        {
            var json = File.ReadAllText(_fingerprintPath);
            var fingerprint = Fingerprint.FromJson(json);

            Assert.AreEqual(681, fingerprint.Files.Length, "Did not fully read the fingerprint file.");
            Assert.AreEqual("9d075b9546211da641d06f4c576aa9b9c62212fb", Utils.BytesToString(fingerprint.MasterHash));
            Assert.AreEqual("8.116.5", fingerprint.Version);
        }
    }
}
