using Sodium;
using System;

namespace CoCSharp.Network.Cryptography
{
    /// <summary>
    /// Represents a public and private key pair.
    /// </summary>
    /// <remarks>
    /// Wrapper around <see cref="KeyPair"/> of libsodium.
    /// </remarks>
    public class CoCKeyPair : IDisposable
    {
        /// <summary>
        /// Represents the length of a key in bytes. This field is constant.
        /// </summary>
        public const int KeyLength = 32;

        /// <summary>
        /// Represents the length of a nonce in bytes. This field is constant.
        /// </summary>
        public const int NonceLength = 24;

        private bool _disposed = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="CoCKeyPair"/> class with the
        /// specified public key and private key.
        /// </summary>
        /// <param name="publicKey">Public key.</param>
        /// <param name="privateKey">Private key.</param>
        /// <exception cref="ArgumentNullException"><paramref name="publicKey"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="privateKey"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="publicKey"/> length is not 32.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="privateKey"/> length is not 32.</exception>
        public CoCKeyPair(byte[] publicKey, byte[] privateKey)
        {
            if (publicKey == null)
                throw new ArgumentNullException("publicKey");
            if (publicKey.Length != PublicKeyBox.PublicKeyBytes)
                throw new ArgumentOutOfRangeException("publicKey", "publicKey must be 32 bytes in length.");

            if (privateKey == null)
                throw new ArgumentNullException("privateKey");
            if (privateKey.Length != PublicKeyBox.SecretKeyBytes)
                throw new ArgumentOutOfRangeException("privateKey", "publicKey must be 32 bytes in length.");

            _keyPair = new KeyPair(publicKey, privateKey);
        }

        private KeyPair _keyPair;

        /// <summary>
        /// Gets the public key.
        /// </summary>
        public byte[] PublicKey
        {
            get
            {
                if (_disposed)
                    throw new ObjectDisposedException(null, "Cannot access CoCKeyPair object because it was disposed.");

                return _keyPair.PublicKey;
            }
        }

        /// <summary>
        /// Gets the private key.
        /// </summary>
        public byte[] PrivateKey
        {
            get
            {
                if (_disposed)
                    throw new ObjectDisposedException(null, "Cannot access CoCKeyPair object because it was disposed.");

                return _keyPair.PrivateKey;
            }
        }

        /// <summary>
        /// Releases all the resources used by the <see cref="CoCKeyPair"/>.
        /// </summary>
        public void Dispose()
        {
            if (_disposed)
                return;

            _keyPair.Dispose();
            _disposed = true;
        }
    }
}
