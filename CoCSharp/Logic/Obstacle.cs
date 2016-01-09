using CoCSharp.Data;
using Newtonsoft.Json;
using System;

namespace CoCSharp.Logic
{
    /// <summary>
    /// Represents a Clash of Clans obstacle.
    /// </summary>
    public class Obstacle : VillageObject
    {
        /// <summary>
        /// Represents the base game ID of an <see cref="Obstacle"/>. This field
        /// is constant.
        /// </summary>
        public const int BaseGameID = 503000000;

        /// <summary>
        /// Represents the base data ID of an <see cref="Obstacle"/>. This field
        /// is constant.
        /// </summary>
        public const int BaseDataID = 8000000;

        /// <summary>
        /// Initializes a new instance of the <see cref="Obstacle"/> class.
        /// </summary>
        public Obstacle() : base()
        {
            // Space
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Obstacle"/> class
        /// with the specified data ID.
        /// </summary>
        /// <param name="dataID">Data ID of the <see cref="Obstacle"/>.</param>
        public Obstacle(int dataID) : base(dataID)
        {
            // Space
        }

        /// <summary>
        /// Gets or sets the loot multiplier of the <see cref="Obstacle"/>.
        /// </summary>
        [JsonProperty("loot_multiply_ver", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int LootMultiplier { get; set; }

        [JsonProperty("clear_t", DefaultValueHandling = DefaultValueHandling.Ignore)]
        private int _clearTime;
        /// <summary>
        /// Gets or sets the <see cref="TimeSpan"/> of the clear time
        /// of the building.
        /// </summary>
        [JsonIgnore]
        public TimeSpan ClearTime
        {
            get { return TimeSpan.FromSeconds(_clearTime + 100); }
            set { _clearTime = (int)value.TotalSeconds; }
        }

        /// <summary>
        /// Determines if the specified game ID is valid for an
        /// <see cref="Obstacle"/>.
        /// </summary>
        /// <param name="id">Game ID to validate.</param>
        /// <returns>Returns <c>true</c> if the game ID specified is valid.</returns>
        public static bool ValidGameID(int id)
        {
            return !(id < BaseGameID || id > 504000000); // 504000000 is Traps BaseDataID
        }

        /// <summary>
        /// Converts the specified game ID into an index to the <see cref="Building"/>
        /// in a <see cref="Village.Buildings"/>.
        /// </summary>
        /// <param name="id">Game ID to convert.</param>
        /// <returns>Returns the index of the <see cref="ObsoleteAttribute"/> in <see cref="Village.Obstacles"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="id"/> is no the range of 503000000 &lt; id &lt; 504000000.</exception>
        public static int GameIDToIndex(int id)
        {
            if (!ValidGameID(id)) 
                throw new ArgumentOutOfRangeException("Game ID must be in the range of 503000000 < id < 504000000");

            return id - BaseGameID;
        }

        /// <summary>
        /// Determines if the specified data ID is valid for an
        /// <see cref="Obstacle"/>.
        /// </summary>
        /// <param name="id">Data ID to validate.</param>
        /// <returns>Returns <c>true</c> if the game ID specified is valid.</returns>
        public static bool ValidDataID(int id)
        {
            return !(id < BaseDataID || id > 9000000);
        }

        /// <summary>
        /// Converts the specified game ID into an index to the <see cref="ObstacleData"/>
        /// in a deserialized <see cref="ObstacleData"/> array.
        /// </summary>
        /// <param name="id">Data ID to convert.</param>
        /// <returns>Reurns the index of the <see cref="Obstacle"/> in a deserialized <see cref="ObstacleData"/> array.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="id"/> is no the range of 503000000 &lt; id &lt; 504000000.</exception>
        public static int DataIDToIndex(int id)
        {
            if (!ValidDataID(id))
                throw new ArgumentOutOfRangeException("Game ID must be in the range of " + BaseDataID + " < id < 9000000");

            return id - BaseDataID;
        }
    }
}
