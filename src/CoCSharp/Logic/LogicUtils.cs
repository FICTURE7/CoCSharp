using CoCSharp.Csv;
using CoCSharp.Data;
using CoCSharp.Data.Models;
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
        public static int CalculateExpPoints(TimeSpan duration)
        {
            return CalculateExperience((int)duration.TotalSeconds);
        }

        /// <summary>
        /// Calculates the amount of gems needed to speed up for the specified duration.
        /// </summary>
        /// <param name="assets"><see cref="AssetManager"/> from which to obtain <see cref="GlobalData"/>.</param>
        /// <param name="duration">Duration of the operation.</param>
        /// <returns>Gems needed to speed up.</returns>
        public static int CalculateSpeedUpCost(AssetManager assets, TimeSpan duration)
        {
            if (assets == null)
                throw new ArgumentNullException(nameof(assets));

            var globals = assets.Get<CsvDataTable<GlobalData>>();
            var seconds = (int)duration.TotalSeconds;
            var cost1min = globals.Rows["SPEED_UP_DIAMOND_COST_1_MIN"][0].NumberValue;
            var cost1hour = globals.Rows["SPEED_UP_DIAMOND_COST_1_HOUR"][0].NumberValue;
            var cost1day = globals.Rows["SPEED_UP_DIAMOND_COST_24_HOURS"][0].NumberValue;
            var cost1week = globals.Rows["SPEED_UP_DIAMOND_COST_1_WEEK"][0].NumberValue;

            const int _1MIN_SECS = 60;
            const int _1HOUR_SECS = 60 * _1MIN_SECS;
            const int _1DAY_SECS = 24 * _1HOUR_SECS;
            const int _1WEEK_SECS = 7 * _1DAY_SECS;

            var hCost = 0;
            var lCost = 0;

            var hTime = 0;
            var lTime = 0;

            // 1 Minute.
            if (seconds <= _1MIN_SECS)
            {
                return cost1min;
            }
            // 1 Hour.
            else if (seconds <= _1HOUR_SECS)
            {
                hCost = cost1hour;
                lCost = cost1min;
                hTime = _1HOUR_SECS;
                lTime = _1MIN_SECS;
            }
            // 1 Day.
            else if (seconds <= _1DAY_SECS)
            {
                hCost = cost1day;
                lCost = cost1hour;
                hTime = _1DAY_SECS;
                lTime = _1HOUR_SECS;
            }
            // 1 Week.
            else if (seconds >= _1DAY_SECS)
            {
                hCost = cost1week;
                lCost = cost1day;
                hTime = _1WEEK_SECS;
                lTime = _1DAY_SECS;
            }

            return (int)Math.Round((double)((hCost - lCost) * (seconds - lTime) / (hTime - lTime))) + lCost;
        }

        public static int CalculateExpLevel(AssetManager assets, ref int expLevel, ref int expPoints)
        {
            if (assets == null)
                throw new ArgumentNullException(nameof(assets));

            var expLevelTable = assets.Get<CsvDataTable<ExperienceLevelData>>();
            var curData = expLevelTable.Rows[expLevel.ToString()][0];

            // Incase the player has reached max level.
            var nextDataRow = expLevelTable.Rows[(expLevel + 1).ToString()];
            if (nextDataRow == null)
                return expLevel;

            var nextData = nextDataRow[0];
            if (expPoints >= nextData.ExpPoints)
            {
                expLevel++;
                expPoints -= curData.ExpPoints;
            }

            return expLevel;
        }
    }
}
