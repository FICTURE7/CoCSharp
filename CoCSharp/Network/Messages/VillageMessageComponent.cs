using CoCSharp.Logic;
using Ionic.Zlib;
using System;
using System.IO;

namespace CoCSharp.Network.Messages
{
    /// <summary>
    /// Represents a <see cref="Village"/> sent in
    /// the networking protocol.
    /// </summary>
    public class VillageMessageComponent : MessageComponent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VillageMessageComponent"/> class.
        /// </summary>
        public VillageMessageComponent()
        {
            // Space
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VillageMessageComponent"/> class from
        /// the specified <see cref="Avatar"/>.
        /// </summary>
        /// <param name="avatar"><see cref="Avatar"/> from which the data will be set.</param>
        /// <exception cref="ArgumentNullException"><paramref name="avatar"/> is null.</exception>
        public VillageMessageComponent(Avatar avatar)
        {
            if (avatar == null)
                throw new ArgumentNullException("avatar");

            HomeID = avatar.ID;
            ShieldDuration = avatar.ShieldDuration;

            //GuardDuration = 1800;
            //Unknown3 = 69119;
            Unknown4 = 1200;

            Unknown5 = 60;
            Home = avatar.Home;
        }

        /// <summary>
        /// Unknown integer 1.
        /// </summary>
        public int Unknown1; // 0

        /// <summary>
        /// Home ID.
        /// </summary>
        public long HomeID;
        /// <summary>
        /// Duration of shield.
        /// </summary>
        public TimeSpan ShieldDuration;
        /// <summary>
        /// Duration of guard.
        /// </summary>
        public TimeSpan GuardDuration; // 1800 = 8.x.x, GuardDuration?

        /// <summary>
        /// Unknown integer 3.
        /// </summary>
        public int Unknown3; // 69119 = 8.x.x seems to change
        /// <summary>
        /// Unknown integer 4.
        /// </summary>
        public int Unknown4; // 1200
        /// <summary>
        /// Unknown integer 5.
        /// </summary>
        public int Unknown5; // 60

        /// <summary>
        /// Village of the avatar.
        /// </summary>
        public Village Home;

        /// <summary>
        /// Reads the <see cref="VillageMessageComponent"/> from the specified <see cref="MessageReader"/>.
        /// </summary>
        /// <param name="reader">
        /// <see cref="MessageReader"/> that will be used to read the <see cref="VillageMessageComponent"/>.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="reader"/> is null.</exception>
        /// <exception cref="InvalidMessageException">Home data array is null.</exception>
        public override void ReadMessageComponent(MessageReader reader)
        {
            ThrowIfReaderNull(reader);

            Unknown1 = reader.ReadInt32(); // 0

            HomeID = reader.ReadInt64();
            ShieldDuration = TimeSpan.FromSeconds(reader.ReadInt32());

            GuardDuration = TimeSpan.FromSeconds(reader.ReadInt32()); // 1800 = 8.x.x
            Unknown3 = reader.ReadInt32(); // 69119 = 8.x.x seems to change, might be a TimeSpan.
            Unknown4 = reader.ReadInt32(); // 1200
            Unknown5 = reader.ReadInt32(); // 60

            if (reader.ReadBoolean())
            {
                var homeData = reader.ReadBytes();
                if (homeData == null)
                    throw new InvalidMessageException("No data was provided about Village.");

                if (homeData.Length != 0)
                {
                    // Use a BinaryReader for little-endian reading.
                    using (var mem = new MemoryStream(homeData))
                    using (var br = new BinaryReader(mem))
                    {
                        var decompressedLength = br.ReadInt32();
                        var compressedHome = br.ReadBytes(homeData.Length - 4); // -4 to remove the decompressedLength bytes read.
                        var homeJson = ZlibStream.UncompressString(compressedHome);
                        Home = Village.FromJson(homeJson);
                    }
                }
            }
        }

        /// <summary>
        /// Writes the <see cref="VillageMessageComponent"/> to the specified <see cref="MessageWriter"/>.
        /// </summary>
        /// <param name="writer">
        /// <see cref="MessageWriter"/> that will be used to write the <see cref="VillageMessageComponent"/>.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="writer"/> is null.</exception>
        public override void WriteMessageComponent(MessageWriter writer)
        {
            ThrowIfWriterNull(writer);

            writer.Write(Unknown1); // 0

            writer.Write(HomeID);
            writer.Write((int)ShieldDuration.TotalSeconds);

            writer.Write((int)GuardDuration.TotalSeconds); // 1800 = 8.x.x
            writer.Write(Unknown3); // 69119 = 8.x.x seems to change, might be a TimeSpan.
            writer.Write(Unknown4); // 1200
            writer.Write(Unknown5); // 60

            if (Home != null)
            {
                writer.Write(true);

                // Uses BinaryWriter for little-endian writing.
                using (var mem = new MemoryStream())
                using (var bw = new BinaryWriter(mem))
                {
                    var homeJson = Home.ToJson();
                    var compressedHomeJson = ZlibStream.CompressString(homeJson);

                    bw.Write(homeJson.Length);
                    bw.Write(compressedHomeJson);
                    writer.Write(mem.ToArray(), true);
                }
            }
        }
    }
}
