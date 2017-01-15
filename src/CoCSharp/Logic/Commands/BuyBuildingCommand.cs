using CoCSharp.Csv;
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
        public override int Id { get { return 500; } }

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

            Tick = reader.ReadInt32();
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

            writer.Write(Tick);
        }

        /// <summary>
        /// Performs the execution of the <see cref="BuyBuildingCommand"/> on the specified <see cref="Avatar"/>.
        /// </summary>
        /// <param name="level"><see cref="Level"/> on which to perform the <see cref="BuyBuildingCommand"/>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="level"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="level.Village"/> is null.</exception>
        public override void Execute(Level level)
        {
            ThrowIfLevelNull(level);
            ThrowIfLevelVillageNull(level);

            var dataRef = new CsvDataRowRef<BuildingData>(BuildingDataID);
            var assets = level.Assets;
            var tableCollection = assets.DataTables;
            var row = dataRef.Get(tableCollection);
            if (row == null)
            {
                level.Logs.Log($"Unable to find CsvDataRow<BuildingData> for data ID {BuildingDataID}.");
            }
            else
            {

                // Use the first level.
                var data = row[0];
                if (data == null)
                {
                    level.Logs.Log($"Unable to find BuildingData of level 0 for data ID {BuildingDataID}.");
                }
                else
                {
                    var globalsTable = assets.DataTables.GetTable<GlobalData>();
                    var resource = data.BuildResource;
                    var cost = data.BuildCost;

                    var workerManager = level.Village.WorkerManager;
                    if (data.Name == "Worker Building" && workerManager.TotalWorkers > 0)
                    {
                        var wgloRow = (CsvDataRow<GlobalData>)null;
                        if (workerManager.TotalWorkers == 1)
                        {
                            wgloRow = globalsTable.Rows["WORKER_COST_2ND"];
                        }
                        else if (workerManager.TotalWorkers == 2)
                        {
                            wgloRow = globalsTable.Rows["WORKER_COST_3RD"];
                        }
                        else if (workerManager.TotalWorkers == 3)
                        {
                            wgloRow = globalsTable.Rows["WORKER_COST_4TH"];
                        }
                        else if (workerManager.TotalWorkers == 4)
                        {
                            wgloRow = globalsTable.Rows["WORKER_COST_5TH"];
                        }

                        if (wgloRow == null)
                        {
                            level.Logs.Log("Unable to find correct GlobalData for worker cost.");
                        }
                        else
                        {
                            cost = wgloRow[0].NumberValue;
                        }
                    }

                    level.Avatar.UseResource(resource, cost);

                    var building = new Building(level.Village, data);
                    building.X = X;
                    building.Y = Y;
                    building.BeginConstruction(Tick);
                }
            }
        }
    }
}
