using CoCSharp.Data;
using System;

namespace CoCSharp.Network.Messages
{
    /// <summary>
    /// Message that is sent by the server after the <see cref="LoginSuccessMessage"/>
    /// is sent.
    /// </summary>
    public class OwnHomeDataMessage : Message
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OwnHomeDataMessage"/> class.
        /// </summary>
        public OwnHomeDataMessage()
        {
            // Space
        }

        /// <summary>
        /// Gets the ID of the <see cref="OwnHomeDataMessage"/>.
        /// </summary>
        public override ushort ID { get { return 24101; } }

        /// <summary>
        /// Time since last visited.
        /// </summary>
        public TimeSpan LastVisit;

        /// <summary>
        /// Unknown string 1.
        /// </summary>
        public string Unknown1; // -1

        /// <summary>
        /// Server local UTC timestamp.
        /// </summary>
        public DateTime Timestamp;
        /// <summary>
        /// Own village data.
        /// </summary>
        public VillageMessageComponent OwnVillageData;
        /// <summary>
        /// Own avatar data.
        /// </summary>
        public AvatarMessageComponent OwnAvatarData;

        /// <summary>
        /// Unknown integer 2.
        /// </summary>
        public int Unknown2;
        /// <summary>
        /// Unknown integer 3.
        /// </summary>
        public int Unknown3;
        /// <summary>
        /// Unknown long 4.
        /// </summary>
        public long Unknown4;
        /// <summary>
        /// Unknown long 5.
        /// </summary>
        public long Unknown5;
        /// <summary>
        /// Unknown long 6.
        /// </summary>
        public long Unknown6;

        /// <summary>
        /// Reads the <see cref="OwnHomeDataMessage"/> from the specified <see cref="MessageReader"/>.
        /// </summary>
        /// <param name="reader">
        /// <see cref="MessageReader"/> that will be used to read the <see cref="OwnHomeDataMessage"/>.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="reader"/> is null.</exception>
        public override void ReadMessage(MessageReader reader)
        {
            ThrowIfReaderNull(reader);

            LastVisit = TimeSpan.FromSeconds(reader.ReadInt32());

            Unknown1 = reader.ReadString(); // -1

            Timestamp = DateTimeConverter.FromUnixTimestamp(reader.ReadInt32());
            OwnVillageData = new VillageMessageComponent();
            OwnVillageData.ReadMessageComponent(reader);
            OwnAvatarData = new AvatarMessageComponent();
            OwnAvatarData.ReadMessageComponent(reader);

            Unknown2 = reader.ReadInt32();
            Unknown3 = reader.ReadInt32();
            Unknown4 = reader.ReadInt64(); // 1462629754000
            Unknown5 = reader.ReadInt64(); // 1462629754000
            Unknown6 = reader.ReadInt64(); // 1462631554000
        }

        /// <summary>
        /// Writes the <see cref="OwnHomeDataMessage"/> to the specified <see cref="MessageWriter"/>.
        /// </summary>
        /// <param name="writer">
        /// <see cref="MessageWriter"/> that will be used to write the <see cref="OwnHomeDataMessage"/>.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="writer"/> is null.</exception>
        /// <exception cref="InvalidOperationException"><see cref="OwnVillageData"/> is null.</exception>
        /// <exception cref="InvalidOperationException"><see cref="OwnAvatarData"/> is null.</exception>
        public override void WriteMessage(MessageWriter writer)
        {
            ThrowIfWriterNull(writer);

            if (OwnVillageData == null)
                throw new InvalidOperationException("OwnVillageData cannot be null.");
            if (OwnAvatarData == null)
                throw new InvalidOperationException("OwnAvatarData cannot be null.");

            writer.Write((int)LastVisit.TotalSeconds);

            writer.Write(Unknown1); // -1

            writer.Write((int)DateTimeConverter.ToUnixTimestamp(Timestamp));
            OwnVillageData.WriteMessageComponent(writer);
            OwnAvatarData.WriteMessageComponent(writer);

            writer.Write(Unknown2);
            writer.Write(Unknown3);
            writer.Write(Unknown4); // 1462629754000
            writer.Write(Unknown5); // 1462629754000
            writer.Write(Unknown6); // 1462631554000
        }
    }
}
