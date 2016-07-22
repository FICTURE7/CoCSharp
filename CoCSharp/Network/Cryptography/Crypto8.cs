using System;
using Sodium;

namespace CoCSharp.Network.Cryptography
{
    /// <summary>
    /// Implements method to encrypt or decrypt network traffic of the Clash of Clan protocol
    /// version 8.x.x. This was based of of clugh's work(https://github.com/clugh/cocdp/wiki/Protocol) and
    /// (https://github.com/clugh/coc-proxy-csharp). :]
    /// </summary>
    public class Crypto8 : CoCCrypto
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
        /// Gets a new instance of the standard key-pair used by custom servers and clients.
        /// </summary>
        /// <remarks>
        /// More information here (https://github.com/FICTURE7/CoCSharp/issues/54#issuecomment-173556064).
        /// </remarks>
        public static CoCKeyPair StandardKeyPair
        {
            // Cloning just not to mess up with refs
            get { return new CoCKeyPair((byte[])_standardPublicKey.Clone(), (byte[])_standardPrivateKey.Clone()); }
        }

        /// <summary>
        /// Gets a new instance of Supercell server's public key.
        /// </summary>
        /// <remarks>
        /// This was extracted from the android version of libg.so
        /// </remarks>
        public static byte[] SupercellPublicKey
        {
            get
            {
                return new byte[]
                {
                    // 8.322.16 ->
                    0xBB, 0x9C, 0xA4, 0xC6, 0xB5, 0x2E, 0xCD, 0xB4, 0x02, 0x67, 0xC3, 0xBC,
                    0xCA, 0x03, 0x67, 0x92, 0x01, 0xA4, 0x03, 0xEF, 0x62, 0x30, 0xB9, 0xE4,
                    0x88, 0xDB, 0x94, 0x9B, 0x58, 0xBC, 0x74, 0x79

                    // 8.212.12 ->
                    //0x9B, 0x39, 0xB4, 0x40, 0xFF, 0x6C, 0x13, 0xAD, 0x07, 0xB5, 0x06, 0xFC,
                    //0x55, 0xE3, 0x7F, 0x69, 0x85, 0x68, 0x95, 0xC3, 0xFD, 0x5A, 0xB3, 0x59,
                    //0x78, 0xCD, 0xF5, 0xE3, 0x4E, 0xB3, 0x74, 0x71

                    // 8.212.9 ->
                    //0x46, 0x9b, 0x70, 0x4e, 0x7f, 0x60, 0x09, 0xba, 0x8f, 0xc7, 0x2e, 0x9b,
                    //0x5c, 0x86, 0x4c, 0x8e, 0x92, 0x85, 0xa7, 0x55, 0xc5, 0x19, 0x0f, 0x03,
                    //0xf5, 0xc7, 0x48, 0x52, 0xf6, 0xd9, 0xf4, 0x19

                    // 8.212.3 ->
                    //0x15, 0x0C, 0x52, 0xDB, 0x12, 0xBA, 0x1C, 0x9D, 0xD8, 0x09, 0xB8, 0x93,
                    //0x4A, 0x53, 0x5F, 0x42, 0x8A, 0x91, 0xB7, 0xB6, 0x1E, 0x15, 0xAB, 0x46,
                    //0x9E, 0x42, 0xB9, 0x61, 0x4C, 0x76, 0xA3, 0x25,

                    // 8.116.2 ->
                    //0x01, 0xC9, 0x8C, 0x14, 0x3A, 0x84, 0x0D, 0x92, 0xEE, 0x65, 0x69, 0x96,
                    //0xDA, 0xD5, 0xAF, 0x41, 0xDE, 0x5D, 0x1B, 0x8E, 0xBB, 0x28, 0x90, 0x81,
                    //0x36, 0x8B, 0x5C, 0xFD, 0xA9, 0xBD, 0x4A, 0x30
                };
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Crypto8"/> class with the
        /// specified <see cref="MessageDirection"/> and a generated <see cref="CoCKeyPair"/> using <see cref="GenerateKeyPair"/>.
        /// </summary>
        /// <param name="direction">Direction of the data.</param>
        /// <exception cref="ArgumentException">Incorrect direction.</exception>
        public Crypto8(MessageDirection direction) : this(direction, GenerateKeyPair())
        {
            // Space
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Crypto8"/> class with the
        /// specified <see cref="MessageDirection"/> and <see cref="CoCKeyPair"/>.
        /// </summary>
        /// <param name="direction">Direction of the data.</param>
        /// <param name="keyPair">Public and private key pair to use for encryption.</param>
        /// <exception cref="ArgumentException"><paramref name="direction"/> is incorrect.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="keyPair"/> is null.</exception>"
        public Crypto8(MessageDirection direction, CoCKeyPair keyPair)
        {
            if (direction != MessageDirection.Client && direction != MessageDirection.Server)
                throw new ArgumentException("Cannot initialize Crypto8 with direction '" + direction + "'.", "direction");
            if (keyPair == null)
                throw new ArgumentNullException("keyPair");

            _direction = direction;
            _keyPair = keyPair;
        }

        /// <summary>
        /// Gets the current <see cref="CoCKeyPair"/> used by the <see cref="Crypto8"/> to encrypt or decrypt data.
        /// </summary>
        public CoCKeyPair KeyPair
        {
            get { return _keyPair; }
        }

        /// <summary>
        /// Gets the shared public key.
        /// </summary>
        /// <remarks>
        /// It can either be 'k', 'pk' or 'serverkey' depending on the state.
        /// </remarks>
        public byte[] SharedKey
        {
            get { return _sharedKey; }
        }

        /// <summary>
        /// Gets the direction of the data.
        /// </summary>
        public MessageDirection Direction
        {
            get { return _direction; }
        }

        private readonly CoCKeyPair _keyPair;
        private readonly MessageDirection _direction;

        private byte[] _sharedKey; // other end public key can either be clientkey(pk), serverkey and k
        private byte[] _blake2bNonce; // generated with (clientKey, serverKey) or (snonce, clientKey, serverKey)
        private byte[] _encryptNonce; // can be snonce or rnonce according to _direction
        private byte[] _decryptNonce; // can be snonce or rnonce according to _direction
        private CryptoState _cryptoState;

        private enum CryptoState
        {
            None = 0,
            InitialKey = 1, // first key
            BlakeNonce = 2, // snonce given
            SecoundKey = 3 // k given by the server, after 20104
        }

        /// <summary>
        /// Encrypts the provided bytes(plaintext).
        /// </summary>
        /// <param name="data">Bytes to encrypt.</param>
        public override void Encrypt(ref byte[] data)
        {
            switch (_cryptoState)
            {
                case CryptoState.InitialKey:
                case CryptoState.BlakeNonce:
                    data = PublicKeyBox.Create(data, _blake2bNonce, _keyPair.PrivateKey, _sharedKey);
                    break;

                case CryptoState.SecoundKey:
                    IncrementNonce(_encryptNonce);
                    var padData = SecretBox.Create(data, _encryptNonce, _sharedKey);
                    data = new byte[padData.Length - 16];
                    Buffer.BlockCopy(padData, 16, data, 0, padData.Length - 16); // skip 16 bytes pad
                    break;

                default:
                    throw new InvalidOperationException("Cannot encrypt in current state.");
            }
        }

        /// <summary>
        /// Decrypts the provided bytes(ciphertext).
        /// </summary>
        /// <param name="data">Bytes to decrypt.</param>
        public override void Decrypt(ref byte[] data)
        {
            switch (_cryptoState)
            {
                case CryptoState.InitialKey:
                case CryptoState.BlakeNonce:
                    data = PublicKeyBox.Open(data, _blake2bNonce, _keyPair.PrivateKey, _sharedKey); // use blake nonce
                    break;

                case CryptoState.SecoundKey:
                    IncrementNonce(_decryptNonce);
                    var padData = new byte[data.Length + 16]; // append a 16 bytes long pad to it
                    Buffer.BlockCopy(data, 0, padData, 16, data.Length);
                    data = SecretBox.Open(padData, _decryptNonce, _sharedKey); // use decrypt nonce
                    break;

                default:
                    throw new InvalidOperationException("Cannot decrypt in current state.");
            }
        }

        /// <summary>
        /// Updates the <see cref="Crypto8"/> with the other end's public key according to the <see cref="MessageDirection"/>
        /// the <see cref="Crypto8"/> was initialized with.
        /// </summary>
        /// <remarks>
        /// The Blake2B nonce will be generated depending on the state of the <see cref="Crypto8"/>.
        /// </remarks>
        /// <param name="publicKey">Other end's public key.</param>
        /// <exception cref="ArgumentNullException"><paramref name="publicKey"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="publicKey"/> length is not 32.</exception>
        public void UpdateSharedKey(byte[] publicKey)
        {
            if (publicKey == null)
                throw new ArgumentNullException("publicKey");
            if (publicKey.Length != CoCKeyPair.KeyLength)
                throw new ArgumentOutOfRangeException("publicKey", "publicKey must be 32 bytes in length.");

            if (_cryptoState == CryptoState.SecoundKey)
                throw new InvalidOperationException();
            else if (_cryptoState == CryptoState.None)
            {
                if (Direction == MessageDirection.Client) // order of keys is important. we're the server
                    _blake2bNonce = GenerateBlake2BNonce(publicKey, _keyPair.PublicKey);
                else // we're the client
                    _blake2bNonce = GenerateBlake2BNonce(_keyPair.PublicKey, publicKey);

                _cryptoState = CryptoState.InitialKey; // we got initial key and blakenonce
            }
            else
            {
                if (_decryptNonce == null) // make sure we have a decrypt nonce before decrypting with k
                    throw new InvalidOperationException("Cannot update shared key 'k' because did not provide a decrypt nonce.");

                if (_encryptNonce == null) // make sure we have an encrypt nonce before encrypting with k
                    throw new InvalidOperationException("Cannot update shared key 'k' because did not provide an encrypt nonce.");

                _cryptoState = CryptoState.SecoundKey;
            }

            _sharedKey = publicKey;
        }

        /// <summary>
        /// Updates the specified <see cref="UpdateNonceType"/> with the specified nonce.
        /// </summary>
        /// <param name="nonce">Nonce to use for the update.</param>
        /// <param name="nonceType">Nonce type to update.</param>
        /// <exception cref="ArgumentNullException"><paramref name="nonce"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="nonce"/> length is not 24.</exception>
        public void UpdateNonce(byte[] nonce, UpdateNonceType nonceType)
        {
            if (_cryptoState == CryptoState.SecoundKey) // can only be updated twice
                throw new InvalidOperationException("Cannot update nonce after updated with shared key 'k'.");
            if (nonce == null)
                throw new ArgumentNullException("nonce");
            if (nonce.Length != CoCKeyPair.NonceLength)
                throw new ArgumentOutOfRangeException("nonce", "nonce must be 24 bytes in length.");

            switch (nonceType)
            {
                case UpdateNonceType.Blake:
                    if (_cryptoState == CryptoState.InitialKey)
                    {
                        if (Direction == MessageDirection.Client) // order of keys is important. we're the server
                            _blake2bNonce = GenerateBlake2BNonce(nonce, _sharedKey, _keyPair.PublicKey);
                        else // we're the client
                            _blake2bNonce = GenerateBlake2BNonce(nonce, _keyPair.PublicKey, _sharedKey);

                        _cryptoState = CryptoState.BlakeNonce; // use blake nonce
                    }
                    break;

                case UpdateNonceType.Decrypt:
                    _decryptNonce = nonce;
                    break;

                case UpdateNonceType.Encrypt:
                    _encryptNonce = nonce;
                    break;

                default:
                    throw new ArgumentException("Unexpected NonceType: " + nonceType, "nonceType");
            }
        }

        /// <summary>
        /// Generates a public and private <see cref="CoCKeyPair"/>.
        /// </summary>
        /// <remarks>
        /// This is a wrapper around <see cref="PublicKeyBox.GenerateKeyPair()"/>.
        /// </remarks>
        /// <returns>Generated <see cref="CoCKeyPair"/>.</returns>
        public static CoCKeyPair GenerateKeyPair()
        {
            var keyPair = PublicKeyBox.GenerateKeyPair();
            return new CoCKeyPair(keyPair.PublicKey, keyPair.PrivateKey);
        }

        /// <summary>
        /// Generates a 24 bytes long nonce.
        /// </summary>
        /// <remarks>
        /// This is a wrapper around <see cref="PublicKeyBox.GenerateNonce()"/>.
        /// </remarks>
        /// <returns>Generated 24 bytes long nonce.</returns>
        public static byte[] GenerateNonce()
        {
            return PublicKeyBox.GenerateNonce();
        }

        // Generate blake2b nonce with clientkey(pk) and serverkey.
        private static byte[] GenerateBlake2BNonce(byte[] clientKey, byte[] serverKey)
        {
            var hashBuffer = new byte[clientKey.Length + serverKey.Length];

            Buffer.BlockCopy(clientKey, 0, hashBuffer, 0, clientKey.Length);
            Buffer.BlockCopy(serverKey, 0, hashBuffer, CoCKeyPair.KeyLength, serverKey.Length);

            return GenericHash.Hash(hashBuffer, null, CoCKeyPair.NonceLength);
        }

        // Generate blake2b nonce with snonce, clientkey and serverkey.
        private static byte[] GenerateBlake2BNonce(byte[] snonce, byte[] clientKey, byte[] serverKey)
        {
            var hashBuffer = new byte[clientKey.Length + serverKey.Length + snonce.Length];

            Buffer.BlockCopy(snonce, 0, hashBuffer, 0, CoCKeyPair.NonceLength);
            Buffer.BlockCopy(clientKey, 0, hashBuffer, CoCKeyPair.NonceLength, clientKey.Length);
            Buffer.BlockCopy(serverKey, 0, hashBuffer, CoCKeyPair.NonceLength + CoCKeyPair.KeyLength, serverKey.Length);

            return GenericHash.Hash(hashBuffer, null, CoCKeyPair.NonceLength);
        }

        // Increment nonce by 2.
        private static void IncrementNonce(byte[] nonce)
        {
            // TODO: Write own method for incrementing nonces by 2.
            nonce = Utilities.Increment(Utilities.Increment(nonce));
        }
    }
}
