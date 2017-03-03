using CoCSharp.Network.Cryptography.NaCl.Internal;
using System;
using System.Security.Cryptography;

namespace CoCSharp.Network.Cryptography.NaCl
{
    /// <summary>
    /// Provides methods to manipulate authen
    /// </summary>
    public static class PublicKeyBox
    {
        #region Constants
        private static readonly RNGCryptoServiceProvider s_rng = new RNGCryptoServiceProvider();

        /// <summary>
        /// Represents the length of a public key. This field is constant.
        /// </summary>
        public const int PublicKeyLength = 32;
        /// <summary>
        /// Represents the length of a secret key. This field is constant.
        /// </summary>
        public const int SecretKeyLength = 32;
        /// <summary>
        /// Represents the length of a nonce. This field is constant.
        /// </summary>
        public const int NonceLength = 24;
        #endregion

        #region Methods
        /// <summary>
        /// Generates a <see cref="KeyPair"/> from the specified private key(sk).
        /// </summary>
        /// <param name="privateKey">Private key(sk) from which to generate the <see cref="KeyPair"/>.</param>
        /// <returns><see cref="KeyPair"/> generated from the specified private key(sk).</returns>
        public static KeyPair GenerateKeyPair(byte[] privateKey)
        {
            CheckKey(privateKey, nameof(privateKey));

            var publicKey = new byte[PublicKeyLength];

            curve25519xsalsa20poly1305.crypto_box_getpublickey(privateKey, publicKey);
            return new KeyPair(publicKey, privateKey);
        }

        /// <summary>
        /// Generates a <see cref="KeyPair"/> from a random private key(sk).
        /// </summary>
        /// <returns><see cref="KeyPair"/> generated from a random private key(sk).</returns>
        public static KeyPair GenerateKeyPair()
        {
            var publicKey = new byte[PublicKeyLength];
            var privateKey = new byte[SecretKeyLength];

            curve25519xsalsa20poly1305.crypto_box_keypair(publicKey, privateKey);
            return new KeyPair(publicKey, privateKey);
        }

        /// <summary>
        /// Generates a random 24-bytes long nonce.
        /// </summary>
        /// <returns>24-bytes long random nonce.</returns>
        public static byte[] GenerateNonce()
        {
            var nonce = new byte[NonceLength];
            s_rng.GetBytes(nonce);
            return nonce;
        }

        public static byte[] Box(byte[] message, byte[] publicKey, byte[] secretKey, byte[] nonce)
        {
            var paddedChiper = new byte[message.Length + curve25519xsalsa20poly1305.crypto_secretbox_ZEROBYTES];
            var paddedMessage = new byte[message.Length + curve25519xsalsa20poly1305.crypto_secretbox_ZEROBYTES];
            Buffer.BlockCopy(message, 0, paddedMessage, curve25519xsalsa20poly1305.crypto_secretbox_ZEROBYTES, message.Length);

            if (curve25519xsalsa20poly1305.crypto_box(paddedChiper, paddedMessage, nonce, publicKey, secretKey) != 0)
                throw new CryptographicException("Failed to box PublicKeyBox.");

            var chiperLen = paddedChiper.Length - curve25519xsalsa20poly1305.crypto_secretbox_BOXZEROBYTES;
            var chiper = new byte[chiperLen];
            Buffer.BlockCopy(paddedChiper, curve25519xsalsa20poly1305.crypto_secretbox_BOXZEROBYTES, chiper, 0, chiperLen);
            return chiper;
        }

        public static byte[] Open(byte[] chiper, byte[] publicKey, byte[] secretKey, byte[] nonce)
        {
            var paddedChiper = new byte[chiper.Length + curve25519xsalsa20poly1305.crypto_secretbox_BOXZEROBYTES];
            var paddedMessage = new byte[chiper.Length + curve25519xsalsa20poly1305.crypto_secretbox_BOXZEROBYTES];
            Buffer.BlockCopy(chiper, 0, paddedChiper, curve25519xsalsa20poly1305.crypto_secretbox_BOXZEROBYTES, chiper.Length);

            if (curve25519xsalsa20poly1305.crypto_box_open(paddedMessage, paddedChiper, nonce, publicKey, secretKey) != 0)
                throw new CryptographicException("Failed to open PublicKeyBox.");

            var messageLen = paddedMessage.Length - curve25519xsalsa20poly1305.crypto_secretbox_ZEROBYTES;
            var message = new byte[messageLen];
            Buffer.BlockCopy(paddedMessage, curve25519xsalsa20poly1305.crypto_secretbox_ZEROBYTES, message, 0, messageLen);
            return message;
        }

        private static void CheckKey(byte[] key, string name)
        {
            if (key == null)
                throw new ArgumentNullException(name);
            if (key.Length != 32)
                throw new ArgumentOutOfRangeException(name, "Key length must be 32 bytes long.");
        }
        #endregion
    }
}
