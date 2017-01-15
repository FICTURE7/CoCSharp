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
        /// the specified <see cref="Level"/>.
        /// </summary>
        /// <param name="level"><see cref="Avatar"/> from which the data will be set.</param>
        /// <exception cref="ArgumentNullException"><paramref name="level"/> is null.</exception>
        public VillageMessageComponent(Level level)
        {
            if (level == null)
                throw new ArgumentNullException(nameof(level));

            HomeId = level.Avatar.Id;
            ShieldDuration = level.Avatar.ShieldDuration;

            //GuardDuration = 1800;
            //Unknown3 = 69119;
            //Unknown4 = 1200;


            //Unknown5 = 60;
            VillageJson = level.Village.ToJson();
        }

        /// <summary>
        /// Home ID.
        /// </summary>
        public long HomeId;
        /// <summary>
        /// Duration of shield.
        /// </summary>
        public TimeSpan ShieldDuration;
        /// <summary>
        /// Duration of guard.
        /// </summary>
        public TimeSpan GuardDuration;

        /// <summary>
        /// Unknown integer 1.
        /// </summary>
        public int Unknown1; // 69119 = 8.x.x seems to change

        /// <summary>
        /// Village of the avatar.
        /// </summary>
        public string VillageJson;
        /// <summary>
        /// JSON describing events.
        /// </summary>
        public string EventJson;

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

            HomeId = reader.ReadInt64();
            ShieldDuration = TimeSpan.FromSeconds(reader.ReadInt32());
            GuardDuration = TimeSpan.FromSeconds(reader.ReadInt32()); // 1800 = 8.x.x

            Unknown1 = reader.ReadInt32(); // 69119 = 8.x.x seems to change, might be a TimeSpan.

            if (reader.ReadBoolean())
            {
                var homeData = reader.ReadBytes();
                if (homeData == null)
                    throw new InvalidMessageException("No data was provided about Village.");

                if (homeData.Length != 0)
                {
                    // Use a BinaryReader for little-endian reading.
                    var mem = new MemoryStream(homeData);
                    using (var br = new BinaryReader(mem))
                    {
                        var decompressedLength = br.ReadInt32();
                        var compressedHome = br.ReadBytes(homeData.Length - 4); // -4 to remove the decompressedLength bytes read.
                        var homeJson = ZlibStream.UncompressString(compressedHome);
                        VillageJson = homeJson;
                    }
                }
            }

            if (reader.ReadBoolean())
            {
                var eventData = reader.ReadBytes();
                if (eventData == null)
                    throw new InvalidMessageException("No data was provided about Village.");

                if (eventData.Length != 0)
                {
                    // Use a BinaryReader for little-endian reading.
                    var mem = new MemoryStream(eventData);
                    using (var br = new BinaryReader(mem))
                    {
                        var decompressedLength = br.ReadInt32();
                        var compressedEvent = br.ReadBytes(eventData.Length - 4); // -4 to remove the decompressedLength bytes read.
                        var eventJson = ZlibStream.UncompressString(compressedEvent);
                        EventJson = eventJson;
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

            writer.Write(HomeId);
            writer.Write((int)ShieldDuration.TotalSeconds);
            writer.Write((int)GuardDuration.TotalSeconds); // 1800 = 8.x.x

            writer.Write(Unknown1); // 69119 = 8.x.x seems to change, might be a TimeSpan.

            if (VillageJson != null)
            {
                writer.Write(true);

                // Uses BinaryWriter for little-endian writing.
                var mem = new MemoryStream();
                using (var bw = new BinaryWriter(mem))
                {
                    var homeJson = VillageJson;
                    var compressedHomeJson = ZlibStream.CompressString(homeJson);

                    bw.Write(homeJson.Length);
                    bw.Write(compressedHomeJson);
                    writer.Write(mem.ToArray(), true);
                }
            }
            else
            {
                writer.Write(false);
            }

            if (EventJson != null)
            {
                writer.Write(true);

                // Uses BinaryWriter for little-endian writing.
                var mem = new MemoryStream();
                using (var bw = new BinaryWriter(mem))
                {
                    var eventJson = EventJson;
                    var compressedEventJson = ZlibStream.CompressString(eventJson);

                    bw.Write(eventJson.Length);
                    bw.Write(compressedEventJson);
                    writer.Write(mem.ToArray(), true);
                }
            }
            else
            {
                writer.Write(false);
            }
        }
    }
}
