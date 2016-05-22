using CoCSharp.Logic;
using Ionic.Zlib;
using System;
using System.IO;

namespace CoCSharp.Network.Messages
{
    /// <summary>
    /// Message that is sent by the server to the client to
    /// provide the client with the avatar profile information.
    /// </summary>
    public class AvatarProfileResponseMessage : Message
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AvatarProfileResponseMessage"/> class.
        /// </summary>
        public AvatarProfileResponseMessage()
        {
            // Space
        }

        /// <summary>
        /// Gets the ID of the <see cref="AvatarProfileResponseMessage"/>.
        /// </summary>
        public override ushort ID { get { return 24334; } }

        /// <summary>
        /// Data about the requested avatar profile.
        /// </summary>
        public AvatarMessageComponent AvatarData;
        /// <summary>
        /// Village of requested avatar profile.
        /// </summary>
        public Village Village;
        /// <summary>
        /// Amount of troops donated.
        /// </summary>
        public int TroopsDonated;
        /// <summary>
        /// Amount of troops received.
        /// </summary>
        public int TroopsReceived;
        /// <summary>
        /// Time renaming to be eligible for war.
        /// </summary>
        public TimeSpan WarCoolDown;

        /// <summary>
        /// Unknown integer 2.
        /// </summary>
        public int Unknown2;
        /// <summary>
        /// Unknown boolean 3.
        /// </summary>
        public bool Unknown3;

        /// <summary>
        /// Reads the <see cref="AvatarProfileResponseMessage"/> from the specified <see cref="MessageReader"/>.
        /// </summary>
        /// <param name="reader">
        /// <see cref="MessageReader"/> that will be used to read the <see cref="AvatarProfileResponseMessage"/>.
        /// </param>
        public override void ReadMessage(MessageReader reader)
        {
            AvatarData = new AvatarMessageComponent();
            AvatarData.ReadMessageComponent(reader);

            var villageBytes = reader.ReadBytes();
            if (villageBytes.Length != 0)
            {
                using (var mem = new MemoryStream(villageBytes))
                using (var br = new BinaryReader(mem))
                {
                    var decompressedLength = br.ReadInt32();
                    var compressedVillage = br.ReadBytes(villageBytes.Length - 4);
                    var villageJson = ZlibStream.UncompressString(compressedVillage);
                    Village = Village.FromJson(villageJson);
                }
            }

            TroopsDonated = reader.ReadInt32();
            TroopsReceived = reader.ReadInt32();
            WarCoolDown = TimeSpan.FromSeconds(reader.ReadInt32());

            Unknown2 = reader.ReadInt32();
            Unknown3 = reader.ReadBoolean();
        }

        /// <summary>
        /// Writes the <see cref="AvatarProfileResponseMessage"/> to the specified <see cref="MessageWriter"/>.
        /// </summary>
        /// <param name="writer">
        /// <see cref="MessageWriter"/> that will be used to write the <see cref="AvatarProfileResponseMessage"/>.
        /// </param>
        public override void WriteMessage(MessageWriter writer)
        {
            if (AvatarData == null)
                throw new InvalidOperationException("AvatarData cannot be null.");

            AvatarData.WriteMessageComponent(writer);

            using (var mem = new MemoryStream())
            using (var bw = new BinaryWriter(mem))
            {
                var villageJson = Village.ToJson();
                var compressedVillage = ZlibStream.CompressString(villageJson);

                bw.Write(villageJson.Length);
                bw.Write(compressedVillage);
                writer.Write(mem.ToArray(), true);
            }

            writer.Write(TroopsDonated);
            writer.Write(TroopsReceived);
            writer.Write((int)WarCoolDown.TotalSeconds);

            writer.Write(Unknown2);
            writer.Write(Unknown3);
        }
    }
}
