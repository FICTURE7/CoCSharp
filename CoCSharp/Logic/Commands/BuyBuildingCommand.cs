using CoCSharp.Data.Models;
using CoCSharp.Network;
using System;

namespace CoCSharp.Logic.Commands
{
    /// <summary>
    /// Command that is sent by the client to the server to tell
    /// it that a building was bought.
    /// </summary>
    public class BuyBuildingCommand : Command
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BuyBuildingCommand"/> class.
        /// </summary>
        public BuyBuildingCommand()
        {
            // Space
        }

        /// <summary>
        /// Gets the ID of the <see cref="BuyBuildingCommand"/>.
        /// </summary>
        public override int ID { get { return 500; } }

        /// <summary>
        /// X coordinates of the <see cref="Building"/> where it was placed.
        /// </summary>
        public int X;
        /// <summary>
        /// Y coordinates of the <see cref="Building"/> where it was placed.
        /// </summary>
        public int Y;
        /// <summary>
        /// Data ID of the <see cref="Building"/> that was bought.
        /// </summary>
        public int BuildingDataID;

        /// <summary>
        /// Unknown integer 1.
        /// </summary>
        public int Unknown1;

        /// <summary>
        /// Reads the <see cref="BuyBuildingCommand"/> from the specified <see cref="MessageReader"/>.
        /// </summary>
        /// <param name="reader">
        /// <see cref="MessageReader"/> that will be used to read the <see cref="BuyBuildingCommand"/>.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="reader"/> is null.</exception>
        public override void ReadCommand(MessageReader reader)
        {
            ThrowIfReaderNull(reader);

            X = reader.ReadInt32();
            Y = reader.ReadInt32();
            BuildingDataID = reader.ReadInt32();

            Unknown1 = reader.ReadInt32();
        }

        /// <summary>
        /// Writes the <see cref="BuyBuildingCommand"/> to the specified <see cref="MessageWriter"/>.
        /// </summary>
        /// <param name="writer">
        /// <see cref="MessageWriter"/> that will be used to write the <see cref="BuyBuildingCommand"/>.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="writer"/> is null.</exception>
        public override void WriteCommand(MessageWriter writer)
        {
            ThrowIfWriterNull(writer);

            writer.Write(X);
            writer.Write(Y);
            writer.Write(BuildingDataID);

            writer.Write(Unknown1);
        }

        /// <summary>
        /// Performs the <see cref="BuyBuildingCommand"/> on the specified <see cref="Avatar"/>.
        /// </summary>
        /// <param name="avatar"><see cref="Avatar"/> on which to perform the <see cref="BuyBuildingCommand"/>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="avatar"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="avatar.Home"/> is null.</exception>
        public override void Execute(Avatar avatar)
        {
            ThrowIfAvatarNull(avatar);

            if (avatar.Home == null)
                throw new ArgumentNullException("avatar.Home");

            var data = avatar.Home.AssetManager.SearchCsv<BuildingData>(BuildingDataID, 0);
            var resource = data.BuildResource;
            new Building(avatar.Home, data, X, Y, -1);
        }
    }
}
