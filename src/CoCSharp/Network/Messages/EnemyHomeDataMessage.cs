using CoCSharp.Network;
using Ionic.Zlib;
using System;
using System.IO;

namespace CoCSharp.Network.Messages
{
    /// <summary>
    /// Message that is sent by the server to the client to provide
    /// information about the enemy's home.
    /// </summary>
    public class EnemyHomeDataMessage : Message
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EnemyHomeDataMessage"/> class.
        /// </summary>
        public EnemyHomeDataMessage()
        {
            // Space
        }

        /// <summary>
        /// Gets the ID of the <see cref="EnemyHomeDataMessage"/>.
        /// </summary>
        public override ushort Id { get { return 24107; } }

        /// <summary>
        /// Time since last visit.
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
        /// Enemy village data.
        /// </summary>
        public VillageMessageComponent EnemyVillageData;
        /// <summary>
        /// Enemy avatar data.
        /// </summary>
        public AvatarMessageComponent EnemyAvatarData;
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
        /// Unknown byte 4.
        /// </summary>
        public byte Unknown4;

        /// <summary>
        /// Reads the <see cref="EnemyHomeDataMessage"/> from the specified <see cref="MessageReader"/>.
        /// </summary>
        /// <param name="reader">
        /// <see cref="MessageReader"/> that will be used to read the <see cref="EnemyHomeDataMessage"/>.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="reader"/> is null.</exception>
        public override void ReadMessage(MessageReader reader)
        {
            ThrowIfReaderNull(reader);

            LastVisit = TimeSpan.FromSeconds(reader.ReadInt32());

            Unknown1 = reader.ReadString(); // -1

            Timestamp = TimeUtils.FromUnixTimestamp(reader.ReadInt32());
            EnemyVillageData = new VillageMessageComponent();
            EnemyVillageData.ReadMessageComponent(reader);           

            EnemyAvatarData = new AvatarMessageComponent();
            EnemyAvatarData.ReadMessageComponent(reader);

            OwnAvatarData = new AvatarMessageComponent();
            OwnAvatarData.ReadMessageComponent(reader);

            Unknown2 = reader.ReadInt32();
            Unknown3 = reader.ReadInt32();
            Unknown4 = reader.ReadByte();
        }

        /// <summary>
        /// Writes the <see cref="EnemyHomeDataMessage"/> to the specified <see cref="MessageWriter"/>.
        /// </summary>
        /// <param name="writer">
        /// <see cref="MessageWriter"/> that will be used to write the <see cref="EnemyHomeDataMessage"/>.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="writer"/> is null.</exception>
        /// <exception cref="InvalidOperationException"><see cref="EnemyVillageData"/> is null.</exception>
        /// <exception cref="InvalidOperationException"><see cref="EnemyAvatarData"/> is null.</exception>
        /// <exception cref="InvalidOperationException"><see cref="OwnAvatarData"/> is null.</exception>
        public override void WriteMessage(MessageWriter writer)
        {
            ThrowIfWriterNull(writer);

            if (EnemyVillageData == null)
                throw new InvalidOperationException("EnemyVillageData cannot be null.");
            if (EnemyAvatarData == null)
                throw new InvalidOperationException("EnemyHomeAvatarData cannot be null.");
            if (OwnAvatarData == null)
                throw new InvalidOperationException("OwnAvatarData cannot be null.");

            writer.Write((int)LastVisit.TotalSeconds);

            writer.Write(Unknown1);

            writer.Write((int)TimeUtils.ToUnixTimestamp(Timestamp));
            EnemyVillageData.WriteMessageComponent(writer);
            EnemyAvatarData.WriteMessageComponent(writer);
            OwnAvatarData.WriteMessageComponent(writer);

            writer.Write(Unknown2);
            writer.Write(Unknown3);
            writer.Write(Unknown4);
        }
    }
}
