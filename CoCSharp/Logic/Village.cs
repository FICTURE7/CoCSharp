using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace CoCSharp.Logic
{
    /// <summary>
    /// Represents a Clash of Clans village.
    /// </summary>
    public class Village
    {
        /// <summary>
        /// Represents the width of the <see cref="Village"/> layout.
        /// </summary>
        public const int Width = 44;

        /// <summary>
        /// Represents the height of the <see cref="Village"/> layout.
        /// </summary>
        public const int Height = 44;

        /// <summary>
        /// Initializes a new instance of the <see cref="Village"/> class.
        /// </summary>
        public Village()
        {
            Buildings = new List<Building>();
            Obstacles = new List<Obstacle>();
            Traps = new List<Trap>();
            Decorations = new List<Decoration>();
        }

        /// <summary>
        /// Gets or sets the experience version.
        /// </summary>
        /// <remarks>
        /// We don't know what this is needed for but I found it in the 8.x.x update
        /// and the client needs it when there is a "loot_multiplier_ver" in an Obstacle object; it crashes
        /// if it does not find it.
        /// </remarks>
        [JsonProperty("exp_ver")]
        public int ExperienceVersion { get; set; }

        /// <summary>
        /// Gets or sets the list of <see cref="Building"/> in the <see cref="Village"/>.
        /// </summary>
        [JsonProperty("buildings")]
        public List<Building> Buildings { get; set; }

        /// <summary>
        /// Gets or sets the list of <see cref="Obstacle"/> in the <see cref="Village"/>.
        /// </summary>
        [JsonProperty("obstacles")]
        public List<Obstacle> Obstacles { get; set; }

        /// <summary>
        /// Gets or sets the list of <see cref="Trap"/> in the <see cref="Village"/>.
        /// </summary>
        [JsonProperty("traps")]
        public List<Trap> Traps { get; set; }

        /// <summary>
        /// Gets or sets the list of <see cref="Decoration"/> in the <see cref="Village"/>.
        /// </summary>
        [JsonProperty("decos")]
        public List<Decoration> Decorations { get; set; }

        /// <summary>
        /// Gets the JSON from which the <see cref="Village"/> was
        /// deserialized; returns <c>null</c> if the <see cref="Village"/> wasn't deserialized.
        /// </summary>
        [JsonIgnore]
        public string DeserializedJson { get; private set; }

        #region Potential Private Methods

        // These methods becomes pointless to be public be cause
        // of the GetVillageObject<T>(gameId) method.

        /// <summary>
        /// Gets the <see cref="Building"/> with the specified game ID.
        /// </summary>
        /// <param name="gameId"><see cref="Building"/> with the game ID.</param>
        /// <returns><see cref="Building"/> with the specified game ID.</returns>
        /// <exception cref="ArgumentException"><paramref name="gameId"/> is not a valid <see cref="Building"/> game ID.</exception>
        /// <exception cref="ArgumentException"><paramref name="gameId"/> is not in the <see cref="Village"/>.</exception>
        public Building GetBuilding(int gameId)
        {
            if (gameId < Building.BaseGameID || gameId > Building.BaseGameID + VillageObject.Base)
                throw new ArgumentException("'" + gameId + "' is not a valid Building game ID. It must be between '" + Building.BaseGameID +
                                            "' and '" + (Building.BaseGameID + VillageObject.Base) + "'.");

            var index = gameId - Building.BaseGameID;
            // Check if the index is valid.
            // Index should be never less than 0 as it was checked above.
            if (index >= Buildings.Count)
                throw new ArgumentException("Could not get Building with game ID '" + gameId + "'.", "gameId");

            return Buildings[index];
        }

        /// <summary>
        /// Gets the <see cref="Obstacle"/> with the specified game ID.
        /// </summary>
        /// <param name="gameId"><see cref="Obstacle"/> with the game ID.</param>
        /// <returns><see cref="Obstacle"/> with the specified game ID.</returns>
        /// <exception cref="ArgumentException"><paramref name="gameId"/> is not a valid <see cref="Obstacle"/> game ID.</exception>
        /// <exception cref="ArgumentException"><paramref name="gameId"/> is not in the <see cref="Village"/>.</exception>
        public Obstacle GetObstacle(int gameId)
        {
            if (gameId < Obstacle.BaseGameID || gameId > Obstacle.BaseGameID + VillageObject.Base)
                throw new ArgumentException("'" + gameId + "' is not a valid Obstacle game ID. It must be between '" + Obstacle.BaseGameID +
                                            "' and '" + (Obstacle.BaseGameID + VillageObject.Base) + "'.");

            var index = gameId - Obstacle.BaseGameID;
            // Check if the index is valid.
            // Index should be never less than 0 as it was checked above.
            if (index >= Obstacles.Count)
                throw new ArgumentException("Could not get Obstacle with game ID '" + gameId + "'.", "gameId");

            return Obstacles[index];
        }

        /// <summary>
        /// Gets the <see cref="Trap"/> with the specified game ID.
        /// </summary>
        /// <param name="gameId"><see cref="Trap"/> with the game ID.</param>
        /// <returns><see cref="Trap"/> with the specified game ID.</returns>
        /// <exception cref="ArgumentException"><paramref name="gameId"/> is not a valid <see cref="Trap"/> game ID.</exception>
        /// <exception cref="ArgumentException"><paramref name="gameId"/> is not in the <see cref="Village"/>.</exception>
        public Trap GetTrap(int gameId)
        {
            if (gameId < Trap.BaseGameID || gameId > Trap.BaseGameID + VillageObject.Base)
                throw new ArgumentException("'" + gameId + "' is not a valid Trap game ID. It must be between '" + Trap.BaseGameID +
                                            "' and '" + (Trap.BaseGameID + VillageObject.Base) + "'.");

            var index = gameId - Trap.BaseGameID;
            // Check if the index is valid.
            // Index should be never less than 0 as it was checked above.
            if (index >= Traps.Count)
                throw new ArgumentException("Could not get Trap with game ID '" + gameId + "'.", "gameId");

            return Traps[index];
        }

        /// <summary>
        /// Gets the <see cref="Decoration"/> with the specified game ID.
        /// </summary>
        /// <param name="gameId"><see cref="Decoration"/> with the game ID.</param>
        /// <returns><see cref="Decoration"/> with the specified game ID.</returns>
        /// <exception cref="ArgumentException"><paramref name="gameId"/> is not a valid <see cref="Decoration"/> game ID.</exception>
        /// <exception cref="ArgumentException"><paramref name="gameId"/> is not in the <see cref="Village"/>.</exception>
        public Decoration GetDecoration(int gameId)
        {
            if (gameId < Decoration.BaseGameID || gameId > Decoration.BaseGameID + VillageObject.Base)
                throw new ArgumentException("'" + gameId + "' is not a valid Decoration game ID. It must be between '" + Decoration.BaseGameID +
                                            "' and '" + (Decoration.BaseGameID + VillageObject.Base) + "'.");

            var index = gameId - Decoration.BaseGameID;
            // Check if the index is valid.
            // Index should be never less than 0 as it was checked above.
            if (index >= Decorations.Count)
                throw new ArgumentException("Could not get Decoration with game ID '" + gameId + "'.", "gameId");

            return Decorations[index];
        }

        #endregion

        /// <summary>
        /// Gets the <see cref="VillageObject"/> with specified game ID as 
        /// the specified <see cref="VillageObject"/> type.
        /// </summary>
        /// <typeparam name="T">Type of the <see cref="VillageObject"/> to return.</typeparam>
        /// <param name="gameId"><see cref="VillageObject"/> with the game ID.</param>
        /// <returns><see cref="VillageObject"/> with the specified game ID.</returns>
        public T GetVillageObject<T>(int gameId) where T : VillageObject
        {
            return (T)GetVillageObject(gameId);
        }

        /// <summary>
        /// Gets the <see cref="VillageObject"/> with the specified game ID.
        /// </summary>
        /// <param name="gameId"><see cref="VillageObject"/> with the game ID.</param>
        /// <returns><see cref="VillageObject"/> with the specified game ID.</returns>
        /// <exception cref="ArgumentException"><paramref name="gameId"/> is not in the village.</exception>
        public VillageObject GetVillageObject(int gameId)
        {
            // Code repetition here with double checking of gameIds.
            if (gameId > Building.BaseGameID || gameId < Building.BaseGameID + VillageObject.Base)
                return GetBuilding(gameId);
            else if (gameId > Obstacle.BaseGameID || gameId < Obstacle.BaseGameID + VillageObject.Base)
                return GetObstacle(gameId);
            else if (gameId > Trap.BaseGameID || gameId < Trap.BaseGameID + VillageObject.Base)
                return GetTrap(gameId);
            else if (gameId > Decoration.BaseGameID || gameId < Decoration.BaseGameID + VillageObject.Base)
                return GetDecoration(gameId);
            else
                throw new ArgumentException("Could not find VillageObject in this Village with game ID '" + gameId + "'.", "gameId");
        }

        /// <summary>
        /// Returns a JSON string that represents the current <see cref="Village"/>.
        /// </summary>
        /// <returns>A JSON string that represents the current <see cref="Village"/>.</returns>
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
            return indent == true ? JsonConvert.SerializeObject(this, Formatting.Indented) : JsonConvert.SerializeObject(this);
        }

        /// <summary>
        /// Returns a <see cref="Village"/> that will be deserialize from the specified
        /// JSON string.
        /// </summary>
        /// <param name="value">JSON string that represents the <see cref="Village"/>.</param>
        /// <returns>A <see cref="Village"/> that is deserialized from the specified JSON string.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is null or empty.</exception>
        public static Village FromJson(string value)
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentNullException("value");

            var village = JsonConvert.DeserializeObject<Village>(value);
            village.DeserializedJson = value;

            // Schedule constructions of Village Objects so that
            // it execute logic stuff.
            for (int i = 0; i < village.Buildings.Count; i++)
            {
                var building = village.Buildings[i];

                // If the building is in construction schdule it with its current loaded construction time.
                if (building.IsConstructing)
                    building.InternalScheduleBuild();
            }

            return village;
        }
    }
}
