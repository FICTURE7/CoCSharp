using CoCSharp.Network;
using System;

namespace CoCSharp.Network.Messages
{
    /// <summary>
    /// Message that is sent by the server to the client to
    /// provide data about a visited village. Sent after <see cref="VisitHomeMessage"/>.
    /// </summary>
    public class VisitHomeDataMessage : Message
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VisitHomeDataMessage"/> class.
        /// </summary>
        public VisitHomeDataMessage()
        {
            // Space
        }

        /// <summary>
        /// Gets the ID of the <see cref="VisitHomeDataMessage"/>.
        /// </summary>
        public override ushort Id { get { return 24113; } }

        /// <summary>
        /// Time since last visit.
        /// </summary>
        public TimeSpan LastVisit;
        /// <summary>
        /// Timestamp.
        /// </summary>
        public DateTime Timestamp;
        /// <summary>
        /// Visit village data.
        /// </summary>
        public VillageMessageComponent VisitVillageData;
        /// <summary>
        /// Visit avatar data.
        /// </summary>
        public AvatarMessageComponent VisitAvatarData;

        /// <summary>
        /// Unknown integer 1.
        /// </summary>
        public int Unknown1; // 0
        /// <summary>
        /// Unknown byte 2.
        /// </summary>
        public byte Unknown2; // 1

        /// <summary>
        /// Own avatar data.
        /// </summary>
        public AvatarMessageComponent OwnAvatarData;

        /// <summary>
        /// Reads the <see cref="VisitHomeDataMessage"/> from the specified <see cref="MessageReader"/>.
        /// </summary>
        /// <param name="reader">
        /// <see cref="MessageReader"/> that will be used to read the <see cref="VisitHomeDataMessage"/>.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="reader"/> is null.</exception>
        public override void ReadMessage(MessageReader reader)
        {
            ThrowIfReaderNull(reader);

            LastVisit = TimeSpan.FromSeconds(reader.ReadInt32());
            Timestamp = TimeUtils.FromUnixTimestamp(reader.ReadInt32());

            VisitVillageData = new VillageMessageComponent();
            VisitVillageData.ReadMessageComponent(reader);

            VisitAvatarData = new AvatarMessageComponent();
            VisitAvatarData.ReadMessageComponent(reader);

            Unknown1 = reader.ReadInt32();
            Unknown2 = reader.ReadByte();

            OwnAvatarData = new AvatarMessageComponent();
            OwnAvatarData.ReadMessageComponent(reader);
        }

        /// <summary>
        /// Writes the <see cref="VisitHomeDataMessage"/> to the specified <see cref="MessageWriter"/>.
        /// </summary>
        /// <param name="writer">
        /// <see cref="MessageWriter"/> that will be used to write the <see cref="VisitHomeDataMessage"/>.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="writer"/> is null.</exception>
        /// <exception cref="InvalidOperationException"><see cref="VisitVillageData"/> is null.</exception>
        /// <exception cref="InvalidOperationException"><see cref="VisitAvatarData"/> is null.</exception>
        /// <exception cref="InvalidOperationException"><see cref="OwnAvatarData"/> is null.</exception>
        public override void WriteMessage(MessageWriter writer)
        {
            ThrowIfWriterNull(writer);

            if (VisitVillageData == null)
                throw new InvalidOperationException("EnemyVillageData cannot be null.");
            if (VisitAvatarData == null)
                throw new InvalidOperationException("EnemyHomeAvatarData cannot be null.");
            if (OwnAvatarData == null)
                throw new InvalidOperationException("OwnAvatarData cannot be null.");

            writer.Write((int)LastVisit.TotalSeconds);
            writer.Write((int)TimeUtils.ToUnixTimestamp(Timestamp));
            VisitVillageData.WriteMessageComponent(writer);
            VisitAvatarData.WriteMessageComponent(writer);

            writer.Write(Unknown1);
            writer.Write(Unknown2);

            OwnAvatarData.WriteMessageComponent(writer);
        }
    }
}
