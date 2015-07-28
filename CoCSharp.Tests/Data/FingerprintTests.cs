using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CoCSharp.Data;
using System.IO;

namespace CoCSharp.Tests.Data
{
    [TestClass]
    public class FingerprintTests
    {
        [TestMethod]
        public void TestFingerprint()
        {
            var fingerprintJson = File.ReadAllText("fingerprint.json");
            var fingerprint = new Fingerprint(fingerprintJson);

            Console.WriteLine("From disk: ");
            Console.WriteLine(fingerprintJson);
            Console.WriteLine();
            Console.WriteLine("From CoCSharp.Data.Fingerprint: ");
            Console.WriteLine(fingerprint.ToJson());
        }
    }
}
