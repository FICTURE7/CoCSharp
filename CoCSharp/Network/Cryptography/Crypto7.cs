using System;

namespace CoCSharp.Network.Cryptography
{
    /// <summary>
    /// Implements method to encrypt or decrypt network traffic of the Clash of Clan protocol
    /// version 7.x.x. This was ported from Clash of Clans Documentation Project(https://github.com/clanner/cocdp/blob/master/cocutils.py)
    /// by clanner to C#. :]
    /// </summary>
    public class Crypto7 : CoCCrypto
    {
        private const string InitialKey = "fhsd6f86f67rt8fw78fw789we78r9789wer6re";
        private const string InitialNonce = "nonce";

        /// <summary>
        /// Initializes a new instance of the <see cref="Crypto7"/> class with
        /// the default key and nonce.
        /// </summary>
        public Crypto7()
        {
            InitializeCiphers(InitialKey + InitialNonce);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Crypto7"/> class with
        /// the specified key.
        /// </summary>
        /// <param name="key"></param>
        /// <exception cref="ArgumentNullException"><paramref name="key"/> is null.</exception>
        public Crypto7(string key)
        {
            if (key == null)
                throw new ArgumentNullException("key");

            InitializeCiphers(key);
        }

        private RC4 Encryptor { get; set; }
        private RC4 Decryptor { get; set; }

        /// <summary>
        /// Encrypts the provided bytes(plaintext).
        /// </summary>
        /// <param name="data">Bytes to encrypt.</param>
        /// <exception cref="ArgumentNullException"><paramref name="data"/> is null.</exception>
        public override void Encrypt(ref byte[] data)
        {
            if (data == null)
                throw new ArgumentNullException("data");

            for (int k = 0; k < data.Length; k++)
                data[k] ^= Encryptor.PRGA();
        }

        /// <summary>
        /// Decrypts the provided bytes(ciphertext).
        /// </summary>
        /// <param name="data">Bytes to decrypt.</param>
        /// <exception cref="ArgumentNullException"><paramref name="data"/> is null.</exception>
        public override void Decrypt(ref byte[] data)
        {
            if (data == null)
                throw new ArgumentNullException("data");

            for (int k = 0; k < data.Length; k++)
                data[k] ^= Decryptor.PRGA();
        }

        /// <summary>
        /// Update the key with the specified client seed and server nonce.
        /// </summary>
        /// <param name="clientSeed">Client seed.</param>
        /// <param name="serverNonce">Server random nonce.</param>
        /// <exception cref="ArgumentNullException"><paramref name="serverNonce"/> is null.</exception>
        public void UpdateCiphers(int clientSeed, byte[] serverNonce)
        {
            if (serverNonce == null)
                throw new ArgumentNullException("serverNonce");

            var newNonce = ScrambleNonce((ulong)clientSeed, serverNonce);
            var key = InitialKey + newNonce;
            InitializeCiphers(key);
        }

        /// <summary>
        /// Generates a random byte array of random length between 15 and 25 bytes long.
        /// </summary>
        /// <returns>The random byte array.</returns>
        public static byte[] GenerateNonce()
        {
            var buffer = new byte[InternalUtils.Random.Next(15, 25)];
            InternalUtils.Random.NextBytes(buffer);
            return buffer;
        }

        /// <summary>
        /// Initializes the ciphers with the specified key.
        /// </summary>
        /// <param name="key">The key used to update the cipher.</param>
        private void InitializeCiphers(string key)
        {
            Encryptor = new RC4(key);
            Decryptor = new RC4(key);

            for (int k = 0; k < key.Length; k++)
            {
                // skip bytes
                Encryptor.PRGA();
                Decryptor.PRGA();
            }
        }

        private static string ScrambleNonce(ulong clientSeed, byte[] serverNonce)
        {
            var scrambler = new Scrambler(clientSeed);
            var byte100 = 0;
            for (int i = 0; i < 100; i++)
                byte100 = scrambler.GetByte();
            var scrambled = string.Empty;
            for (int i = 0; i < serverNonce.Length; i++)
                scrambled += (char)(serverNonce[i] ^ (scrambler.GetByte() & byte100));
            return scrambled;
        }

        private class RC4
        {
            public RC4(byte[] key)
            {
                Key = KSA(key);
            }

            public RC4(string key)
            {
                Key = KSA(StringToByteArray(key));
            }

            public byte[] Key { get; set; } // "S"

            private byte i { get; set; }
            private byte j { get; set; }

            public byte PRGA()
            {
                /* Pseudo-Random Generation Algorithm
                 * 
                 * The returned value should be XORed with
                 * the data to encrypt or decrypt it.
                 */

                var temp = (byte)0;

                i = (byte)((i + 1) % 256);
                j = (byte)((j + Key[i]) % 256);

                // swap S[i] and S[j];           
                temp = Key[i];
                Key[i] = Key[j];
                Key[j] = temp;

                return Key[(Key[i] + Key[j]) % 256]; // value to XOR with data
            }

            private static byte[] KSA(byte[] key)
            {
                /* Key-Scheduling Algorithm
                 * 
                 * Used to initialize key array.
                 */

                var keyLength = key.Length;
                var S = new byte[256];

                for (int i = 0; i != 256; i++) S[i] = (byte)i;

                var j = (byte)0;
                var temp = (byte)0;

                for (int i = 0; i != 256; i++)
                {
                    j = (byte)((j + S[i] + key[i % keyLength]) % 256); // meth is working

                    // swap S[i] and S[j];
                    temp = S[i];
                    S[i] = S[j];
                    S[j] = temp;
                }
                return S;
            }

            private static byte[] StringToByteArray(string str)
            {
                var bytes = new byte[str.Length];
                for (int i = 0; i < str.Length; i++) bytes[i] = (byte)str[i];
                return bytes;
            }
        }

        private class Scrambler
        {
            public Scrambler(ulong seed)
            {
                IX = 0;
                Buffer = SeedBuffer(seed);
            }

            public int IX { get; set; }
            public ulong[] Buffer { get; set; }

            private static ulong[] SeedBuffer(ulong seed)
            {
                var buffer = new ulong[624];
                for (int i = 0; i < 624; i++)
                {
                    buffer[i] = seed;
                    seed = (1812433253 * ((seed ^ RShift(seed, 30)) + 1)) & 0xFFFFFFFF;
                }
                return buffer;
            }

            public int GetByte()
            {
                var x = (ulong)GetInt();
                if (IsNeg(x)) x = Negate(x);
                return (int)(x % 256);
            }

            private int GetInt()
            {
                if (IX == 0) MixBuffer();
                var val = Buffer[IX];

                IX = (IX + 1) % 624;
                val ^= RShift(val, 11) ^ LShift((val ^ RShift(val, 11)), 7) & 0x9D2C5680;
                return (int)(RShift((val ^ LShift(val, 15L) & 0xEFC60000), 18L) ^ val ^ LShift(val, 15L) & 0xEFC60000);
            }

            private void MixBuffer()
            {
                var i = 0;
                var j = 0;
                while (i < 624)
                {
                    i += 1;
                    var v4 = (Buffer[i % 624] & 0x7FFFFFFF) + (Buffer[j] & 0x80000000);
                    var v6 = RShift(v4, 1) ^ Buffer[(i + 396) % 624];
                    if ((v4 & 1) != 0) v6 ^= 0x9908B0DF;
                    Buffer[j] = v6;
                    j += 1;
                }
            }

            private static ulong RShift(ulong num, ulong n)
            {
                var highbits = (ulong)0;
                if ((num & Pow(2, 31)) != 0) highbits = (Pow(2, n) - 1) * Pow(2, 32 - n);
                return (num / Pow(2, n)) | highbits;
            }

            private static ulong LShift(ulong num, ulong n)
            {
                return (num * Pow(2, n)) % Pow(2, 32);
            }

            private static bool IsNeg(ulong num)
            {
                return (num & (ulong)Math.Pow(2, 31)) != 0;
            }

            private static ulong Negate(ulong num)
            {
                return (~num) + 1;
            }

            private static ulong Pow(ulong x, ulong y)
            {
                return (ulong)Math.Pow(x, y);
            }
        }
    }
}
