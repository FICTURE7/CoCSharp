using CoCSharp.Network.Cryptography;
using CoCSharp.Network.Cryptography.NaCl;
using CoCSharp.Network.Messages;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace CoCSharp.Network
{
    /// <summary>
    /// Represents a processor to process <see cref="Message"/> that was encrypted using <c>libsodium</c>.
    /// </summary>
    public class MessageProcessorNaCl : MessageProcessor
    {
        /// <summary>
        /// Defines the states of the processor.
        /// </summary>
        public enum States : byte
        {
            /// <summary>
            /// Handshaking state.
            /// </summary>
            Handshaking,

            /// <summary>
            /// Authenticating state.
            /// </summary>
            Authentifying,

            /// <summary>
            /// Authenticated state.
            /// </summary>
            Authentified,

            /// <summary>
            /// Completed state.
            /// </summary>
            Completed
        };

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="MessageProcessorNaCl"/> with the specified <see cref="KeyPair"/>
        /// </summary>
        /// <param name="keyPair"><see cref="KeyPair"/> to use.</param>
        /// <exception cref="ArgumentNullException"><paramref name="keyPair"/> is null.</exception>
        public MessageProcessorNaCl(KeyPair keyPair)
        {
            if (keyPair == null)
                throw new ArgumentNullException(nameof(keyPair));

            _keyPair = keyPair;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageProcessorNaCl"/> with the specified <see cref="KeyPair"/>
        /// and server public key.
        /// </summary>
        /// 
        /// <remarks>
        /// Use this constructor initialize as a client.
        /// </remarks>
        /// 
        /// <param name="keyPair"><see cref="KeyPair"/> to use.</param>
        /// <param name="serverKey">Public key of the server.</param>
        /// <exception cref="ArgumentNullException"><paramref name="keyPair"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="serverKey"/> is null.</exception>
        public MessageProcessorNaCl(KeyPair keyPair, byte[] serverKey)
        {
            if (keyPair == null)
                throw new ArgumentNullException(nameof(keyPair));
            if (serverKey == null || serverKey.Length == 0)
                throw new ArgumentNullException(nameof(serverKey));

            _keyPair = keyPair;
            _serverKey = serverKey;
        }
        #endregion

        #region Fields & Properties
        // Represents how the incoming messages will be processed.
        private int _incommingState;
        // Represents how the outgoing messages will be processed.
        private int _outgoingState;

        private States _nstate;
        private MessageDirection _direction;

        private readonly MessageReader _reader;
        private readonly MessageWriter _writer;

        // Crypto8 object that is going to decrypt incoming
        // and encrypt outgoing messages.
        private Crypto8 _crypto;
        // Key pair to use to initialize _crypto with, this key pair
        // can be the server private and public key, or the generated
        // client private and public key.
        private readonly KeyPair _keyPair;
        // Public-key of the server. This key will be used to update _crypto.UpdateSharedKey(_serverKey)
        // when the processor is processing as a client.
        // This is not used when _direction is going to the client.
        private readonly byte[] _serverKey;

        private byte[] _key;
        private byte[] _sessionKey;
        // Other end's nonce.
        private byte[] _remoteNonce;
        // Our generated nonce.
        private byte[] _localNonce;

        /// <summary>
        /// Gets the <see cref="State"/> state of the <see cref="MessageProcessorNaCl"/>.
        /// </summary>
        public States State => _nstate;

        /// <summary>
        /// Gets or sets the session key.
        /// </summary>
        public byte[] SessionKey => _sessionKey;

        /// <summary>
        /// Gets the <see cref="CoCCrypto"/> that is going to decrypt incoming and encrypt outgoing
        /// messages.
        /// </summary>
        public override CoCCrypto Crypto => _crypto;
        #endregion

        #region Methods
        /// <summary>
        /// Processes the specified chippered array of bytes and returns
        /// the resulting <see cref="Message"/>.
        /// </summary>
        /// <param name="header">Header of the message.</param>
        /// <param name="stream">Chippered array of bytes representing a message to process.</param>
        /// <param name="raw">Raw array of bytes representing the message.</param>
        /// <param name="plaintext">Plaintext representation of the data read.</param>
        /// <returns>Resulting <see cref="Message"/>.</returns>
        public override Message ProcessIncoming(MessageHeader header, BufferStream stream, ref byte[] raw, ref byte[] plaintext)
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));

            // Direction of where the message is *going to*.
            var messageDirection = Message.GetMessageDirection(header.Id);
            // Message instance that we will return to the caller.
            var message = MessageFactory.Create(header.Id);

            // Decrypt incoming data taking the message direction.
            // Header is only used to print debugging info.
            plaintext = ProcessIncomingData(messageDirection, header, stream);

            Debug.Assert(plaintext != null);
            using (var reader = new MessageReader(new MemoryStream(plaintext)))
            {
                try
                {
                    message.ReadMessage(reader);

                    // Set the session key if the message is HandshakeSucessMessage.
                    // This session key will then be used later on.
                    const int HANDSHAKE_SUCCESS_ID = 20100;
                    if (header.Id == HANDSHAKE_SUCCESS_ID)
                    {
                        var hsMessage = (HandshakeSuccessMessage)message;
                        var sessionKey = hsMessage.SessionKey;

                        Debug.WriteLine($"Updated session-key: {ToHexString(sessionKey)}");
                        _sessionKey = sessionKey;
                    }
                }
                catch
                {
                    Interlocked.Add(ref _incommingState, (int)_direction);
                    throw;
                }
            }

            Interlocked.Add(ref _incommingState, (int)_direction);
            return message;
        }

        /// <summary>
        /// Processes the specified <see cref="Message"/> and returns
        /// the resulting chippered array of bytes.
        /// </summary>
        /// <param name="message"><see cref="Message"/> to process.</param>
        /// <returns>Resulting chippered array of bytes.</returns>
        public override byte[] ProcessOutgoing(Message message)
        {
            if (message == null)
                throw new ArgumentNullException(nameof(message));

            var messageDirection = Message.GetMessageDirection(message);
            var bodyStream = new MemoryStream();
            using (var bodyWriter = new MessageWriter(bodyStream))
            {
                message.WriteMessage(bodyWriter);
                var body = bodyStream.ToArray();
                var chiper = ProcessOutgoingData(messageDirection, body);

                Interlocked.Add(ref _outgoingState, (int)_direction);
                return chiper;
            }
        }

        #region Incoming
        private byte[] ProcessIncomingData(MessageDirection direction, MessageHeader header, BufferStream stream)
        {
            var plaintext = (byte[])null;
            var chiper = new byte[header.Length];
            stream.Read(chiper, 0, header.Length);

            // Handshaking.
            if (_incommingState == 0)
            {
                // Handshakes are sent unencrypted.
                // First message by both ends is always sent unencrypted.
                plaintext = new byte[header.Length];
                Buffer.BlockCopy(chiper, 0, plaintext, 0, header.Length);

                // Use the first message direction to configure the processor.
                // If message is coming in, then the going out is opposite direction.
                _direction = GetOppositeDirection(direction);
                // If we're the client then we must have a serverkey.
                if (_direction == MessageDirection.Server && _serverKey == null)
                    throw new InvalidOperationException("Public key of server was not specified.");

                if (_crypto == null)
                    _crypto = new Crypto8(_direction, _keyPair);

                Debug.WriteLine($"Initialized Crypto8 {_direction} with private-key: {ToHexString(_keyPair.PrivateKey)}, public-key {_keyPair.PublicKey}");
            }

            // If somehow the incoming message to process comes from the same direction.
            // The MessageProcessor can only process message going in One Direction. ;]
            if (GetOppositeDirection(direction) != _direction)
                throw new InvalidOperationException("Tried to process an incoming data from a message coming from the same direction."); // -> Protocol Exception?

            // _incomingState == 2 means we are the server.
            // Usually processing 10101 - LoginRequestMessage.
            if (_incommingState == 2)
            {
                // -> Post-Encryption.
                // Copies the public key appended to the beginning of the message
                // sent of message 20100.
                var publicKey = new byte[KeyPair.KeyLength];
                Buffer.BlockCopy(chiper, 0, publicKey, 0, KeyPair.KeyLength);

                Debug.WriteLine($"Public-Key from {header.Id}: {ToHexString(publicKey)}");

                // Copies the remaining bytes into the postChiper buffer which
                // will be decrypted by _crypto.
                var tmpPlaintextLen = header.Length - KeyPair.KeyLength;
                var tmpPlaintext = new byte[tmpPlaintextLen];
                Buffer.BlockCopy(chiper, KeyPair.KeyLength, tmpPlaintext, 0, tmpPlaintextLen);

                // Crypto8 will take the client's publicKey & _keyPair.PublicKey and generate a blake2b nonce
                _crypto.UpdateSharedKey(publicKey);
                // Then use _keyPair.PrivateKey, publicKey and nonce to decrypt.
                _crypto.Decrypt(ref tmpPlaintext);

                // -> Pre-Encryption.
                var sessionKey = new byte[KeyPair.NonceLength];
                var remoteNonce = new byte[KeyPair.NonceLength];

                // Copy the SessionKey and the ClientNonce.
                Buffer.BlockCopy(tmpPlaintext, 0, sessionKey, 0, KeyPair.NonceLength);
                Buffer.BlockCopy(tmpPlaintext, KeyPair.NonceLength, remoteNonce, 0, KeyPair.NonceLength);

                Debug.WriteLine($"Session-key from {header.Id}: {ToHexString(sessionKey)}");
                Debug.WriteLine($"Client-nonce from {header.Id}: {ToHexString(remoteNonce)}");

                // Copies the plaintext without the session key and the remote/client nonce.
                var plaintextLen = tmpPlaintext.Length - (KeyPair.NonceLength * 2);
                plaintext = new byte[plaintextLen];
                Buffer.BlockCopy(tmpPlaintext, KeyPair.NonceLength * 2, plaintext, 0, plaintextLen);

                _crypto.UpdateNonce(remoteNonce, UpdateNonceType.Decrypt);
                _crypto.UpdateNonce(remoteNonce, UpdateNonceType.Blake);
                _sessionKey = sessionKey;
                _remoteNonce = remoteNonce;

                _nstate = States.Authentifying;
            }
            // _incomingState == 3 means we are the client.
            // Usually processing 20104 - LoginSuccessMessage.
            else if (_incommingState == 3)
            {
                // -> Might want to generate random nonce here for the
                // client nonce.
                Debug.Assert(_localNonce != null);

                var tmpPlaintext = (byte[])chiper.Clone();

                // _crypto will use this nonce after the second key is passed to it
                // using _crypto.UpdateSharedKey.
                var localNonce = _localNonce;
                _crypto.UpdateNonce(localNonce, UpdateNonceType.Encrypt);
                _crypto.UpdateNonce(localNonce, UpdateNonceType.Blake);
                _crypto.Decrypt(ref tmpPlaintext);

                // -> Post-Encryption
                var remoteNonce = new byte[KeyPair.NonceLength];
                var key = new byte[KeyPair.KeyLength];

                // Copies the ServerNonce and the new second secret key.
                Buffer.BlockCopy(tmpPlaintext, 0, remoteNonce, 0, KeyPair.NonceLength);
                Buffer.BlockCopy(tmpPlaintext, KeyPair.NonceLength, key, 0, KeyPair.KeyLength);

                Debug.WriteLine($"Shared-key from {header.Id}: {ToHexString(key)}");
                Debug.WriteLine($"Server-nonce from {header.Id}: {ToHexString(remoteNonce)}");

                // Copies the plaintext without the server nonce and the new second secret key.
                var plaintextLen = tmpPlaintext.Length - KeyPair.NonceLength - KeyPair.KeyLength;
                plaintext = new byte[plaintextLen];
                Buffer.BlockCopy(tmpPlaintext, KeyPair.NonceLength + KeyPair.KeyLength, plaintext, 0, plaintextLen);

                _crypto.UpdateNonce(remoteNonce, UpdateNonceType.Decrypt);
                _crypto.UpdateSharedKey(key);

                _remoteNonce = remoteNonce;
                _key = key;

                _nstate = States.Authentified;
            }
            // Messages after the previous states are processed the same way.
            else if (_incommingState > (int)_direction)
            {
                _nstate = States.Completed;

                plaintext = (byte[])chiper.Clone();
                _crypto.Decrypt(ref plaintext);
            }

            return plaintext;
        }
        #endregion

        #region Outgoing
        private byte[] ProcessOutgoingData(MessageDirection direction, byte[] plaintext)
        {
            var chiper = (byte[])null;

            // Handshaking
            if (_outgoingState == 0)
            {
                chiper = (byte[])plaintext.Clone();

                _direction = direction;
                if (_crypto == null)
                    _crypto = new Crypto8(direction, _keyPair);
            }

            if (direction != _direction)
                throw new InvalidOperationException("Tried to process an outgoing message coming from a different direction."); // -> Protocol Exception?

            // _outgoingState == 2 means we're the server.
            // Usually processing 20104 - LoginSuccessMessage.
            if (_outgoingState == 2)
            {
                Debug.Assert(_remoteNonce != null);
                Debug.Assert(_localNonce == null);

                var key = Crypto8.GenerateKeyPair();
                var localNonce = Crypto8.GenerateNonce();
                var remoteNonce = _remoteNonce;

                var tmpChiper = new byte[plaintext.Length + KeyPair.NonceLength + KeyPair.KeyLength];
                Buffer.BlockCopy(localNonce, 0, tmpChiper, 0, KeyPair.NonceLength);
                Buffer.BlockCopy(key.PublicKey, 0, tmpChiper, KeyPair.NonceLength, KeyPair.KeyLength);
                Buffer.BlockCopy(plaintext, 0, tmpChiper, KeyPair.NonceLength + KeyPair.KeyLength, plaintext.Length);

                _crypto.Encrypt(ref tmpChiper);

                Debug.WriteLine($"Server-nonce: {ToHexString(localNonce)}");
                Debug.WriteLine($"Shared-key: {ToHexString(key.PublicKey)}");

                _crypto.UpdateNonce(localNonce, UpdateNonceType.Encrypt);
                _crypto.UpdateSharedKey(key.PublicKey);

                _localNonce = localNonce;
                chiper = tmpChiper;
                //_key = key;
            }
            // _outgoingState == 3 means we're the client.
            // Usually processing 10101 - LoginRequestMessage.
            else if (_outgoingState == 3)
            {
                var serverKey = _serverKey;
                var sessionKey = _sessionKey;
                Debug.Assert(serverKey != null, "Server key was null.");
                Debug.Assert(sessionKey != null, "Session key was null.");

                // Generate a ClientNonce.
                var localNonce = Crypto8.GenerateNonce();

                Debug.WriteLine($"Generated ClientNonce: {ToHexString(localNonce)}");

                // Craft the new packet which is
                // tmpChiper = sessionKey + localNonce + plaintext.
                var tmpChiper = new byte[plaintext.Length + KeyPair.NonceLength * 2];
                Buffer.BlockCopy(sessionKey, 0, tmpChiper, 0, KeyPair.NonceLength);
                Buffer.BlockCopy(localNonce, 0, tmpChiper, KeyPair.NonceLength, KeyPair.NonceLength);
                Buffer.BlockCopy(plaintext, 0, tmpChiper, KeyPair.NonceLength * 2, plaintext.Length);

                // Use our specified from the constructor _keyPair.PublicKey and specified _serverKey from the constructor
                // to generate the blake2b nonce and encrypt using _keyPair.PrivateKey and the generated blake2b nonce.
                _crypto.UpdateSharedKey(serverKey);
                _crypto.Encrypt(ref tmpChiper);

                // Craft another new packet which is
                // chiper = _keyPair.PublicKey + tmpChiper.
                chiper = new byte[tmpChiper.Length + KeyPair.KeyLength];
                Buffer.BlockCopy(_crypto.KeyPair.PublicKey, 0, chiper, 0, KeyPair.KeyLength);
                Buffer.BlockCopy(tmpChiper, 0, chiper, KeyPair.KeyLength, tmpChiper.Length);

                _localNonce = localNonce;
            }
            // Messages after the previous states are processed the same way.
            else if (_outgoingState > (int)_direction)
            {
                var tmpChiper = (byte[])plaintext.Clone();

                _crypto.Encrypt(ref tmpChiper);
                chiper = tmpChiper;
            }

            return chiper;
        }
        #endregion

        // Returns the opposite direction of specified direction.
        private static MessageDirection GetOppositeDirection(MessageDirection dir)
        {
            if (dir == MessageDirection.Client)
                return MessageDirection.Server;

            if (dir == MessageDirection.Server)
                return MessageDirection.Client;

            throw new Exception();
        }

        // Returns the hex-form of the specified byte array.
        private static string ToHexString(byte[] bytes)
        {
            var str = string.Empty;
            for (int i = 0; i < bytes.Length; i++)
                str += bytes[i].ToString("x2");
            return str;
        }
        #endregion
    }
}
