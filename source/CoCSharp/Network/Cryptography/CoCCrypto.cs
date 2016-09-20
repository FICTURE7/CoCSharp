namespace CoCSharp.Network.Cryptography
{
    /// <summary>
    /// Base class of Clash of Clans encryption.
    /// </summary>
    public abstract class CoCCrypto
    {
        /// <summary>
        /// Gets the version of the <see cref="CoCCrypto"/>.
        /// </summary>
        public abstract int Version { get; }

        /// <summary>
        /// Encrypts the provided bytes(plain-text).
        /// </summary>
        /// <param name="data">Bytes to encrypt.</param>
        public abstract void Encrypt(ref byte[] data);

        /// <summary>
        /// Decrypts the provided bytes(cipher-text).
        /// </summary>
        /// <param name="data">Bytes to decrypt.</param>
        public abstract void Decrypt(ref byte[] data);
    }
}


