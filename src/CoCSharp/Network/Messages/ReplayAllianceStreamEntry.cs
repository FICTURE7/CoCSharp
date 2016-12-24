using System;

namespace CoCSharp.Network.Messages
{
    /// <summary>
    /// Stream entry indicating a reply was sent to the alliance chat.
    /// </summary>
    public class ReplayAllianceStreamEntry : AllianceStreamEntry
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChatAllianceStreamEntry"/> class.
        /// </summary>
        public ReplayAllianceStreamEntry()
        {
            //space
        }

        /// <summary>
        /// Unknown integer 3.
        /// </summary>
        public int Unknown3;
        /// <summary>
        /// Unknown integer 4.
        /// </summary>
        public int Unknown4;
        /// <summary>
        /// Unknown integer 5.
        /// </summary>
        public int Unknown5;
        /// <summary>
        /// Unknown byte 6.
        /// </summary>
        public byte Unknown6;
        /// <summary>
        /// Message that is sent along with the replay.
        /// </summary>
        public string TextMessage;
        /// <summary>
        /// Enemy name.
        /// </summary>
        public string EnemyName;
        /// <summary>
        /// Json that describes the replay.
        /// </summary>
        public string ReplayJson;
        /// <summary>
        /// Unknown integer 7.
        /// </summary>
        public int Unknown7;
        /// <summary>
        /// Unknown integer 8.
        /// </summary>
        public int Unknown8;
        /// <summary>
        /// Unknown integer 9.
        /// </summary>
        public int Unknown9;

        /// <summary>
        /// Get the ID of the <see cref="ReplayAllianceStreamEntry"/>
        /// </summary>
        public override int Id => 5;

        /// <summary>
        /// Reads the <see cref="ReplayAllianceStreamEntry"/> from the specified <see cref="MessageReader"/>.
        /// </summary>
        /// <param name="reader">
        /// <see cref="MessageReader"/> that will be used to read the <see cref="ReplayAllianceStreamEntry"/>.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="reader"/> is null.</exception>
        public override void ReadStreamEntry(MessageReader reader)
        {
            base.ReadStreamEntry(reader);

            Unknown3 = reader.ReadInt32();
            Unknown4 = reader.ReadInt32();
            Unknown5 = reader.ReadInt32();
            Unknown6 = reader.ReadByte();

            TextMessage = reader.ReadString();
            EnemyName = reader.ReadString();
            ReplayJson = reader.ReadString();

            Unknown7 = reader.ReadInt32();
            Unknown8 = reader.ReadInt32();
            Unknown9 = reader.ReadInt32();
        }

        /// <summary>
        /// Writes the <see cref="ReplayAllianceStreamEntry"/> to the specified <see cref="MessageWriter"/>.
        /// </summary>
        /// <param name="writer">
        /// <see cref="MessageWriter"/> that will be used to write the <see cref="ReplayAllianceStreamEntry"/>.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="writer"/> is null.</exception>
        public override void WriteStreamEntry(MessageWriter writer)
        {
            base.WriteStreamEntry(writer);

            writer.Write(Unknown3);
            writer.Write(Unknown4);
            writer.Write(Unknown5);
            writer.Write(Unknown6);

            writer.Write(TextMessage);
            writer.Write(EnemyName);
            writer.Write(ReplayJson);

            writer.Write(Unknown7);
            writer.Write(Unknown8);
            writer.Write(Unknown9);
        }
    }
}
