using System;

namespace CoCSharp.Logic
{
    /// <summary>
    /// Provides methods related to Clash of Clans logic.
    /// </summary>
    public static class LogicUtils
    {
        /// <summary>
        /// Calculates the amount of experience gained for the specified amount of seconds.
        /// </summary>
        /// <param name="seconds">Amount of seconds.</param>
        /// <returns>Experience gained.</returns>
        public static int CalculateExperience(int seconds)
        {
            return (int)Math.Sqrt(seconds);
        }

        /// <summary>
        /// Calculates the amount of experience gained for the specified duration.
        /// </summary>
        /// <param name="duration">Duration of the operation.</param>
        /// <returns>Experience gained.</returns>
        public static int CalculateExperience(TimeSpan duration)
        {
            return CalculateExperience((int)duration.TotalSeconds);
        }
    }
}
