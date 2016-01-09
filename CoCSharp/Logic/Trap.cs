using CoCSharp.Data;
using Newtonsoft.Json;
using System;

namespace CoCSharp.Logic
{
    /// <summary>
    /// Represents a Clash of Clans trap.
    /// </summary>
    public class Trap : VillageObject
    {
        /// <summary>
        /// Represents the base game ID of an <see cref="Trap"/>. This field
        /// is constant.
        /// </summary>
        public const int BaseGameID = 504000000;

        /// <summary>
        /// Represents the base data ID of an <see cref="Trap"/>. This field
        /// is constant.
        /// </summary>
        public const int BaseDataID = 12000000;

        /// <summary>
        /// Initializes a new instance of the <see cref="Trap"/> class.
        /// </summary>
        public Trap() : base()
        {
            // Space
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Trap"/> class
        /// with the specified data ID.
        /// </summary>
        /// <param name="dataID">Data ID of the <see cref="Trap"/>.</param>
        public Trap(int dataID) : base(dataID)
        {
            // Space
        }

        /// <summary>
        /// Gets or sets whether the trap needs to be repaired.
        /// </summary>
        [JsonProperty("need_repair", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool Broken { get; set; }

        /// <summary>
        /// Determines if the specified game ID is valid for an
        /// <see cref="Trap"/>.
        /// </summary>
        /// <param name="id">Game ID to validate.</param>
        /// <returns>Returns <c>true</c> if the game ID specified is valid.</returns>
        public static bool ValidGameID(int id)
        {
            return !(id < BaseGameID || id > 504000000); // 504000000 is Traps BaseDataID
        }

        /// <summary>
        /// Converts the specified game ID into an index to the <see cref="Trap"/>
        /// in a <see cref="Village.Buildings"/>.
        /// </summary>
        /// <param name="id">Game ID to convert.</param>
        /// <returns>Returns the index of the <see cref="Trap"/> in <see cref="Village.Traps"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="id"/> is no the range of 503000000 &lt; id &lt; 504000000.</exception>
        public static int GameIDToIndex(int id)
        {
            if (ValidGameID(id))
                throw new ArgumentOutOfRangeException("Game ID must be in the range of 503000000 < id < 504000000");

            return id - BaseGameID;
        }

        /// <summary>
        /// Determines if the specified data ID is valid for a
        /// <see cref="Trap"/>.
        /// </summary>
        /// <param name="id">Data ID to validate.</param>
        /// <returns>Returns <c>true</c> if the game ID specified is valid.</returns>
        public static bool ValidDataID(int id)
        {
            return !(id < BaseDataID || id > 9000000);
        }

        /// <summary>
        /// Converts the specified game ID into an index to the <see cref="TrapData"/>
        /// in a deserialized <see cref="TrapData"/> array.
        /// </summary>
        /// <param name="id">Data ID to convert.</param>
        /// <returns>Reurns the index of the <see cref="Trap"/> in a deserialized <see cref="TrapData"/> array.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="id"/> is no the range of 503000000 &lt; id &lt; 504000000.</exception>
        public static int DataIDToIndex(int id)
        {
            if (ValidDataID(id))
                throw new ArgumentOutOfRangeException("Game ID must be in the range of " + BaseDataID + " < id < 504000000");

            return id - BaseDataID;
        }
    }
}
