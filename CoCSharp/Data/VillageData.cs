using CoCSharp.Logic;
using CoCSharp.Networking;
using Ionic.Zlib;
using System;
using System.IO;

namespace CoCSharp.Data
{
    /// <summary>
    /// Represents a <see cref="Village"/> sent in
    /// the networking protocol.
    /// </summary>
    public class VillageData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VillageData"/> class.
        /// </summary>
        public VillageData()
        {
            // Space
        }

        /// <summary>
        /// Unknown integer 1.
        /// </summary>
        public int Unknown1; // 0
        /// <summary>
        /// User ID.
        /// </summary>
        public long UserID;
        /// <summary>
        /// Duration of sheild.
        /// </summary>
        public TimeSpan ShieldDuration;

        /// <summary>
        /// Unknown integer 2.
        /// </summary>
        public int Unknown2; // 1200
        /// <summary>
        /// Unknown integer 3.
        /// </summary>
        public int Unknown3; // 60
        /// <summary>
        /// Village data is compressed.
        /// </summary>
        public bool Compressed;
        /// <summary>
        /// Village of the avatar.
        /// </summary>
        public Village Home;

        /// <summary>
        /// Unknown integer 4.
        /// </summary>
        public int Unknown4; // 0

        /// <summary>
        /// Reads the <see cref="VillageData"/> from the specified <see cref="MessageReader"/>.
        /// </summary>
        /// <param name="reader">
        /// <see cref="MessageReader"/> that will be used to read the <see cref="VillageData"/>.
        /// </param>
        /// <exception cref="NotImplementedException">Compressed set to false.</exception>
        /// <exception cref="InvalidMessageException">Home data array is null.</exception>
        public void Read(MessageReader reader)
        {
            if (!Compressed)
                throw new NotImplementedException("Uncompressed Village definition is not implemented.");

            Unknown1 = reader.ReadInt32(); // 0
            UserID = reader.ReadInt64();
            ShieldDuration = TimeSpan.FromSeconds(reader.ReadInt32());
            Unknown2 = reader.ReadInt32(); // 1200
            Unknown3 = reader.ReadInt32(); // 60
            Compressed = reader.ReadBoolean();

            var homeData = reader.ReadBytes();
            if (homeData == null)
                throw new InvalidMessageException("No data was provided about Village.");

            using (var br = new BinaryReader(new MemoryStream(homeData))) // little endian
            {
                var decompressedLength = br.ReadInt32();
                var compressedHome = br.ReadBytes(homeData.Length - 4); // -4 to remove the decompressedLength bytes read
                var homeJson = ZlibStream.UncompressString(compressedHome);
                Home = Village.FromJson(homeJson);
            }

            Unknown4 = reader.ReadInt32();
        }

        /// <summary>
        /// Writes the <see cref="VillageData"/> to the specified <see cref="MessageWriter"/>.
        /// </summary>
        /// <param name="writer">
        /// <see cref="MessageWriter"/> that will be used to write the <see cref="VillageData"/>.
        /// </param>
        /// <exception cref="NotImplementedException">Compressed set to false.</exception>
        public void Write(MessageWriter writer)
        {
            if (!Compressed) // quit early just not to mess up the stream
                throw new NotImplementedException("Uncompressed Village definition is not implemented.");

            writer.Write(Unknown1); // 0
            writer.Write(UserID);
            writer.Write((int)ShieldDuration.TotalSeconds);
            writer.Write(Unknown2); // 1200
            writer.Write(Unknown3); // 60
            writer.Write(Compressed);

            using (var bw = new BinaryWriter(new MemoryStream()))
            {
                var homeJson = Home.ToJson();
                var comHomeJson = ZlibStream.CompressString(homeJson);

                bw.Write(homeJson.Length);
                bw.Write(comHomeJson);
                writer.Write(((MemoryStream)bw.BaseStream).ToArray(), true);
            }

            writer.Write(Unknown4);
        }
    }
}
