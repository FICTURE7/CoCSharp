namespace CoCSharp.Network.Cryptography
{
    /// <summary>
    /// Base class of Clash of Clans encryption.
    /// </summary>
    public abstract class CoCCrypto
    {
        /// <summary>
        /// Encrypts the provided bytes(plaintext).
        /// </summary>
        /// <param name="data">Bytes to encrypt.</param>
        public abstract void Encrypt(ref byte[] data);

        /// <summary>
        /// Decrypts the provided bytes(ciphertext).
        /// </summary>
        /// <param name="data">Bytes to decrypt.</param>
        public abstract void Decrypt(ref byte[] data);
    }
}


