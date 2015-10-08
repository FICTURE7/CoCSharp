using CoCSharp.Networking;
using NUnit.Framework;
using System;

namespace CoCSharp.Test.Networking
{
    [TestFixture]
    public class CoCCryptoTests
    {
        private Randomizer m_Randomizer = new Randomizer();
        private CoCCrypto m_Crypto = new CoCCrypto();

        [Test]
        public void TestDefaultCrypto()
        {
            Console.WriteLine();
            Console.WriteLine("DefaultCrypto Dump");
            Console.WriteLine("------------------");
            var bytes = new byte[512];
            m_Crypto.Encrypt(bytes);

            TestUtils.Print(bytes);
        }

        [Test]
        public void TestRandomCrypto()
        {
            Console.WriteLine();
            var bytes = new byte[512];
            var serverRandom = new byte[14];
            var randomizerSeed = Randomizer.RandomSeed;
            m_Randomizer = new Randomizer(randomizerSeed);
            var seed = m_Randomizer.Next();

            m_Randomizer.NextBytes(serverRandom);
            m_Crypto.UpdateCiphers((ulong)seed, serverRandom);
            m_Crypto.Encrypt(bytes);

            Console.WriteLine("Randomizer Seed: {0}", randomizerSeed);
            Console.WriteLine("Seed: {0}", seed);
            Console.WriteLine("ServerRandom Dump");
            Console.WriteLine("-----------------");
            TestUtils.Print(serverRandom);
            Console.WriteLine();

            Console.WriteLine("RandomCrypto Dump");
            Console.WriteLine("-----------------");
            TestUtils.Print(bytes);
        }
    }
}
