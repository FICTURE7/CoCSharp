using Sodium;
using System;

namespace CoCSharp.Networking.Cryptography
{
    /// <summary>
    /// Implements method to encrypt or decrypt network traffic of the Clash of Clan protocol
    /// version 8.x.x.
    /// </summary>
    public class Crypto8
    {
        private static readonly byte[] _standardPrivateKey = new byte[]
        {
            0x18, 0x91, 0xD4, 0x01, 0xFA, 0xDB, 0x51, 0xD2, 0x5D, 0x3A, 0x91, 0x74,
            0xD4, 0x72, 0xA9, 0xF6, 0x91, 0xA4, 0x5B, 0x97, 0x42, 0x85, 0xD4, 0x77,
            0x29, 0xC4, 0x5C, 0x65, 0x38, 0x07, 0x0D, 0x85
        };

        private static readonly byte[] _standardPublicKey = new byte[] // == PublicKeyBox.GenerateKeyPair(_standardPrivateKey);
        {
            0x72, 0xF1, 0xA4, 0xA4, 0xC4, 0x8E, 0x44, 0xDA, 0x0C, 0x42, 0x31, 0x0F,
            0x80, 0x0E, 0x96, 0x62, 0x4E, 0x6D, 0xC6, 0xA6, 0x41, 0xA9, 0xD4, 0x1C,
            0x3B, 0x50, 0x39, 0xD8, 0xDF, 0xAD, 0xC2, 0x7E
        };

        /// <summary>
        /// Gets a new instance of the standard keypair used by custom servers and clients.
        /// </summary>
        public static CoCKeyPair StandardKeyPair
        {
            get { return new CoCKeyPair((byte[])_standardPublicKey.Clone(), (byte[])_standardPrivateKey.Clone()); }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Crypto8"/> class with
        /// the standard public and private <see cref="CoCKeyPair"/>.
        /// </summary>
        public Crypto8() : this((byte[])_standardPrivateKey.Clone())
        {
            // Cloning because PublicKeyBox.GenerateKeyPair messes _standardPrivateKey.
        }

        /// <summary>
        /// Initializes a new instance of the<see cref="Crypto8"/> class with
        /// the specified private key. The public key will be generated using <see cref="PublicKeyBox.GenerateKeyPair(byte[])"/>
        /// function of libsodium.
        /// </summary>
        /// <param name="privateKey">Private key. The public key will generated using it.</param>
        public Crypto8(byte[] privateKey)
        {
            if (privateKey == null)
                throw new ArgumentNullException("privateKey");

            var keyPair = PublicKeyBox.GenerateKeyPair(privateKey);
            _keyPair = new CoCKeyPair(keyPair.PublicKey, keyPair.PrivateKey);
        }

        /// <summary>
        /// Gets the current <see cref="CoCKeyPair"/> used by the <see cref="Crypto8"/>.
        /// </summary>
        public CoCKeyPair KeyPair
        {
            get { return _keyPair; }
        }

        private CoCKeyPair _keyPair; // current key pair
        private byte[] _publicKey; // other end public key
        private byte[] _nonce; 

        /// <summary>
        /// Encrypts the provided data.
        /// </summary>
        /// <param name="data">Data to encrypt.</param>
        public void Encrypt(ref byte[] data)
        {
            data = PublicKeyBox.Create(data, _nonce, _keyPair.PrivateKey, _publicKey);
        }

        /// <summary>
        /// Decrypts the provided data.
        /// </summary>
        /// <param name="data">Data to decrypt.</param>
        public void Decrypt(ref byte[] data)
        {
            data = PublicKeyBox.Open(data, _nonce, _keyPair.PrivateKey, _publicKey);
        }

        /// <summary>
        /// Updates the <see cref="Crypto8"/> with the other end's public key.
        /// </summary>
        /// <param name="publicKey">Other end's public key.</param>
        public void UpdateKey(byte[] publicKey)
        {
            if (publicKey == null)
                throw new ArgumentNullException("publicKey");
            if (publicKey.Length != CoCKeyPair.KeyLength)
                throw new ArgumentOutOfRangeException("Length of publicKey must be 32 bytes long.");

            _publicKey = publicKey;
            _nonce = GenerateNonce(_publicKey, _keyPair.PublicKey); // clientKey and serverKey
        }

        /// <summary>
        /// Generates a <see cref="CoCKeyPair"/>.
        /// </summary>
        /// <returns>Generated <see cref="CoCKeyPair"/>.</returns>
        public CoCKeyPair GenerateKeyPair()
        {
            var keyPair = PublicKeyBox.GenerateKeyPair();
            return new CoCKeyPair(keyPair.PublicKey, keyPair.PrivateKey);
        }

        /// <summary>
        /// Generates a 24 bytes long nonce.
        /// </summary>
        /// <returns>Generated 24 bytes long nonce.</returns>
        public byte[] GenerateNonce()
        {
            return PublicKeyBox.GenerateNonce();
        }

        private static byte[] GenerateNonce(byte[] clientKey, byte[] serverKey)
        {
            var hashBuffer = new byte[clientKey.Length + serverKey.Length];

            Buffer.BlockCopy(clientKey, 0, hashBuffer, 0, clientKey.Length);
            Buffer.BlockCopy(serverKey, 0, hashBuffer, CoCKeyPair.KeyLength, serverKey.Length);

            return Utils.Blake2B.ComputeHash(hashBuffer);
        }
    }
}
