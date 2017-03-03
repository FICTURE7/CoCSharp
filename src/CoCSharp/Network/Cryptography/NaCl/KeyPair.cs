using CoCSharp.Network.Cryptography.NaCl.Internal;
using System;

namespace CoCSharp.Network.Cryptography.NaCl
{
    /// <summary>
    /// Represents an NaCl key pair.
    /// </summary>
    public class KeyPair
    {
        [Obsolete]
        public const int NonceLength = 24;
        [Obsolete]
        public const int KeyLength = 32;

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="KeyPair"/> class with the
        /// specified public key and private key.
        /// </summary>
        /// <param name="publicKey">Public key.</param>
        /// <param name="privateKey">Private key.</param>
        /// <exception cref="ArgumentNullException"><paramref name="publicKey"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="privateKey"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="publicKey"/> length is not 32.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="privateKey"/> length is not 32.</exception>
        public KeyPair(byte[] publicKey, byte[] privateKey)
        {
            if (publicKey == null)
                throw new ArgumentNullException(nameof(publicKey));
            if (publicKey.Length != PublicKeyBox.PublicKeyLength)
                throw new ArgumentOutOfRangeException(nameof(publicKey), "Key must be 32 bytes long.");

            if (privateKey == null)
                throw new ArgumentNullException(nameof(privateKey));
            if (privateKey.Length != PublicKeyBox.SecretKeyLength)
                throw new ArgumentOutOfRangeException(nameof(privateKey), "Key must be 32 bytes long.");

            _pk = publicKey;
            _sk = privateKey;
            _precomputeredk = new byte[PublicKeyBox.PublicKeyLength];
            curve25519xsalsa20poly1305.crypto_box_beforenm(_precomputeredk, _pk, _sk);
        }
        #endregion

        #region Fields & Properties
        private readonly byte[] _pk;
        private readonly byte[] _sk;
        private readonly byte[] _precomputeredk;

        /// <summary>
        /// Gets the public key.
        /// </summary>
        public byte[] PublicKey => _pk;

        /// <summary>
        /// Gets the private key.
        /// </summary>
        public byte[] PrivateKey => _sk;

        // Not used. :/

        /// <summary>
        /// Gets the pre-computed of the private key and the public key. (crypto_box_beforenm).
        /// </summary>
        public byte[] PrecomputerKey => _precomputeredk;
        #endregion
    }
}
