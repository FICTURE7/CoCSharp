using CoCSharp.Logic;
using System;

namespace CoCSharp
{
    /// <summary>
    /// Provides methods to convert Clash of Clans identifiers.
    /// </summary>
    public static class IDConverter
    {
        private const int Base = 1000000;

        /// <summary>
        /// Converts the specified data ID into its index equivalent.
        /// </summary>
        /// <param name="dataId">Data ID to convert.</param>
        /// <returns>Index converted from the data ID.</returns>
        public static int ToIndex(int dataId)
        {
            return dataId % Base;
        }

        /// <summary>
        /// Converts the specified index into its data ID equivalent.
        /// </summary>
        /// <typeparam name="T">Type of <see cref="VillageObject"/>.</typeparam>
        /// <param name="index">Index to convert.</param>
        /// <returns>Data ID converted from the index.</returns>
        public static int ToData<T>(int index) where T : VillageObject
        {
            var instance = Activator.CreateInstance<T>(); // creating new instances is probs a bad idea
            return instance.BaseDataID + index;
        }

        /// <summary>
        /// Converts the specified index into its game ID equivalent.
        /// </summary>
        /// <typeparam name="T">Type of <see cref="VillageObject"/>.</typeparam>
        /// <param name="index">Index to convert.</param>
        /// <returns>Game ID converted from the index.</returns>
        public static int ToGame<T>(int index) where T : VillageObject
        {
            var instance = Activator.CreateInstance<T>();
            return instance.BaseGameID + index;
        }

        /// <summary>
        /// Determines if the specified game ID is valid for the specified
        /// <see cref="VillageObject"/> type.
        /// </summary>
        /// <typeparam name="T">Type of <see cref="VillageObject"/>.</typeparam>
        /// <param name="gameId">Game ID to check.</param>
        /// <returns>Returns <c>true</c> if the specified game ID is valid else returns <c>false</c>.</returns>
        public static bool IsValidGame<T>(int gameId) where T : VillageObject
        {
            var instance = Activator.CreateInstance<T>();
            return instance.BaseGameID <= gameId && gameId <= instance.BaseGameID + Base;
        }

        /// <summary>
        /// Determines if the specified data ID is valid for the specified
        /// <see cref="VillageObject"/> type.
        /// </summary>
        /// <typeparam name="T">Type of <see cref="VillageObject"/>.</typeparam>
        /// <param name="dataId">Data ID to check.</param>
        /// <returns>Returns <c>true</c> if the specified data ID is valid else returns <c>false</c>.</returns>
        public static bool IsValidData<T>(int dataId) where T : VillageObject
        {
            var instance = Activator.CreateInstance<T>();
            return instance.BaseDataID <= dataId && dataId <= instance.BaseDataID + Base;
        }
    }
}
