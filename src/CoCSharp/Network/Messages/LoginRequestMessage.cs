﻿using System;

namespace CoCSharp.Network.Messages
{
    /// <summary>
    /// Message that is sent by the client to the server to request
    /// for a login.
    /// </summary>
    public class LoginRequestMessage : Message
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LoginRequestMessage"/> class.
        /// </summary>
        public LoginRequestMessage()
        {
            // Space
        }

        /// <summary>
        /// User ID needed to login in a specific account.
        /// </summary>
        public long UserId;
        /// <summary>
        /// User token needed to login in a specific account.
        /// </summary>
        public string UserToken;
        /// <summary>
        /// Client major version. If the client version is incorrect
        /// the server will respond with a <see cref="LoginFailedMessage"/>.
        /// </summary>
        public int MajorVersion;
        /// <summary>
        /// Client content version. If the client version is incorrect
        /// the server will respond with a <see cref="LoginFailedMessage"/>.
        /// </summary>
        public int ContentVersion;
        /// <summary>
        /// Client minor version. If the client version is incorrect
        /// the server will respond with a <see cref="LoginFailedMessage"/>.
        /// </summary>
        public int MinorVersion;
        /// <summary>
        /// MasterHash of 'fingerprint.json'. If the fingerprint master hash is incorrect
        /// the server will respond with a <see cref="LoginFailedMessage"/>.
        /// </summary>
        public string MasterHash;
        /// <summary>
        /// UDID.
        /// </summary>
        public string Udid;
        /// <summary>
        /// Open UDID.
        /// </summary>
        public string OpenUdid;
        /// <summary>
        /// MAC address of the device.
        /// </summary>
        public string MacAddress;
        /// <summary>
        /// Model of the device.
        /// </summary>
        public string DeviceModel;
        /// <summary>
        /// Locale key.
        /// </summary>
        public int LocaleKey;
        /// <summary>
        /// Language of the device.
        /// </summary>
        public string Language;
        /// <summary>
        /// Advertising GUID.
        /// </summary>
        public string AdvertisingGuid;
        /// <summary>
        /// Operating system version.
        /// </summary>
        public string OSVersion;

        /// <summary>
        /// Unknown byte 2.
        /// </summary>
        public byte Unknown2;
        /// <summary>
        /// Unknown string 3.
        /// </summary>
        public string Unknown3;

        /// <summary>
        /// Android device ID.
        /// </summary>
        public string AndroidDeviceId;
        /// <summary>
        /// Facebook distribution ID.
        /// </summary>
        public string FacebookDistributionID;
        /// <summary>
        /// Is advertising tracking enabled.
        /// </summary>
        public bool IsAdvertisingTrackingEnabled;
        /// <summary>
        /// Vendor GUID.
        /// </summary>
        public string VendorGUID;
        /// <summary>
        /// Seed that was needed for encryption before update 8.x.x.
        /// </summary>
        public int Seed;
        
        /// <summary>
        /// Unknown byte 4.
        /// </summary>
        public byte Unknown4;
        /// <summary>
        /// Unknown string 5.
        /// </summary>
        public string Unknown5;
        /// <summary>
        /// Unknown string 6.
        /// </summary>
        public string Unknown6;

        /// <summary>
        /// Client version string.
        /// </summary>
        public string ClientVersion;

        /// <summary>
        ///  Gets the ID of the <see cref="LoginRequestMessage"/>.
        /// </summary>
        public override ushort Id { get { return 10101; } }

        /// <summary>
        /// Reads the <see cref="LoginRequestMessage"/> from the specified <see cref="MessageReader"/>.
        /// </summary>
        /// <param name="reader">
        /// <see cref="MessageReader"/> that will be used to read the <see cref="LoginRequestMessage"/>.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="reader"/> is null.</exception>
        public override void ReadMessage(MessageReader reader)
        {
            ThrowIfReaderNull(reader);

            UserId = reader.ReadInt64();
            UserToken = reader.ReadString();
            MajorVersion = reader.ReadInt32();
            ContentVersion = reader.ReadInt32();
            MinorVersion = reader.ReadInt32();
            MasterHash = reader.ReadString();

            Udid = reader.ReadString();

            OpenUdid = reader.ReadString();
            MacAddress = reader.ReadString();
            DeviceModel = reader.ReadString();
            LocaleKey = reader.ReadInt32();
            Language = reader.ReadString();
            AdvertisingGuid = reader.ReadString();
            OSVersion = reader.ReadString();

            Unknown2 = reader.ReadByte();
            Unknown3 = reader.ReadString();

            AndroidDeviceId = reader.ReadString();
            FacebookDistributionID = reader.ReadString();
            IsAdvertisingTrackingEnabled = reader.ReadBoolean();
            VendorGUID = reader.ReadString();
            Seed = reader.ReadInt32();

            Unknown4 = reader.ReadByte();
            Unknown5 = reader.ReadString();
            Unknown6 = reader.ReadString();

            ClientVersion = reader.ReadString();
        }

        /// <summary>
        /// Writes the <see cref="LoginRequestMessage"/> to the specified <see cref="MessageWriter"/>.
        /// </summary>
        /// <param name="writer">
        /// <see cref="MessageWriter"/> that will be used to write the <see cref="LoginRequestMessage"/>.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="writer"/> is null.</exception>
        public override void WriteMessage(MessageWriter writer)
        {
            ThrowIfWriterNull(writer);

            writer.Write(UserId);
            writer.Write(UserToken);
            writer.Write(MajorVersion);
            writer.Write(ContentVersion);
            writer.Write(MinorVersion);
            writer.Write(MasterHash);

            writer.Write(Udid);

            writer.Write(OpenUdid);
            writer.Write(MacAddress);
            writer.Write(DeviceModel);
            writer.Write(LocaleKey);
            writer.Write(Language);
            writer.Write(AdvertisingGuid);
            writer.Write(OSVersion);

            writer.Write(Unknown2);
            writer.Write(Unknown3);

            writer.Write(AndroidDeviceId);
            writer.Write(FacebookDistributionID);
            writer.Write(IsAdvertisingTrackingEnabled);
            writer.Write(VendorGUID);
            writer.Write(Seed);

            writer.Write(Unknown4);
            writer.Write(Unknown5);
            writer.Write(Unknown6);

            writer.Write(ClientVersion);
        }
    }
}
