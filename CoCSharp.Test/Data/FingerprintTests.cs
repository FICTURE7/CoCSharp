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
        public void Constructors_ValidArgs()
        {
            var fingerprint = new Fingerprint(_fingerprintPath);
            Assert.AreEqual(681, fingerprint.Count, "Did not fully deserialize the fingerprint file.");
            Assert.AreEqual("9d075b9546211da641d06f4c576aa9b9c62212fb", InternalUtils.BytesToString(fingerprint.MasterHash));
            Assert.AreEqual("8.116.5", fingerprint.Version);
        }

        [Test]
        public void FromJson_InvalidArgs_Exception()
        {
            Assert.Throws<ArgumentNullException>(() => Fingerprint.FromJson(null));
            Assert.Throws<ArgumentNullException>(() => Fingerprint.FromJson(string.Empty));
            Assert.Throws<ArgumentNullException>(() => Fingerprint.FromJson("   "));
        }

        [Test]
        public void FromJson_ValidArgs()
        {
            var json = File.ReadAllText(_fingerprintPath);
            var fingerprint = Fingerprint.FromJson(json);

            Assert.AreEqual(681, fingerprint.Count, "Did not fully read the fingerprint file.");
            Assert.AreEqual("9d075b9546211da641d06f4c576aa9b9c62212fb", InternalUtils.BytesToString(fingerprint.MasterHash));
            Assert.AreEqual("8.116.5", fingerprint.Version);
        }

        [Test]
        public void ToJson_Equality()
        {
            // Original fingerprint.
            var oriFingerprint = new Fingerprint(_fingerprintPath);

            // Turn the Original fingerprint into a JSON string.
            var oriJson = oriFingerprint.ToJson();

            // Create a new Fingerprint object from the oriFingerprint.ToJson() string.
            var newFingerprint = Fingerprint.FromJson(oriJson);

            Assert.AreEqual(oriFingerprint.ComputeMasterHash(), newFingerprint.ComputeMasterHash());
        }

        [Test]
        public void ForEach_Loaded_Iterates()
        {
            var fingerprint = new Fingerprint(_fingerprintPath);
            var index = 0;
            foreach (var file in fingerprint)
            {
                Assert.AreSame(fingerprint[index], file);
                index++;
            }

            Assert.AreEqual(index, fingerprint.Count);
        }

        [Test]
        public void ComputeMasterHash_Equality()
        {
            var fingerprint = new Fingerprint(_fingerprintPath);
            var masterHash = fingerprint.ComputeMasterHash();
            Assert.AreEqual(fingerprint.MasterHash, masterHash);
        }
    }
}
