using System;
using CoCSharp.Network.Cryptography.NaCl;
using CoCSharp.Network.Cryptography.NaCl.Internal.Blake2;

namespace CoCSharp.Network.Cryptography
{
    /// <summary>
    /// Implements method to encrypt or decrypt network traffic of the Clash of Clan protocol
    /// version 8.x.x. This was based of clugh's work(https://github.com/clugh/cocdp/wiki/Protocol and
    /// (https://github.com/clugh/coc-proxy-csharp). :]
    /// </summary>
    public class Crypto8 : CoCCrypto
    {
        #region Constants
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
        public static KeyPair StandardKeyPair
        {
            // Cloning just not to mess up with refs
            get { return new KeyPair((byte[])_standardPublicKey.Clone(), (byte[])_standardPrivateKey.Clone()); }
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
                    // 8.709.2 ->
                    0x78, 0xDF, 0x4D, 0xF6, 0xF0, 0xE6, 0xDB, 0x02, 0x61, 0xE8, 0x7D, 0xD3, 0x8E,
                    0x74, 0xBB, 0x29, 0x55, 0x7D, 0xFE, 0x5B, 0x9B, 0xCE, 0x91, 0xF9, 0x2E, 0x1C,
                    0x98, 0x35, 0x5D, 0x7E, 0x71, 0x39

                    // 8.551.18 ->
                    //0x13, 0x15, 0xD5, 0xBA, 0x8E, 0x41, 0xD4, 0x0D, 0x2A, 0xD1, 0x48, 0x0A, 0xA6,
                    //0x21, 0xAD, 0x86, 0x4C, 0xF2, 0x55, 0x75, 0x69, 0xD1, 0x84, 0xFA, 0x00, 0xFC,
                    //0x11, 0x47, 0x1D, 0xD3, 0x1F, 0x4B

                    // 8.551.4 ->
                    //0x34, 0x9C, 0xE7, 0x8B, 0x78, 0xA0, 0x6A, 0x4E, 0x94, 0x64, 0x54, 0x35,
                    //0xAC, 0xBA, 0x1D, 0xFF, 0xFC, 0x40, 0xCC, 0x22, 0x76, 0x55, 0x8E, 0xD2,
                    //0xD1, 0x18, 0xF1, 0x34, 0x3A, 0x19, 0x78, 0x76

                    // 8.322.16 ->
                    //0xBB, 0x9C, 0xA4, 0xC6, 0xB5, 0x2E, 0xCD, 0xB4, 0x02, 0x67, 0xC3, 0xBC,
                    //0xCA, 0x03, 0x67, 0x92, 0x01, 0xA4, 0x03, 0xEF, 0x62, 0x30, 0xB9, 0xE4,
                    //0x88, 0xDB, 0x94, 0x9B, 0x58, 0xBC, 0x74, 0x79

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
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="Crypto8"/> class with the
        /// specified <see cref="MessageDirection"/> and a generated <see cref="NaCl.KeyPair"/> using <see cref="GenerateKeyPair()"/>.
        /// </summary>
        /// <param name="direction">Direction of the data.</param>
        /// <exception cref="ArgumentException">Incorrect direction.</exception>
        public Crypto8(MessageDirection direction) : this(direction, GenerateKeyPair())
        {
            // Space
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Crypto8"/> class with the
        /// specified <see cref="MessageDirection"/> and <see cref="NaCl.KeyPair"/>.
        /// </summary>
        /// <param name="direction">Direction of the data.</param>
        /// <param name="keyPair">Public and private key pair to use for encryption.</param>
        /// <exception cref="ArgumentException"><paramref name="direction"/> is incorrect.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="keyPair"/> is null.</exception>"
        public Crypto8(MessageDirection direction, KeyPair keyPair)
        {
            if (direction != MessageDirection.Client && direction != MessageDirection.Server)
                throw new ArgumentException("Cannot initialize Crypto8 with direction '" + direction + "'.", nameof(direction));
            if (keyPair == null)
                throw new ArgumentNullException(nameof(KeyPair));

            _direction = direction;
            _keyPair = keyPair;
        }
        #endregion

        #region Fields & Properties
        /// <summary>
        /// Gets the version of the <see cref="CoCCrypto"/>.
        /// </summary>
        public override int Version => 8;

        /// <summary>
        /// Gets the current <see cref="NaCl.KeyPair"/> used by the <see cref="Crypto8"/> to encrypt or decrypt data.
        /// </summary>
        public KeyPair KeyPair => _keyPair;

        /// <summary>
        /// Gets the shared public key.
        /// </summary>
        /// <remarks>
        /// It can either be 'k', 'pk' or 'serverkey' depending on the state.
        /// </remarks>
        public byte[] SharedKey => _sharedKey;

        /// <summary>
        /// Gets the direction of the data.
        /// </summary>
        public MessageDirection Direction => _direction;

        // Precomputered shared key.
        private KeyPair _precompKey;

        private readonly KeyPair _keyPair;
        private readonly MessageDirection _direction;

        // Other end's public key can either be clientkey(pk), serverkey and k.
        private byte[] _sharedKey;
        // Generated by hashing (clientKey, serverKey) or (snonce, clientKey, serverKey).
        private byte[] _blake2bNonce;
        // Can either be snonce or rnonce according to _direction.
        private byte[] _encryptNonce;
        // Can either be snonce or rnonce according to _direction.
        private byte[] _decryptNonce;
        // Crypto state. To have an idea where we are in the authentication.
        internal CryptoState _cryptoState;
        #endregion

        internal enum CryptoState
        {
            None = 0,
            // First key.
            InitialKey = 1,
            // snonce given.
            BlakeNonce = 2,
            // k given by the server, after 20104.
            SecoundKey = 3
        }

        /// <summary>
        /// Encrypts the provided bytes(plain-text).
        /// </summary>
        /// <param name="data">Bytes to encrypt.</param>
        public override void Encrypt(ref byte[] data)
        {
            if (data == null)
                return;

            switch (_cryptoState)
            {
                case CryptoState.InitialKey:
                case CryptoState.BlakeNonce:
                    data = PublicKeyBox.Box(data, _sharedKey, _keyPair.PrivateKey, _blake2bNonce);
                    break;

                case CryptoState.SecoundKey:
                    IncrementNonce(_encryptNonce);

                    data = SecretBox.Box(data, _sharedKey, _encryptNonce);
                    break;

                default:
                    throw new InvalidOperationException("Cannot encrypt in current state.");
            }
        }

        /// <summary>
        /// Decrypts the provided bytes(cipher-text).
        /// </summary>
        /// <param name="data">Bytes to decrypt.</param>
        public override void Decrypt(ref byte[] data)
        {
            if (data == null)
                return;

            switch (_cryptoState)
            {
                case CryptoState.InitialKey:
                case CryptoState.BlakeNonce:
                    data = PublicKeyBox.Open(data, _sharedKey, _keyPair.PrivateKey, _blake2bNonce);
                    break;

                case CryptoState.SecoundKey:
                    IncrementNonce(_decryptNonce);

                    data = SecretBox.Open(data, _sharedKey, _decryptNonce);
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
                throw new ArgumentNullException(nameof(publicKey));
            if (publicKey.Length != PublicKeyBox.PublicKeyLength)
                throw new ArgumentOutOfRangeException(nameof(publicKey), "Key must be 32 bytes in length.");

            if (_cryptoState == CryptoState.SecoundKey)
                throw new InvalidOperationException("Cannot update 'k' because it has already been set.");
            else if (_cryptoState == CryptoState.None)
            {
                // Order of keys is important. 
                // We're the server.
                if (Direction == MessageDirection.Client)
                    _blake2bNonce = GenerateBlake2BNonce(publicKey, _keyPair.PublicKey);
                // We're the client.
                else
                    _blake2bNonce = GenerateBlake2BNonce(_keyPair.PublicKey, publicKey);

                // We got initial key and Blake nonce.
                _cryptoState = CryptoState.InitialKey;
            }
            else
            {
                // Make sure we have a decrypt nonce before decrypting with k.
                if (_decryptNonce == null)
                    throw new InvalidOperationException("Cannot update shared key 'k' because did not provide a decrypt nonce.");

                // Make sure we have an encrypt nonce before encrypting with k
                if (_encryptNonce == null)
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
            if (_cryptoState == CryptoState.SecoundKey)
                throw new InvalidOperationException("Cannot update nonce after updated with shared key 'k'.");
            if (nonce == null)
                throw new ArgumentNullException(nameof(nonce));
            if (nonce.Length != 24)
                throw new ArgumentOutOfRangeException(nameof(nonce), "nonce must be 24 bytes in length.");

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
        /// Generates a <see cref="NaCl.KeyPair"/>.
        /// </summary>
        /// <returns>Generated <see cref="NaCl.KeyPair"/>.</returns>
        public static KeyPair GenerateKeyPair()
        {
            var keyPair = PublicKeyBox.GenerateKeyPair();
            return new KeyPair(keyPair.PublicKey, keyPair.PrivateKey);
        }

        /// <summary>
        /// Generates a <see cref="NaCl.KeyPair"/> from the specified private key.
        /// </summary>
        /// <param name="privateKey">Private key from which to generate the <see cref="NaCl.KeyPair"/>.</param>
        /// <returns>Generated <see cref="NaCl.KeyPair"/>.</returns>
        public static KeyPair GenerateKeyPair(byte[] privateKey)
        {
            var keyPair = PublicKeyBox.GenerateKeyPair(privateKey);
            return new KeyPair(keyPair.PublicKey, keyPair.PrivateKey);
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
            Buffer.BlockCopy(serverKey, 0, hashBuffer, PublicKeyBox.PublicKeyLength, serverKey.Length);

            using (var blake = new Blake2B(24))
                return blake.ComputeHash(hashBuffer);
        }

        // Generate blake2b nonce with snonce, clientkey and serverkey.
        private static byte[] GenerateBlake2BNonce(byte[] snonce, byte[] clientKey, byte[] serverKey)
        {
            var hashBuffer = new byte[clientKey.Length + serverKey.Length + snonce.Length];

            Buffer.BlockCopy(snonce, 0, hashBuffer, 0, PublicKeyBox.NonceLength);
            Buffer.BlockCopy(clientKey, 0, hashBuffer, PublicKeyBox.NonceLength, clientKey.Length);
            Buffer.BlockCopy(serverKey, 0, hashBuffer, PublicKeyBox.NonceLength + PublicKeyBox.PublicKeyLength, serverKey.Length);

            using (var blake = new Blake2B(PublicKeyBox.NonceLength))
                return blake.ComputeHash(hashBuffer);
        }

        // Increment nonce by 2.
        private static void IncrementNonce(byte[] nonce)
        {
            ushort c = 2;
            for (int i = 0; i < nonce.Length; i++)
            {
                c += nonce[i];
                nonce[i] = (byte)c;

                // 8 bits right shift to get the carry.
                c >>= 8;
            }
        }
    }
}
