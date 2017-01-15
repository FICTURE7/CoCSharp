using CoCSharp.Data;
using CoCSharp.Network.Cryptography;
using Ionic.Zlib;
using System;
using System.IO;

namespace CoCSharp.Network.Messages
{
    /// <summary>
    /// Defines login failure reasons.
    /// </summary>
    public enum LoginFailureReason : int
    {
        /// <summary>
        /// Content version is outdated. This occurs when the client's fingerprint hash is not equal
        /// to the server's fingerprint hash.
        /// </summary>
        OutdatedContent = 7,

        /// <summary>
        /// Client revision is outdated. This occurs when the client's version is not equal
        /// to the server's expected version.
        /// </summary>
        OutdatedVersion = 8,

        /// <summary>
        /// Unknown reason 1.
        /// </summary>
        Unknown1 = 9,

        /// <summary>
        /// Server is in maintenance.
        /// </summary>
        Maintenance = 10,

        /// <summary>
        /// Temporarily banned.
        /// </summary>
        TemporarilyBanned = 11,

        /// <summary>
        /// Take a rest. This occurs when the connection to the server has been maintain for too long.
        /// </summary>
        TakeRest = 12,

        /// <summary>
        /// Account has been locked. It can only be unlocked with a specific PIN.
        /// </summary>
        Locked = 13
    };

    /// <summary>
    /// Message that is sent by the server to the client when a <see cref="LoginRequestMessage"/>
    /// failed.
    /// </summary>
    public class LoginFailedMessage : Message
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LoginFailedMessage"/>.
        /// </summary>
        public LoginFailedMessage()
        {
            // Space
        }

        /// <summary>
        /// Reason why login failed.
        /// </summary>
        public LoginFailureReason Reason;
        /// <summary>
        /// Uncompressed fingerprint JSON.
        /// </summary>
        public string FingerprintJson;
        /// <summary>
        /// Host name.
        /// </summary>
        public string Hostname;
        /// <summary>
        /// Download root URL from where all the assets will be downloaded.
        /// </summary>
        public string ContentUrl;
        /// <summary>
        /// Market URL.
        /// </summary>
        public string MarketUrl;
        /// <summary>
        /// Message shown when under maintenance.
        /// </summary>
        public string Message;
        /// <summary>
        /// Duration of the maintenance.
        /// </summary>
        public TimeSpan MaintenanceDuration;

        /// <summary>
        /// Unknown byte 4.
        /// </summary>
        public byte Unknown4;

        /// <summary>
        /// New fingerprint that the server wants the client
        /// to use.
        /// </summary>
        public string FingerprintJsonCompressed;

        /// <summary>
        /// Unknown integer 6.
        /// </summary>
        public int Unknown6;
        /// <summary>
        /// Unknown integer 7.
        /// </summary>
        public int Unknown7;
        /// <summary>
        /// Unknown integer 8.
        /// </summary>
        public int Unknown8;
        /// <summary>
        /// Unknown integer 9.
        /// </summary>
        public int Unknown9;

        /// <summary>
        ///  Gets the ID of the <see cref="LoginFailedMessage"/>.
        /// </summary>
        public override ushort Id { get { return 20103; } }

        /// <summary>
        /// Gets the version of the <see cref="LoginFailedMessage"/>.
        /// </summary>
        public override ushort Version { get { return 2; } }

        /// <summary>
        /// Reads the <see cref="LoginFailedMessage"/> from the specified <see cref="MessageReader"/>.
        /// </summary>
        /// <param name="reader">
        /// <see cref="MessageReader"/> that will be used to read the <see cref="LoginFailedMessage"/>.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="reader"/> is null.</exception>
        public override void ReadMessage(MessageReader reader)
        {
            ThrowIfReaderNull(reader);

            Reason = (LoginFailureReason)reader.ReadInt32(); // Returned 2 when data is not valid.

            FingerprintJson = reader.ReadString(); // null

            Hostname = reader.ReadString(); // stage.clashofclans.com
            ContentUrl = reader.ReadString(); // http://b46f744d64acd2191eda-3720c0374d47e9a0dd52be4d281c260f.r11.cf2.rackcdn.com/
            MarketUrl = reader.ReadString(); // market://details?id=com.supercell.clashofclans
            Message = reader.ReadString();
            MaintenanceDuration = TimeSpan.FromSeconds(reader.ReadInt32());

            Unknown4 = reader.ReadByte(); // 0

            //TODO: Implement Compressed String in MessageWriter and MessageReader.
            var fingerprintData = reader.ReadBytes();
            if (fingerprintData != null)
            {
                using (var br = new BinaryReader(new MemoryStream(fingerprintData)))
                {
                    var decompressedLength = br.ReadInt32();
                    var compressedFingerprint = br.ReadBytes(fingerprintData.Length - 4);
                    var fingerprintJson = ZlibStream.UncompressString(compressedFingerprint);
                    FingerprintJsonCompressed = fingerprintJson;
                }
            }

            Unknown6 = reader.ReadInt32(); // -1
            Unknown7 = reader.ReadInt32(); // 2
            Unknown8 = reader.ReadInt32(); // 0
            Unknown9 = reader.ReadInt32(); // -1
        }

        /// <summary>
        /// Writes the <see cref="LoginFailedMessage"/> to the specified <see cref="MessageWriter"/>.
        /// </summary>
        /// <param name="writer">
        /// <see cref="MessageWriter"/> that will be used to write the <see cref="LoginFailedMessage"/>.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="writer"/> is null.</exception>
        public override void WriteMessage(MessageWriter writer)
        {
            ThrowIfWriterNull(writer);

            writer.Write((int)Reason);

            writer.Write(FingerprintJson);

            writer.Write(Hostname);
            writer.Write(ContentUrl);
            writer.Write(MarketUrl);
            writer.Write(Message);
            writer.Write((int)MaintenanceDuration.TotalSeconds);
            writer.Write(Unknown4);

            if (FingerprintJsonCompressed != null)
            {
                // Uses BinaryWriter for little-endian writing.
                var mem = new MemoryStream();
                using (var bw = new BinaryWriter(mem))
                {
                    var fingerprintJson = FingerprintJsonCompressed;
                    var compressedFingerprintJson = ZlibStream.CompressString(fingerprintJson);

                    bw.Write(fingerprintJson.Length);
                    bw.Write(compressedFingerprintJson);
                    writer.Write(mem.ToArray(), true);
                }
            }
            else
            {
                writer.Write(-1);
            }

            writer.Write(Unknown6);
            writer.Write(Unknown7);
            writer.Write(Unknown8);
            writer.Write(Unknown9);
        }
    }
}
