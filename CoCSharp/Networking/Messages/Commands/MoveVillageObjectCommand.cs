using CoCSharp.Data;
using CoCSharp.Logic;
using System;

namespace CoCSharp.Networking.Messages.Commands
{
    /// <summary>
    /// Command that is sent by the client to the server to tell
    /// it that an object in the village was moved.
    /// </summary>
    public class MoveVillageObjectCommand : Command
    {
        /// <summary>
        /// Intializes a new instance of the <see cref="MoveVillageObjectCommand"/> class.
        /// </summary>
        public MoveVillageObjectCommand()
        {
            // Space
        }

        /// <summary>
        /// Gets the ID of the <see cref="MoveVillageObjectCommand"/>.
        /// </summary>
        public override int ID { get { return 501; } }

        /// <summary>
        /// Data about the move command.
        /// </summary>
        public MoveVillageObjectData MoveData;

        /// <summary>
        /// Unknown integer 1.
        /// </summary>
        public int Unknown1;

        /// <summary>
        /// Reads the <see cref="MoveVillageObjectCommand"/> from the specified <see cref="MessageReader"/>.
        /// </summary>
        /// <param name="reader">
        /// <see cref="MessageReader"/> that will be used to read the <see cref="MoveVillageObjectCommand"/>.
        /// </param>
        public override void ReadCommand(MessageReader reader)
        {
            MoveData = new MoveVillageObjectData();
            MoveData.X = reader.ReadInt32();
            MoveData.Y = reader.ReadInt32();
            MoveData.VillageObjectGameIndex = reader.ReadInt32();

            Unknown1 = reader.ReadInt32();
        }

        /// <summary>
        /// Writes the <see cref="MoveVillageObjectCommand"/> to the specified <see cref="MessageWriter"/>.
        /// </summary>
        /// <param name="writer">
        /// <see cref="MessageWriter"/> that will be used to write the <see cref="MoveVillageObjectCommand"/>.
        /// </param>
        /// <exception cref="NullReferenceException">MoveData is null.</exception>
        public override void WriteCommand(MessageWriter writer)
        {
            if (MoveData == null)
                throw new NullReferenceException("MoveData cannot be null");

            writer.Write(MoveData.X);
            writer.Write(MoveData.Y);
            writer.Write(MoveData.VillageObjectGameIndex);

            writer.Write(Unknown1);
        }
    }
}
