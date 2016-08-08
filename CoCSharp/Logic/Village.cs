using CoCSharp.Data;
using CoCSharp.Data.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace CoCSharp.Logic
{
    /// <summary>
    /// Represents a Clash of Clans village.
    /// </summary>
    public class Village : IDisposable
    {
        //TODO: Implement VillageObjectCollection class.

        #region Constants
        /// <summary>
        /// Represents the width of a <see cref="Village"/> layout.
        /// </summary>
        public const int Width = 48;

        /// <summary>
        /// Represents the height of a <see cref="Village"/> layout.
        /// </summary>
        public const int Height = 48;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="Village"/> class and uses <see cref="AssetManager.DefaultInstance"/>.
        /// </summary>
        /// <exception cref="ArgumentNullException"><see cref="AssetManager.DefaultInstance"/> is null.</exception>
        public Village() : this(AssetManager.DefaultInstance)
        {
            // Space
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Village"/> class with the specified
        /// <see cref="Data.AssetManager"/>.
        /// </summary>
        /// <param name="manager"><see cref="Data.AssetManager"/> to use.</param>
        /// <exception cref="ArgumentNullException"><paramref name="manager"/> is null.</exception>
        public Village(AssetManager manager)
        {
            if (manager == null)
                throw new ArgumentNullException("manager");

            AssetManager = manager;
            _villageObjects = new VillageObjectCollection();
        }
        #endregion

        #region Fields & Properties
        private AssetManager _assetManager;
        /// <summary>
        /// Gets or sets the <see cref="AssetManager"/> from which data will be
        /// used.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is null.</exception>
        public AssetManager AssetManager
        {
            get
            {
                return _assetManager;
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");

                _assetManager = value;
            }
        }

        /// <summary>
        /// Gets or sets the experience version? (Not completely sure if thats its full name).
        /// </summary>
        /// <remarks>
        /// I don't know what this is needed for but I found it in the 8.x.x update
        /// and the client needs it when there is a "loot_multiplier_ver" in an Obstacle object; it crashes
        /// if it does not find it.
        /// </remarks>
        public int ExperienceVersion { get; set; }

        private readonly VillageObjectCollection _villageObjects;
        /// <summary>
        /// Gets the <see cref="VillageObjectCollection"/> which contains all the <see cref="VillageObject"/> in
        /// this <see cref="Village"/>.
        /// </summary>
        public VillageObjectCollection VillageObjects
        {
            get
            {
                return _villageObjects;
            }
        }

        /// <summary>
        /// Gets an enumerator that iterates through the <see cref="Building"/> objects in the <see cref="Village"/>.
        /// </summary>
        public IEnumerable<Building> Buildings
        {
            get
            {
                return _villageObjects.GetRow(Building.Kind).Select(k => (Building)k);
            }
        }

        /// <summary>
        /// Gets an enumerator that iterates through the <see cref="Obstacle"/> objects in the <see cref="Village"/>.
        /// </summary>
        public IEnumerable<Obstacle> Obstacles
        {
            get
            {
                return _villageObjects.GetRow(Obstacle.Kind).Select(k => (Obstacle)k);
            }
        }

        /// <summary>
        /// Gets an enumerator that iterates through <see cref="Trap"/> objects in the <see cref="Village"/>.
        /// </summary>
        public IEnumerable<Trap> Traps
        {
            get
            {
                return _villageObjects.GetRow(Trap.Kind).Select(k => (Trap)k);
            }
        }

        /// <summary>
        /// Gets an enumerator that iterates through the <see cref="Decoration"/> objects in the <see cref="Village"/>.
        /// </summary>
        public IEnumerable<Decoration> Decorations
        {
            get
            {
                return _villageObjects.GetRow(Decoration.Kind).Select(k => (Decoration)k);
            }
        }

        private Building _townhall;
        /// <summary>
        /// Gets or sets the TownHall <see cref="Building"/> of the <see cref="Village"/>; returns
        /// <c>null</c> if there is no TownHall in the <see cref="Village"/>.
        /// </summary>
        public Building TownHall
        {
            get
            {
                return _townhall;
            }
            internal set
            {
                // Prevent the Village from reacting to the old TownHall upgrades.
                if (_townhall != null)
                    _townhall.ConstructionFinished -= OnTownHallConstructionFinished;

                _townhall = value;
                _townhall.ConstructionFinished += OnTownHallConstructionFinished;
            }
        }
        #endregion

        #region Methods
        private bool _disposed;
        /// <summary>
        /// Releases all resources used by this <see cref="Village"/> instance.
        /// </summary>
        /// <remarks>
        /// All the <see cref="VillageObject"/> in the <see cref="Village"/> will be pushed
        /// to an internal pool, <see cref="VillageObjectPool"/>.
        /// </remarks>
        public void Dispose()
        {
            Dispose(true);
        }

        /// <summary>
        /// Releases all resources used by this <see cref="Village"/> instance.
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                foreach (var building in Buildings)
                    building.PushToPool();
                foreach (var obstacle in Obstacles)
                    obstacle.PushToPool();
                foreach (var trap in Traps)
                    trap.PushToPool();
                foreach (var deco in Decorations)
                    deco.PushToPool();
            }

            _disposed = true;
        }

        /// <summary>
        /// Returns a non-indented JSON string that represents the current <see cref="Village"/>.
        /// </summary>
        /// <returns>A non-indented JSON string that represents the current <see cref="Village"/>.</returns>
        public string ToJson()
        {
            return ToJson(false);
        }

        /// <summary>
        /// Returns a JSON string and indented if specified that represents the current <see cref="Village"/>.
        /// </summary>
        /// <param name="indent">If set to <c>true</c> the returned JSON string will be indented.</param>
        /// <returns>A JSON string and indented if specified that represents the current <see cref="Village"/>.</returns>
        public string ToJson(bool indent)
        {
            var jsonStr = string.Empty;

            var textWriter = new StringWriter();
            using (var jsonWriter = new JsonTextWriter(textWriter))
            {
                if (indent)
                {
                    jsonWriter.Formatting = Formatting.Indented;
                    jsonWriter.Indentation = 4;
                }

                jsonWriter.WriteStartObject();

                jsonWriter.WritePropertyName("exp_ver");
                jsonWriter.WriteValue(ExperienceVersion);

                jsonWriter.WritePropertyName("buildings");
                WriteBuildingArray(jsonWriter);

                jsonWriter.WritePropertyName("obstacles");
                WriteObstacleArray(jsonWriter);

                jsonWriter.WritePropertyName("traps");
                WriteTrapArray(jsonWriter);

                jsonWriter.WritePropertyName("decos");
                WriteDecorationArray(jsonWriter);

                jsonWriter.WriteEndObject();

                jsonStr = textWriter.ToString();
            }

            return jsonStr;
        }

        /// <summary>
        /// Returns a <see cref="Village"/> that will be deserialize from the specified
        /// JSON string with the default <see cref="Data.AssetManager"/> instance.
        /// </summary>
        /// <param name="value">JSON string that represents the <see cref="Village"/>.</param>
        /// <returns>A <see cref="Village"/> that is deserialized from the specified JSON string.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is null or whitespace.</exception>
        /// <exception cref="InvalidOperationException"><see cref="AssetManager.DefaultInstance"/> is null.</exception>
        public static Village FromJson(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentNullException("value");

            if (AssetManager.DefaultInstance == null)
                throw new InvalidOperationException("DefaultInstance of AssetManager cannot be null.");

            return FromJson(value, AssetManager.DefaultInstance);
        }

        /// <summary>
        /// Returns a <see cref="Village"/> that will be deserialize from the specified
        /// JSON string with the specified <see cref="Data.AssetManager"/> instance.
        /// </summary>
        /// <param name="value">JSON string that represents the <see cref="Village"/>.</param>
        /// <param name="manager"><see cref="Data.AssetManager"/> from which data of <see cref="VillageObject"/> in the <see cref="Village"/> will be populated.</param>
        /// <returns>A <see cref="Village"/> that is deserialized from the specified JSON string.</returns>
        public static Village FromJson(string value, AssetManager manager)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentNullException("value");
            if (manager == null)
                throw new ArgumentNullException("manager");

            // Make sure the AssetManager provided has loaded all the required CsvData.
            if (!manager.IsCsvLoaded<BuildingData>())
                throw new ArgumentException("manager did not load CsvData of type '" + typeof(BuildingData) + "'.", "manager");
            if (!manager.IsCsvLoaded<ObstacleData>())
                throw new ArgumentException("manager did not load CsvData of type '" + typeof(ObstacleData) + "'.", "manager");
            if (!manager.IsCsvLoaded<TrapData>())
                throw new ArgumentException("manager did not load CsvData of type '" + typeof(TrapData) + "'.", "manager");
            if (!manager.IsCsvLoaded<DecorationData>())
                throw new ArgumentException("manager did not load CsvData of type '" + typeof(DecorationData) + "'.", "manager");

            var village = new Village(manager);

            var textReader = new StringReader(value);
            using (var jsonReader = new JsonTextReader(textReader))
            {
                while (jsonReader.Read())
                {
                    if (jsonReader.TokenType == JsonToken.PropertyName)
                    {
                        var propertyName = (string)jsonReader.Value;
                        switch (propertyName)
                        {
                            case "exp_ver":
                                village.ExperienceVersion = jsonReader.ReadAsInt32().Value;
                                break;

                            case "buildings":
                                ReadBuildingArray(jsonReader, village);
                                break;

                            case "obstacles":
                                ReadObstacleArray(jsonReader, village);
                                break;

                            case "traps":
                                ReadTrapArray(jsonReader, village);
                                break;

                            case "decos":
                                ReadDecorationArray(jsonReader, village);
                                break;
                        }
                    }
                }
            }

            return village;
        }

        #region Json Writing
        private void WriteBuildingArray(JsonWriter writer)
        {
            writer.WriteStartArray();
            foreach (var building in Buildings)
                building.ToJsonWriter(writer);

            writer.WriteEndArray();
        }

        private void WriteObstacleArray(JsonWriter writer)
        {
            writer.WriteStartArray();
            foreach (var obstacle in Obstacles)
                obstacle.ToJsonWriter(writer);

            writer.WriteEndArray();
        }

        private void WriteTrapArray(JsonWriter writer)
        {
            writer.WriteStartArray();
            foreach (var trap in Traps)
                trap.ToJsonWriter(writer);

            writer.WriteEndArray();
        }

        private void WriteDecorationArray(JsonWriter writer)
        {
            writer.WriteStartArray();
            foreach (var deco in Decorations)
                deco.ToJsonWriter(writer);

            writer.WriteEndArray();
        }
        #endregion

        #region Json Reading
        private static void ReadBuildingArray(JsonReader reader, Village village)
        {
            // Make sure we are in an array first.
            if (!reader.Read() || reader.TokenType != JsonToken.StartArray)
                throw new JsonException("Expected a JSON start array.");

            // List of buildings whose CanUpgrade value must be updated.
            // This list gets populated when the townhall buildings is lower than
            // than the buildings in this in the JSON document.
            var list = new List<Building>();

            var building = (Building)null;
            while (reader.Read())
            {
                // Break when we read the end of the array.
                if (reader.TokenType == JsonToken.EndArray)
                    break;

                if (reader.TokenType == JsonToken.StartObject)
                {
                    building = Building.GetInstance(village);
                    building.FromJsonReader(reader);

                    // If we do not have the townhall building yet to figure if we can upgrade or not
                    // we add the building to the list of buildings whose CanUpgrade value will be
                    // updated at the end of the array read.
                    if (village.TownHall == null)
                    {
                        list.Add(building);
                    }
                    else
                    {
                        building.UpdateCanUpgade();
                    }

                    building = null;
                }
            }

            if (village.TownHall == null)
                throw new InvalidOperationException("Village does not contain a Town Hall building.");

            //NOTE: Potential InvalidOperationException when the Trap array is serialized before the Building array.
            //TODO: Read Building array before any VillageObject array to avoid the above NOTE.

            for (int i = 0; i < list.Count; i++)
                list[i].UpdateCanUpgade();
        }

        private static void ReadObstacleArray(JsonReader reader, Village village)
        {
            // Make sure we are in an array first.
            if (!reader.Read() || reader.TokenType != JsonToken.StartArray)
                throw new JsonException("Expected a JSON start array.");

            var obstacle = (Obstacle)null;
            while (reader.Read())
            {
                // Break when we read the end of the array.
                if (reader.TokenType == JsonToken.EndArray)
                    break;

                if (reader.TokenType == JsonToken.StartObject)
                {
                    obstacle = Obstacle.GetInstance(village);
                    obstacle.FromJsonReader(reader);

                    obstacle = null;
                }
            }
        }

        private static void ReadTrapArray(JsonReader reader, Village village)
        {
            // Make sure we are in an array first.
            if (!reader.Read() || reader.TokenType != JsonToken.StartArray)
                throw new JsonException("Expected a JSON start array.");

            // Refer to ReadBuildingArray.
            var list = new List<Trap>();

            var trap = (Trap)null;
            while (reader.Read())
            {
                // Break when we read the end of the array.
                if (reader.TokenType == JsonToken.EndArray)
                    break;

                if (reader.TokenType == JsonToken.StartObject)
                {
                    trap = Trap.GetInstance(village);
                    trap.FromJsonReader(reader);

                    // Refer to ReadBuildingArray.
                    if (village.TownHall == null)
                    {
                        list.Add(trap);
                    }
                    else
                    {
                        trap.UpdateCanUpgade();
                    }

                    trap = null;
                }
            }

            for (int i = 0; i < list.Count; i++)
                list[i].UpdateCanUpgade();
        }

        private static void ReadDecorationArray(JsonReader reader, Village village)
        {
            // Make sure we are in an array first.
            if (!reader.Read() || reader.TokenType != JsonToken.StartArray)
                throw new JsonException("Expected a JSON start array.");

            var decoration = (Decoration)null;
            while (reader.Read())
            {
                // Break when we read the end of the array.
                if (reader.TokenType == JsonToken.EndArray)
                    break;

                if (reader.TokenType == JsonToken.StartObject)
                {
                    decoration = Decoration.GetInstance(village);
                    decoration.FromJsonReader(reader);

                    decoration = null;
                }
            }
        }
        #endregion

        // Called when ever the TownHall has been upgraded.
        // To update the CanUpgrade flags of Buildings and Traps.
        private void OnTownHallConstructionFinished(object sender, ConstructionFinishedEventArgs<BuildingData> e)
        {
            if (e.WasCancelled)
                return;

            // Update our Buildable's CanUpgrade property.
            //for (int i = 0; i < Buildings.Count; i++)
            //    Buildings[i].UpdateCanUpgade();
            //for (int i = 0; i < Traps.Count; i++)
            //    Traps[i].UpdateCanUpgade();

            Debug.WriteLine("Town Hall construction finished. Updated buildings & traps CanUpgrade property.");
        }
        #endregion
    }
}
