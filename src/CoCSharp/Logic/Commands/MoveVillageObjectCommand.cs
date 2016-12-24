using CoCSharp.Data;
using CoCSharp.Network;
using System;
using System.Diagnostics;

namespace CoCSharp.Logic.Commands
{
    /// <summary>
    /// Command that is sent by the client to the server to tell
    /// it that an object in the village was moved.
    /// </summary>
    public class MoveVillageObjectCommand : Command
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MoveVillageObjectCommand"/> class.
        /// </summary>
        public MoveVillageObjectCommand()
        {
            // Space
        }

        /// <summary>
        /// Gets the ID of the <see cref="MoveVillageObjectCommand"/>.
        /// </summary>
        public override int Id { get { return 501; } }

        /// <summary>
        /// Data about the move command.
        /// </summary>
        public MoveVillageObjectMessageComponent MoveData;

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
        /// <exception cref="ArgumentNullException"><paramref name="reader"/> is null.</exception>
        public override void ReadCommand(MessageReader reader)
        {
            ThrowIfReaderNull(reader);

            MoveData = new MoveVillageObjectMessageComponent();
            MoveData.X = reader.ReadInt32();
            MoveData.Y = reader.ReadInt32();
            MoveData.VillageObjectGameId = reader.ReadInt32();

            Unknown1 = reader.ReadInt32();
        }

        /// <summary>
        /// Writes the <see cref="MoveVillageObjectCommand"/> to the specified <see cref="MessageWriter"/>.
        /// </summary>
        /// <param name="writer">
        /// <see cref="MessageWriter"/> that will be used to write the <see cref="MoveVillageObjectCommand"/>.
        /// </param>
        /// <exception cref="NullReferenceException">MoveData is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="writer"/> is null.</exception>
        public override void WriteCommand(MessageWriter writer)
        {
            ThrowIfWriterNull(writer);
            if (MoveData == null)
                throw new NullReferenceException("MoveData cannot be null");

            writer.Write(MoveData.X);
            writer.Write(MoveData.Y);
            writer.Write(MoveData.VillageObjectGameId);

            writer.Write(Unknown1);
        }

        /// <summary>
        /// Performs the execution of the <see cref="MoveVillageObjectCommand"/> on the specified <see cref="Avatar"/>.
        /// </summary>
        /// <param name="level"><see cref="Level"/> on which to perform the <see cref="MoveVillageObjectCommand"/>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="level"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="level.Village"/> is null.</exception>
        public override void Execute(Level level)
        {
            ThrowIfLevelNull(level);
            ThrowIfLevelVillageNull(level);

            var village = level.Village;
            var vilobj = village.VillageObjects[MoveData.VillageObjectGameId];
            if (vilobj == null)
            {
                level.Logs.Log($"Could not find village object with game ID {MoveData.VillageObjectGameId}.");
            }
            else
            {
                vilobj.X = MoveData.X;
                vilobj.Y = MoveData.Y;
            }
        }
    }
}
