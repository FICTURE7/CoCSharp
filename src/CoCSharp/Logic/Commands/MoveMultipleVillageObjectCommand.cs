﻿using CoCSharp.Data;
using CoCSharp.Network;
using System;
using System.Diagnostics;

namespace CoCSharp.Logic.Commands
{
    /// <summary>
    /// Command that is sent by the client to the server to tell
    /// it that multiple object in the village was moved. This is mostly
    /// for walls.
    /// </summary>
    public class MoveMultipleVillageObjectCommand : Command
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MoveMultipleVillageObjectCommand"/> class.
        /// </summary>
        public MoveMultipleVillageObjectCommand()
        {
            // Space
        }

        /// <summary>
        /// Gets the ID of the <see cref="MoveMultipleVillageObjectCommand"/>.
        /// </summary>
        public override int Id { get { return 533; } }

        /// <summary>
        /// Data about the moves command.
        /// </summary>
        public MoveVillageObjectMessageComponent[] MovesData;

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

            var count = reader.ReadInt32();
            if (count < 0)
                throw new InvalidCommandException("Length of MovesData cannot be less than 0.", this);

            MovesData = new MoveVillageObjectMessageComponent[count];
            for (int i = 0; i < count; i++)
            {
                var data = new MoveVillageObjectMessageComponent();
                data.X = reader.ReadInt32();
                data.Y = reader.ReadInt32();
                data.VillageObjectGameId = reader.ReadInt32();

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
        /// <exception cref="InvalidOperationException"><see cref="MovesData"/> is null.</exception>
        /// /// <exception cref="InvalidOperationException">An element in <see cref="MovesData"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="writer"/> is null.</exception>
        public override void WriteCommand(MessageWriter writer)
        {
            ThrowIfWriterNull(writer);

            if (MovesData == null)
                throw new InvalidOperationException("MovesData cannot be null");

            for (int i = 0; i < MovesData.Length; i++)
            {
                if (MovesData[i] == null)
                    throw new InvalidOperationException("MovesData at index '" + i + "' was null.");

                writer.Write(MovesData[i].X);
                writer.Write(MovesData[i].Y);
                writer.Write(MovesData[i].VillageObjectGameId);
            }

            writer.Write(Unknown1);
        }

        /// <summary>
        /// Performs the execution of the <see cref="MoveMultipleVillageObjectCommand"/> on the specified <see cref="Avatar"/>.
        /// </summary>
        /// <param name="level"><see cref="Level"/> on which to perform the <see cref="MoveMultipleVillageObjectCommand"/>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="level"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="level.Village"/> is null.</exception>
        public override void Execute(Level level)
        {
            ThrowIfLevelNull(level);
            ThrowIfLevelVillageNull(level);

            var village = level.Village;
            for (int i = 0; i < MovesData.Length; i++)
            {
                var moveData = MovesData[i];
                var vilobj = village.VillageObjects[moveData.VillageObjectGameId];
                if (vilobj == null)
                {
                    level.Logs.Log($"Could not find village object with game ID {moveData.VillageObjectGameId}.");
                }
                else
                {
                    vilobj.X = moveData.X;
                    vilobj.Y = moveData.Y;
                }
            }
        }
    }
}
