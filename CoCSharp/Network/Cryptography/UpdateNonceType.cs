namespace CoCSharp.Network.Cryptography
{
    /// <summary>
    /// Defines ways to instruct the <see cref="Crypto8"/> to update nonces.
    /// </summary>
    public enum UpdateNonceType
    {
        /// <summary>
        /// Instructs the <see cref="Crypto8"/> to update the Blake2b nonce.
        /// </summary>
        Blake,

        /// <summary>
        /// Instructs the <see cref="Crypto8"/> to update the decrypt nonce. This can
        /// either be rnonce or snonce.
        /// </summary>
        Decrypt,

        /// <summary>
        /// Instructs the <see cref="Crypto8"/> to update the encrypt nonce. This can
        /// either be rnonce or snonce.
        /// </summary>
        Encrypt
    };
}
