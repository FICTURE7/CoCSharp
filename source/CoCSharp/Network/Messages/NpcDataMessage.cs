using CoCSharp.Logic;
using System;

namespace CoCSharp.Network.Messages
{
    /// <summary>
    /// Message that is sent by the server to the client
    /// to send information about an NPC village.
    /// </summary>
    public class NpcDataMessage : Message
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NpcDataMessage"/> class.
        /// </summary>
        public NpcDataMessage()
        {
            // Space
        }

        /// <summary>
        ///  Gets the ID of the <see cref="NpcDataMessage"/>.
        /// </summary>
        public override ushort ID { get { return 24133; } }

        /// <summary>
        /// Unknown integer 1.
        /// </summary>
        public int Unknown1;

        /// <summary>
        /// NPC village.
        /// </summary>
        public Village NpcVillage;
        /// <summary>
        /// Own avatar data.
        /// </summary>
        public AvatarMessageComponent AvatarData;

        /// <summary>
        /// Unknown integer 2.
        /// </summary>
        public int Unknown2;
        /// <summary>
        /// NPC ID that was attacked.
        /// </summary>
        public int NpcID;

        /// <summary>
        /// Reads the <see cref="NpcDataMessage"/> from the specified <see cref="MessageReader"/>.
        /// </summary>
        /// <param name="reader">
        /// <see cref="MessageReader"/> that will be used to read the <see cref="NpcDataMessage"/>.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="reader"/> is null.</exception>
        public override void ReadMessage(MessageReader reader)
        {
            ThrowIfReaderNull(reader);

            Unknown1 = reader.ReadInt32();

            var villageJson = reader.ReadString();
            NpcVillage = Village.FromJson(villageJson);

            AvatarData = new AvatarMessageComponent();
            AvatarData.ReadMessageComponent(reader);

            Unknown2 = reader.ReadInt32();
            NpcID = reader.ReadInt32();
        }

        /// <summary>
        /// Writes the <see cref="NpcDataMessage"/> to the specified <see cref="MessageWriter"/>.
        /// </summary>
        /// <param name="writer">
        /// <see cref="MessageWriter"/> that will be used to write the <see cref="NpcDataMessage"/>.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="writer"/> is null.</exception>
        /// <exception cref="InvalidOperationException"><see cref="AvatarData"/> is null.</exception>
        public override void WriteMessage(MessageWriter writer)
        {
            ThrowIfWriterNull(writer);

            if (AvatarData == null)
                throw new InvalidOperationException("NpcAvatarData cannot be null.");

            writer.Write(Unknown1);

            var villageJson = NpcVillage.ToJson();
            writer.Write(villageJson);
            AvatarData.WriteMessageComponent(writer);

            writer.Write(Unknown2);
            writer.Write(NpcID);
        }
    }
}
