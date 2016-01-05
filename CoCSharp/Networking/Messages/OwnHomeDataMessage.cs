using CoCSharp.Data;
using System;

namespace CoCSharp.Networking.Messages
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
        /// Time sinced last visited.
        /// </summary>
        public TimeSpan LastVisit;

        /// <summary>
        /// Unknown integer 1.
        /// </summary>
        public int Unknown1; // -1

        /// <summary>
        /// Server local timestamp.
        /// </summary>
        public DateTime Timestamp;
        /// <summary>
        /// Own avatar data.
        /// </summary>
        public AvatarData OwnAvatarData;

        /// <summary>
        /// Reads the <see cref="OwnHomeDataMessage"/> from the specified <see cref="MessageReader"/>.
        /// </summary>
        /// <param name="reader">
        /// <see cref="MessageReader"/> that will be used to read the <see cref="OwnHomeDataMessage"/>.
        /// </param>
        public override void ReadMessage(MessageReader reader)
        {
            LastVisit = TimeSpan.FromSeconds(reader.ReadInt32());
            Unknown1 = reader.ReadInt32(); // -1
            Timestamp = DateTimeConverter.FromUnixTimestamp(reader.ReadInt32());

            OwnAvatarData = new AvatarData();
            OwnAvatarData.Read(reader);
        }

        /// <summary>
        /// Writes the <see cref="OwnHomeDataMessage"/> to the specified <see cref="MessageWriter"/>.
        /// </summary>
        /// <param name="writer">
        /// <see cref="MessageWriter"/> that will be used to write the <see cref="OwnHomeDataMessage"/>.
        /// </param>
        public override void WriteMessage(MessageWriter writer)
        {
            if (OwnAvatarData == null) // quit early just not to mess up the stream
                throw new NullReferenceException("OwnAvatarData was null.");

            writer.Write((int)LastVisit.TotalSeconds);
            writer.Write(Unknown1); // -1
            writer.Write((int)DateTimeConverter.ToUnixTimestamp(Timestamp));

            OwnAvatarData.Write(writer);
        }
    }
}
