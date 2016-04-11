using CoCSharp.Data;
using NUnit.Framework;
using System;
using System.IO;

namespace CoCSharp.Test.Data
{
    [TestFixture]
    public class FingerprintTests
    {
        private string _fingerprintPath;

        [SetUp]
        public void SetUp()
        {
            _fingerprintPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Content/fingerprint.json");
        }

        [Test]
        public void TestFingerprintConstructors()
        {
            Assert.Throws<ArgumentNullException>(() => new Fingerprint(null));
            Assert.Throws<ArgumentException>(() => new Fingerprint(string.Empty));
            Assert.Throws<FileNotFoundException>(() => new Fingerprint("missingfile.json"));

            var fingerprint = new Fingerprint(_fingerprintPath);
            Assert.AreEqual(681, fingerprint.Files.Length, "Did not fully deserialize the fingerprint file.");
            Assert.AreEqual("9d075b9546211da641d06f4c576aa9b9c62212fb", fingerprint.MasterHash);
            Assert.AreEqual("8.116.5", fingerprint.Version);
        }

        [Test]
        public void TestFingerprintFromJson()
        {
            Assert.Throws<ArgumentNullException>(() => Fingerprint.FromJson(null));
            Assert.Throws<ArgumentException>(() => Fingerprint.FromJson(string.Empty));

            var json = File.ReadAllText(_fingerprintPath);
            var fingerprint = Fingerprint.FromJson(json);

            Assert.AreEqual(681, fingerprint.Files.Length, "Did not fully read the fingerprint file.");
            Assert.AreEqual("9d075b9546211da641d06f4c576aa9b9c62212fb", fingerprint.MasterHash);
            Assert.AreEqual("8.116.5", fingerprint.Version);
        }
    }
}
