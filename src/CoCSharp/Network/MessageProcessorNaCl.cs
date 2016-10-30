using CoCSharp.Network.Cryptography;
using System;
using System.Diagnostics;
using System.IO;

namespace CoCSharp.Network
{
    /// <summary>
    /// Represents a processor to process <see cref="Message"/> that was encrypted using <c>libsodium</c>.
    /// </summary>
    public class MessageProcessorNaCl : MessageProcessor
    {
        public enum NaClState
        {
            Handshaking,

            Authentifying,

            Authentified,

            Completed
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageProcessorNaCl"/>.
        /// </summary>
        public MessageProcessorNaCl(CoCKeyPair keyPair)
        {
            if (keyPair == null)
                throw new ArgumentNullException(nameof(keyPair));

            _keyPair = keyPair;
        }

        private int _state;
        private NaClState _nstate;
        private MessageDirection _direction;
        private CoCKeyPair _keyPair;
        private Crypto8 _crypto;

        private byte[] _key;
        private byte[] _sessionKey;
        private byte[] _remoteNonce;
        private byte[] _localNonce;

        /// <summary>
        /// Gets the <see cref="NaClState"/> state of the <see cref="MessageProcessorNaCl"/>.
        /// </summary>
        public NaClState State => _nstate;

        /// <summary>
        /// Gets the <see cref="CoCCrypto"/> that is going to decrypt incoming and encrypt outgoing
        /// messages.
        /// </summary>
        public override CoCCrypto Crypto => _crypto;

        public byte[] SharedKey => _key;

        public byte[] SessionKey => _sessionKey;

        public byte[] RemoteNonce => _remoteNonce;

        public byte[] LocalNonce => _localNonce;

