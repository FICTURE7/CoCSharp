using CoCSharp.Data;
using System;

namespace CoCSharp.Networking.Messages.Commands
{
    /// <summary>
    /// Command that is sent by the client to the server to tell
    /// it that multiple object in the village was moved. This is mostly
    /// for walls.
    /// </summary>
    public class MoveMultipleVillageObjectCommand : Command
    {
        /// <summary>
        /// Intializes a new instance of the <see cref="MoveMultipleVillageObjectCommand"/> class.
        /// </summary>
        public MoveMultipleVillageObjectCommand()
        {
            // Space
        }

        /// <summary>
        /// Gets the ID of the <see cref="MoveMultipleVillageObjectCommand"/>.
        /// </summary>
        public override int ID { get { return 533; } }

        /// <summary>
        /// Data about the moves command.
        /// </summary>
        public MoveVillageObjectData[] MovesData;

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
            var count = reader.ReadInt32();
            if (count < 0)
                throw new InvalidCommandException("Number of MovesData cannot be less than 0.", this);
            MovesData = new MoveVillageObjectData[count];
            for (int i = 0; i < count; i++)
            {
                var data = new MoveVillageObjectData();
                data.X = reader.ReadInt32();
                data.Y = reader.ReadInt32();
                data.VillageObjectGameIndex = reader.ReadInt32();

                MovesData[i] = data;
            }

            Unknown1 = reader.ReadInt32();
        }

        /// <summary>
        /// Writes the <see cref="MoveVillageObjectCommand"/> to the specified <see cref="MessageWriter"/>.
        /// </summary>
        /// <param name="writer">
        /// <see cref="MessageWriter"/> that will be used to write the <see cref="MoveVillageObjectCommand"/>.
        /// </param>
        /// <exception cref="NullReferenceException">MovesData is null.</exception>
        public override void WriteCommand(MessageWriter writer)
        {
            if (MovesData == null)
                throw new NullReferenceException("MovesData cannot be null");

            for (int i = 0; i < MovesData.Length; i++)
            {
                writer.Write(MovesData[i].X);
                writer.Write(MovesData[i].Y);
                writer.Write(MovesData[i].VillageObjectGameIndex);
            }

            writer.Write(Unknown1);
        }
    }
}