        /// <summary>
        /// Processes the specified chippered array of bytes and returns
        /// the resulting <see cref="Message"/>.
        /// </summary>
        /// <param name="header">Header of the message.</param>
        /// <param name="chiper">Chippered array of bytes representing a message to process.</param>
        /// <returns>Resulting <see cref="Message"/>.</returns>
        public override Message ProcessIncoming(MessageHeader header, byte[] chiper, ref byte[] plaintext)
        {
            if (chiper == null)
                throw new ArgumentNullException(nameof(chiper));

            // Direction of where the message is *going to*.
            var messageDirection = Message.GetMessageDirection(header.ID);
            // Message instance that we will return to the caller.
            var message = MessageFactory.Create(header.ID);

            // Unencrypted byte array.
            plaintext = null;

            if (_state == 0) // Handshaking.
            {
                // Handshakes are sent unencrypted.
                // First message by both ends is always sent unencrypted.
                plaintext = (byte[])chiper.Clone();

                // Use the first message direction to configure the processor.
                _direction = messageDirection;
                _crypto = new Crypto8(GetOppositeDirection(_direction), _keyPair);

                Debug.WriteLine($"Initialized Crypto8 {GetOppositeDirection(_direction)} with private-key: {ToHexString(_keyPair.PrivateKey)}, public-key {_keyPair.PublicKey}");
            }

            // If somehow the incoming message process comes from the same direction.
            // The MessageProcessor can only process message going in One Direction. ;]
            if (messageDirection != _direction)
                throw new Exception();

            // _state == 1 means we are the server.
            // Usually processing 10101 - LoginRequestMessage.
            if (_state == 1)
            {
                // -> Pre-Encryption.
                // Copies the public key appended to the beginning of the message
                // sent of message 20100.
                var publicKey = new byte[CoCKeyPair.KeyLength];
                Buffer.BlockCopy(chiper, 0, publicKey, 0, CoCKeyPair.KeyLength);

                Debug.WriteLine($"Public-Key from {header.ID}: {ToHexString(publicKey)}");

                // Copies the remaining bytes into the plaintext buffer which
                // will be decrypted by _crypto.
                var plaintextLen = header.Length - CoCKeyPair.KeyLength;
                plaintext = new byte[plaintextLen];
                Buffer.BlockCopy(chiper, CoCKeyPair.KeyLength, plaintext, 0, plaintextLen);

                // Crypto8 will take publicKey & _keyPair.PublicKey and generate a blake2b nonce
                _crypto.UpdateSharedKey(publicKey);
                // Then use _keyPair.PrivateKey, publicKey and nonce to decrypt.
                _crypto.Decrypt(ref plaintext);

                // -> Post-Encryption.
                _sessionKey = new byte[CoCKeyPair.NonceLength];
                _remoteNonce = new byte[CoCKeyPair.NonceLength];

                // Copy the SessionKey and the ClientNonce.
                Buffer.BlockCopy(plaintext, 0, _sessionKey, 0, CoCKeyPair.NonceLength);
                Buffer.BlockCopy(plaintext, CoCKeyPair.NonceLength, _remoteNonce, 0, CoCKeyPair.NonceLength);

                Debug.WriteLine($"Session-key from {header.ID}: {ToHexString(_sessionKey)}");
                Debug.WriteLine($"Client-nonce from {header.ID}: {ToHexString(_remoteNonce)}");

                var actualMessage = new byte[plaintext.Length - (CoCKeyPair.NonceLength * 2)];
                Buffer.BlockCopy(plaintext, CoCKeyPair.NonceLength * 2, actualMessage, 0, actualMessage.Length);

                plaintext = actualMessage;
                _nstate = NaClState.Authentifying;
            }
            // _state == 2 means we are the client.
            // Usually processing 20104 - LoginSuccessMessage.
            else if (_state == 2)
            {
                // _local cannot be null.
                if (_localNonce == null)
                    throw new Exception();

                plaintext = (byte[])chiper.Clone();

                _crypto.UpdateNonce(_localNonce, UpdateNonceType.Encrypt);
                _crypto.UpdateNonce(_localNonce, UpdateNonceType.Blake);
                _crypto.Decrypt(ref plaintext);

                // -> Post-Encryption
                _remoteNonce = new byte[CoCKeyPair.NonceLength];
                _key = new byte[CoCKeyPair.KeyLength];

                Buffer.BlockCopy(plaintext, 0, _remoteNonce, 0, CoCKeyPair.NonceLength);
                Buffer.BlockCopy(plaintext, CoCKeyPair.NonceLength, _key, 0, CoCKeyPair.KeyLength);

                Debug.WriteLine($"Shared-key from {header.ID}: {ToHexString(_key)}");
                Debug.WriteLine($"Server-nonce from {header.ID}: {ToHexString(_remoteNonce)}");

                var actualMessge = new byte[plaintext.Length - CoCKeyPair.NonceLength - CoCKeyPair.KeyLength];
                Buffer.BlockCopy(plaintext, CoCKeyPair.NonceLength + CoCKeyPair.KeyLength, actualMessge, 0, actualMessge.Length);

                _crypto.UpdateNonce(_remoteNonce, UpdateNonceType.Decrypt);
                _crypto.UpdateSharedKey(_key);

                plaintext = actualMessge;
                _nstate = NaClState.Authentified;
            }
            else if (_state > (int)_direction)
            {
                _nstate = NaClState.Completed;
                plaintext = (byte[])chiper.Clone();

                _crypto.Decrypt(ref plaintext);
            }

            Debug.Assert(plaintext != null);

            using (var reader = new MessageReader(new MemoryStream(plaintext)))
            {
                var exception = (Exception)null;

                try
                {
                    message.ReadMessage(reader);
                }
                catch (Exception ex) { exception = ex; }
            }

            _state += (int)_direction;
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

            return null;
        }

        public void UpdateRemoteNonce(byte[] nonce)
        {
            _remoteNonce = nonce;
        }

        public void UpdateLocalNonce(byte[] nonce)
        {
            _localNonce = nonce;
        }

        private static MessageDirection GetOppositeDirection(MessageDirection dir)
        {
            if (dir == MessageDirection.Client)
                return MessageDirection.Server;

            if (dir == MessageDirection.Server)
                return MessageDirection.Client;

            throw new Exception();
        }

        private static string ToHexString(byte[] bytes)
        {
            var str = string.Empty;
            for (int i = 0; i < bytes.Length; i++)
                str += bytes[i].ToString("x2");
            return str;
        }
    }
}
